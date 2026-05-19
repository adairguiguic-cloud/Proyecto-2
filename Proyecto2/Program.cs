using System;

namespace GranjaSimulador
{
    public class Parcela
    {
        public string TipoCultivo { get; set; }
        public int MesesCrecimiento { get; set; }
        public int Progreso { get; set; } 
        public bool Regada { get; set; } 
        public int Ingreso { get; set; } 

        public Parcela()
        {
            TipoCultivo = "Vacía";
            MesesCrecimiento = 0;
            Progreso = 0;
            Regada = false;
            Ingreso = 0;
        }

        public bool EstaVacia()
        {
            return TipoCultivo == "Vacía";
        }

        public bool EstaLista()
        {
            return Progreso >= MesesCrecimiento && !EstaVacia();
        }

        public void Limpiar()
        {
            TipoCultivo = "Vacía";
            MesesCrecimiento = 0;
            Progreso = 0;
            Regada = false;
            Ingreso = 0;
        }
    }
    public class Granja
    {
        public double Dinero { get; set; }
        public int Empleados { get; set; }
        public double SueldoEmpleado { get; set; }
        public int MesesRestantes { get; set; }
        public Parcela[,] Parcelas { get; set; }
        public int TotalRiegos { get; set; }
        public int ContadorPapa { get; set; }
        public int ContadorTomate { get; set; }
        public int ContadorFresa { get; set; }
        public int CosechasPapa { get; set; }
        public int CosechasTomate { get; set; }
        public int CosechasFresa { get; set; }
        public double IngresosTotales { get; set; }
        public double EgresosTotales { get; set; }

        public int Filas => Parcelas.GetLength(0);
        public int Columnas => Parcelas.GetLength(1);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== UNIVERSIDAD RAFAEL LANDÍVAR ===");
            Console.WriteLine("=== Simulador de Granja ===\n");
            Granja granja = InicializarGranja();
            while (granja.MesesRestantes > 0 && granja.Dinero > 0)
            {
                MostrarEstadoGranja(granja);
                int opcion = MostrarMenu();

                switch (opcion)
                {
                    case 1: Sembrar(granja); break;
                    case 2: Regar(granja); break;
                    case 3: ConsultarParcela(granja); break;
                    case 4: AvanzarMes(granja); break;
                    case 5: goto FinSimulacion;
                    default: Console.WriteLine("Opción inválida."); break;
                }
            }

        FinSimulacion:
            MostrarReporteFinal(granja);
        }
        static Granja InicializarGranja()
        {
            Granja granja = new Granja();

            Console.Write("Dinero inicial: Q");
            granja.Dinero = LeerDoublePositivo();

            Console.Write("Número de empleados: ");
            granja.Empleados = LeerEnteroPositivo();

            Console.Write("Sueldo por empleado: Q");
            granja.SueldoEmpleado = LeerDoublePositivo();

            Console.Write("Meses a simular: ");
            granja.MesesRestantes = LeerEnteroPositivo();

            Console.Write("Número de filas de la granja: ");
            int filas = LeerEnteroPositivo();

            Console.Write("Número de columnas de la granja: ");
            int columnas = LeerEnteroPositivo();

            granja.Parcelas = new Parcela[filas, columnas];
            for (int i = 0; i < filas; i++)
                for (int j = 0; j < columnas; j++)
                    granja.Parcelas[i, j] = new Parcela();

            Console.WriteLine("\nGranja inicializada correctamente.\n");
            return granja;
        }

        static void MostrarEstadoGranja(Granja granja)
        {
            Console.Clear();
            Console.WriteLine("=== ESTADO ACTUAL DE LA GRANJA ===");
            Console.WriteLine($"Dinero: Q{granja.Dinero:N2} | Meses restantes: {granja.MesesRestantes}");
            Console.WriteLine($"Empleados: {granja.Empleados} | Sueldo total: Q{granja.Empleados * granja.SueldoEmpleado:N2}");
            Console.WriteLine();
        }

