using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle_grafico.Models
{
    public partial class ViewModel : ObservableObject
    {
        byte[,] Tablero = new byte[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
        public ObservableCollection<Cuadro> TableroWPF { set; get; } = new ObservableCollection<Cuadro>();

        List<Nodo> nodos = new List<Nodo>();
        int tamaño = 100;

        public ViewModel()
        {
            TableroWPF.Add(new Cuadro() { Fila = 0, Columna = 0, Valor = 1 });
            TableroWPF.Add(new Cuadro() { Fila = 0, Columna = 1 * tamaño, Valor = 2 });
            TableroWPF.Add(new Cuadro() { Fila = 0, Columna = 2 * tamaño, Valor = 3 });
            TableroWPF.Add(new Cuadro() { Fila = 1 * tamaño, Columna = 0, Valor = 4 });
            TableroWPF.Add(new Cuadro() { Fila = 1 * tamaño, Columna = 1 * tamaño, Valor = 5 });
            TableroWPF.Add(new Cuadro() { Fila = 1 * tamaño, Columna = 2 * tamaño, Valor = 6 });
            TableroWPF.Add(new Cuadro() { Fila = 2 * tamaño, Columna = 0, Valor = 7 });
            TableroWPF.Add(new Cuadro() { Fila = 2 * tamaño, Columna = 1 * tamaño, Valor = 8 });
        }


        [RelayCommand]
        async Task EmpezarAsync()
        {
            await InicializarAsync();
        }

        void generarTablero()
        {
            Random rand = new Random();
            int ran = rand.Next(1, 500);
            for (int i = 0; i < ran; i++)
            {
                int direccion = rand.Next(4);
                switch (direccion)
                {
                    case 0: up(); break;
                    case 1: down(); break;
                    case 2: left(); break;
                    case 3: right(); break;
                }
            }
            ImprimirTablero(Tablero);
        }

        void left()
        {
            int fila0 = 0, col0 = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Tablero[i, j] == 0)
                    {
                        fila0 = i;
                        col0 = j;
                    }
            if (col0 > 0)
            {
                Tablero[fila0, col0] = Tablero[fila0, col0 - 1];
                Tablero[fila0, col0 - 1] = 0;
            }
        }

        void right()
        {
            int fila0 = 0, col0 = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Tablero[i, j] == 0)
                    {
                        fila0 = i;
                        col0 = j;
                    }
            if (col0 < 2)
            {
                Tablero[fila0, col0] = Tablero[fila0, col0 + 1];
                Tablero[fila0, col0 + 1] = 0;
            }
        }

        void up()
        {
            int fila0 = 0, col0 = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Tablero[i, j] == 0)
                    {
                        fila0 = i;
                        col0 = j;
                    }
            if (fila0 > 0)
            {
                Tablero[fila0, col0] = Tablero[fila0 - 1, col0];
                Tablero[fila0 - 1, col0] = 0;
            }
        }

        void down()
        {
            int fila0 = 0, col0 = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Tablero[i, j] == 0)
                    {
                        fila0 = i;
                        col0 = j;
                    }
            if (fila0 < 2)
            {
                Tablero[fila0, col0] = Tablero[fila0 + 1, col0];
                Tablero[fila0 + 1, col0] = 0;
            }
        }

        // Funciones auxiliares
        string TableroToKey(byte[,] tablero)
        {
            StringBuilder sb = new StringBuilder(9);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    sb.Append(tablero[i, j]);
            return sb.ToString();
        }

        byte[,] KeyToTablero(string tablero)
        {
            byte[,] result = new byte[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    result[i, j] = byte.Parse(tablero[i * 3 + j].ToString());
            return result;
        }

        // Nueva versión async para UI
        async Task ImprimirTableroAsync(byte[,] tablero)
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i, j] != 0)
                    {
                        var cuadro = TableroWPF[tablero[i, j] - 1];
                        double startX = cuadro.Columna;
                        double startY = cuadro.Fila;
                        double targetX = j * tamaño;
                        double targetY = i * tamaño;

                        int pasos = 10; // más pasos = movimiento más suave

                        for (int k = 1; k <= pasos; k++)
                        {
                            cuadro.Columna = (int)(startX + (targetX - startX) * k / pasos);
                            cuadro.Fila = (int)(startY + (targetY - startY) * k / pasos);
                            await Task.Delay(5); // 15 ms por paso para suavidad
                        }
                    }
                }
            }