        static int MostrarMenu()
        {
            Console.WriteLine("MENÚ:");
            Console.WriteLine("1. Sembrar parcela");
            Console.WriteLine("2. Regar parcela");
            Console.WriteLine("3. Consultar parcela");
            Console.WriteLine("4. Avanzar mes");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione opción: ");
            return LeerEntero(1, 5);
        }

        static void Sembrar(Granja granja)
        {
            Console.Write("Fila (0-{0}): ", granja.Filas - 1);
            int fila = LeerEntero(0, granja.Filas - 1);

            Console.Write("Columna (0-{0}): ", granja.Columnas - 1);
            int col = LeerEntero(0, granja.Columnas - 1);

            Parcela parcela = granja.Parcelas[fila, col];
            if (!parcela.EstaVacia())
            {
                Console.WriteLine("¡ERROR! La parcela ya tiene un cultivo.");
                return;
            }

            Console.WriteLine("1. Papa (2 meses, Q450)\n2. Tomate (3 meses, Q650)\n3. Fresa (4 meses, Q900)");
            Console.Write("Seleccione cultivo: ");
            int opcion = LeerEntero(1, 3);

            switch (opcion)
            {
                case 1:
                    parcela.TipoCultivo = "Papa";
                    parcela.MesesCrecimiento = 2;
                    parcela.Ingreso = 450;
                    granja.ContadorPapa++;
                    break;
                case 2:
                    parcela.TipoCultivo = "Tomate";
                    parcela.MesesCrecimiento = 3;
                    parcela.Ingreso = 650;
                    granja.ContadorTomate++;
                    break;
                case 3:
                    parcela.TipoCultivo = "Fresa";
                    parcela.MesesCrecimiento = 4;
                    parcela.Ingreso = 900;
                    granja.ContadorFresa++;
                    break;
            }

            Console.WriteLine($"¡Sembrada {parcela.TipoCultivo} en [{fila},{col}]!");
        }

        static void Regar(Granja granja)
        {
            Console.Write("Fila (0-{0}): ", granja.Filas - 1);
            int fila = LeerEntero(0, granja.Filas - 1);

            Console.Write("Columna (0-{0}): ", granja.Columnas - 1);
            int col = LeerEntero(0, granja.Columnas - 1);

            Parcela parcela = granja.Parcelas[fila, col];

            if (parcela.EstaVacia())
            {
                Console.WriteLine("¡ERROR! No se puede regar una parcela vacía.");
                return;
            }

            if (parcela.Regada)
            {
                Console.WriteLine("¡ERROR! Esta parcela ya fue regada este mes.");
                return;
            }

            if (granja.Dinero < 40)
            {
                Console.WriteLine("¡ERROR! No hay suficiente dinero para regar (Q40).");
                return;
            }

            granja.Dinero -= 40;
            granja.EgresosTotales += 40;
            parcela.Regada = true;
            granja.TotalRiegos++;
            Console.WriteLine($"¡Parcelas [{fila},{col}] regada! Costo: Q40");
        }

        static void ConsultarParcela(Granja granja)
        {
            Console.Write("Fila (0-{0}): ", granja.Filas - 1);
            int fila = LeerEntero(0, granja.Filas - 1);

            Console.Write("Columna (0-{0}): ", granja.Columnas - 1);
            int col = LeerEntero(0, granja.Columnas - 1);

            Parcela parcela = granja.Parcelas[fila, col];

            if (parcela.EstaVacia())
            {
                Console.WriteLine("Parcela vacía.");
                return;
            }

            Console.WriteLine($"Cultivo: {parcela.TipoCultivo}");
            Console.WriteLine($"Progreso: {parcela.Progreso}/{parcela.MesesCrecimiento}");
            Console.WriteLine($"Regada este mes: {(parcela.Regada ? "Sí" : "No")}");
            if (parcela.EstaLista()) Console.WriteLine("¡LISTA PARA COSECHAR!");
        }

        static void AvanzarMes(Granja granja)
        {
            Console.WriteLine("\n--- AVANZANDO MES ---");

            // Pago a empleados
            double sueldoTotal = granja.Empleados * granja.SueldoEmpleado;
            if (granja.Dinero < sueldoTotal)
            {
                Console.WriteLine("¡No hay suficiente dinero para pagar empleados! Fin de simulación.");
                granja.Dinero = 0;
                return;
            }

            granja.Dinero -= sueldoTotal;
            granja.EgresosTotales += sueldoTotal;
            Console.WriteLine($"Pagados empleados: -Q{sueldoTotal:N2}");

            // Crecimiento y cosecha
            int cosechasEsteMes = 0;
            for (int i = 0; i < granja.Filas; i++)
            {
                for (int j = 0; j < granja.Columnas; j++)
                {
                    Parcela parcela = granja.Parcelas[i, j];
                    if (!parcela.EstaVacia())
                    {
                        int incremento = parcela.Regada ? 2 : 1;
                        parcela.Progreso += incremento;

                        if (parcela.EstaLista())
                        {
                            granja.Dinero += parcela.Ingreso;
                            granja.IngresosTotales += parcela.Ingreso;

                            switch (parcela.TipoCultivo)
                            {
                                case "Papa": granja.CosechasPapa++; break;
                                case "Tomate": granja.CosechasTomate++; break;
                                case "Fresa": granja.CosechasFresa++; break;
                            }

                            Console.WriteLine($"¡COSECHA! {parcela.TipoCultivo} en [{i},{j}] +Q{parcela.Ingreso}");
                            parcela.Limpiar();
                            cosechasEsteMes++;
                        }
                    }
                }
            }

            // Reiniciar riego
            for (int i = 0; i < granja.Filas; i++)
                for (int j = 0; j < granja.Columnas; j++)
                    granja.Parcelas[i, j].Regada = false;

            granja.MesesRestantes--;
            Console.WriteLine($"Mes completado. Cosechas este mes: {cosechasEsteMes}");
            Console.WriteLine($"Dinero actual: Q{granja.Dinero:N2}\n");
        }

        static void MostrarReporteFinal(Granja granja)
        {
            Console.Clear();
            Console.WriteLine("=== REPORTE FINAL ===");
            Console.WriteLine($"Dinero final: Q{granja.Dinero:N2}");
            Console.WriteLine($"Total ingresos: Q{granja.IngresosTotales:N2}");
            Console.WriteLine($"Total egresos: Q{granja.EgresosTotales:N2}");
            Console.WriteLine($"Meses simulados: {(granja.MesesRestantes == 0 ? "Todos" : "Interrumpidos")}\n");

            Console.WriteLine("Cosechas realizadas:");
            Console.WriteLine($"  Papa: {granja.CosechasPapa}");
            Console.WriteLine($"  Tomate: {granja.CosechasTomate}");
            Console.WriteLine($"  Fresa: {granja.CosechasFresa}\n");

            Console.WriteLine("Parcelas sembradas:");
            Console.WriteLine($"  Papa: {granja.ContadorPapa}");
            Console.WriteLine($"  Tomate: {granja.ContadorTomate}");
            Console.WriteLine($"  Fresa: {granja.ContadorFresa}\n");

            Console.WriteLine($"Total riegos realizados: {granja.TotalRiegos}");

            int vacias = 0;
            for (int i = 0; i < granja.Filas; i++)
                for (int j = 0; j < granja.Columnas; j++)
                    if (granja.Parcelas[i, j].EstaVacia()) vacias++;

            Console.WriteLine($"Parcelas vacías al final: {vacias}/{granja.Filas * granja.Columnas}");
            Console.WriteLine("\n¡Gracias por usar el Simulador de Granja!");
            Console.ReadKey();
        }

        // Métodos de validación
        static int LeerEntero(int min, int max)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int valor) && valor >= min && valor <= max)
                    return valor;
                Console.Write($"Ingrese un número válido ({min}-{max}): ");
            }
        }

        static int LeerEnteroPositivo()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int valor) && valor > 0)
                    return valor;
                Console.Write("Ingrese un número positivo: ");
            }
        }

        static double LeerDoublePositivo()
        {
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out double valor) && valor >= 0)
                    return valor;
                Console.Write("Ingrese un número válido: ");
            }
        }
    }
}