//            await Task.Delay(10);
        }

        void ImprimirTablero(byte[,] tablero)
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
                for (int j = 0; j < tablero.GetLength(1); j++)
                    if (tablero[i, j] != 0)
                    {
                        TableroWPF[tablero[i, j] - 1].Columna = j * tamaño;
                        TableroWPF[tablero[i, j] - 1].Fila = i * tamaño;
                    }
        }

        int CalcularHeuristica(byte[,] estado)
        {
            int heuristica = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (estado[i, j] != 0)
                    {
                        int col = (estado[i, j] - 1) % 3;
                        int row = (estado[i, j] - 1) / 3;
                        heuristica += Math.Abs(i - row) + Math.Abs(j - col);
                    }
            return heuristica;
        }

        async Task Juego()
        {
            bool completado = false;
            

            while (completado == false)
            {
                Nodo? siguienteNodo = nodos.OrderBy(n => n.Costo).FirstOrDefault(x => !x.Visitado);

                if (siguienteNodo != null && siguienteNodo.Heuristica > 0)
                {
                    await generarNodosAsync(siguienteNodo);
                }
                else
                {
                    
                    List<Nodo> solucion = new List<Nodo>();
                    Nodo? actual = siguienteNodo;
                    while (actual != null)
                    {
                        solucion.Add(actual);
                        actual = actual.Padre;
                    }
                    solucion.Reverse();

                    foreach (var paso in solucion)
                    {
                        await ImprimirTableroAsync(KeyToTablero(paso.Estado));
                    }
                    completado = true;
                }

            }
        }

        void generarNodo(byte[,] estado, Nodo? padre)
        {
            string key = TableroToKey(estado);
            if (!nodos.Any(n => n.Estado == key))
            {
                int heuristica = CalcularHeuristica(estado);
                Nodo nodo = new Nodo()
                {
                    Estado = TableroToKey(estado),
                    Padre = padre,
                    Costo = heuristica + padre.Pasos + 1,
                    Heuristica = heuristica,
                    Pasos = padre.Pasos + 1,
                };
                nodos.Add(nodo);
            }
            else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
            {
                Nodo nodo1 = nodos.First(n => n.Estado == key);
                nodo1.Padre = padre;
                ActualizarNodosHijos(nodo1);
            }
        }

        void ActualizarNodosHijos(Nodo nodo)
        {
            nodo.Pasos = nodo.Padre != null ? nodo.Padre.Pasos + 1 : 0;
            nodo.Costo = nodo.Heuristica + nodo.Pasos;

            var hijos = nodos.Where(n => n.Padre == nodo).ToList();
            foreach (var hijo in hijos)
            {
                hijo.Padre = nodo;
                ActualizarNodosHijos(hijo);
            }
        }

        // Async version para animación
        async Task generarNodosAsync(Nodo padre)
        {
            byte[,] estado = KeyToTablero(padre.Estado);
            padre.Visitado = true;

            int fila0 = 0, col0 = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (estado[i, j] == 0)
                    {
                        fila0 = i;
                        col0 = j;
                    }

            if (fila0 > 0)
            {
                byte[,] nuevoEstado = (byte[,])estado.Clone();
                nuevoEstado[fila0, col0] = nuevoEstado[fila0 - 1, col0];
                nuevoEstado[fila0 - 1, col0] = 0;
                generarNodo(nuevoEstado, padre);
            }
            if (fila0 < 2)
            {
                byte[,] nuevoEstado = (byte[,])estado.Clone();
                nuevoEstado[fila0, col0] = nuevoEstado[fila0 + 1, col0];
                nuevoEstado[fila0 + 1, col0] = 0;
                generarNodo(nuevoEstado, padre);
            }
            if (col0 > 0)
            {
                byte[,] nuevoEstado = (byte[,])estado.Clone();
                nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 - 1];
                nuevoEstado[fila0, col0 - 1] = 0;
                generarNodo(nuevoEstado, padre);
            }
            if (col0 < 2)
            {
                byte[,] nuevoEstado = (byte[,])estado.Clone();
                nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 + 1];
                nuevoEstado[fila0, col0 + 1] = 0;
                generarNodo(nuevoEstado, padre);
            }

          
        }

        async Task InicializarAsync()
        {
            nodos = new List<Nodo>();
            generarTablero();
            await ImprimirTableroAsync(Tablero);

            Nodo nodoInicial = new Nodo()
            {
                Estado = TableroToKey(Tablero),
                Padre = null,
                Costo = CalcularHeuristica(Tablero),
                Heuristica = CalcularHeuristica(Tablero),
                Pasos = 0
            };
            nodos.Add(nodoInicial);
            await Juego();
        }
    }
}
