// See https://aka.ms/new-console-template for more information



using NumberPuzzle_Team_3.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text;




byte[,] Tablero = new byte[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
List<Nodo> nodos = new List<Nodo>();



void Inicializar()
{
    Tablero = new byte[3, 3] { { 1, 2, 0 }, { 4, 6, 3 }, { 7, 5, 8 } };
    nodos = new List<Nodo>();

    Nodo nodoInicial = new Nodo()
    {
        Estado = TableroToKey(Tablero),
        Padre = null,
        Costo = CalcularHeuristica(Tablero),
        Heuristica = CalcularHeuristica(Tablero),
        Pasos = 0
    };
    nodos.Add(nodoInicial);
    generarNodos(nodoInicial);

}


//funciones
void ImprimirTablero(byte[,] tablero)
{
    for (int i = 0; i < tablero.GetLength(0); i++)
    {
        for (int j = 0; j < tablero.GetLength(1); j++)
        {
            Console.Write(tablero[i, j] + " ");
        }
        Console.WriteLine();
    }
}

string TableroToKey(byte[,] tablero)
{
    StringBuilder sb = new StringBuilder(9);
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            sb.Append(tablero[i, j]);
    return sb.ToString();
}

int CalcularHeuristica(byte[,] estado)
{
    int heuristica =0;
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (estado[i, j] != 0)
            {
                int col = (estado[i, j] - 1) % 3;

                int row = (estado[i, j] - 1) / 3;
                heuristica += Math.Abs(i - row) + Math.Abs(j - col);
            }
        }
    }
    return heuristica;
}



byte[,] KeyToTablero(string tablero)
{
    byte[,] result = new byte[3, 3];
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            result[i, j] = byte.Parse(tablero[i * 3 + j].ToString());
    return result;
}

Nodo generarNodo(byte[,] estado, Nodo? padre, int heuristica, int pasos)
{
    Nodo nodo = new Nodo()
    {
        Estado = TableroToKey(estado),
        Padre = padre,
        Costo = heuristica + pasos,
        Heuristica = heuristica,
        Pasos = pasos



    };
    return nodo;
}



void generarNodos(Nodo padre)
{
    byte[,] estado = KeyToTablero(padre.Estado);

    //buscar 0
    int fila0 = 0, col0 = 0;
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (estado[i, j] == 0)
            {
                fila0 = i;
                col0 = j;
            }
        }
    }
    //mover arriba
    if (fila0 > 0)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0 - 1, col0];
        nuevoEstado[fila0 - 1, col0] = 0;
        string key = TableroToKey(nuevoEstado);
        if (!nodos.Any(n => n.Estado == key))
        {
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado), padre.Pasos + 1);
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {
            //agregar aqui una parte recursiva para actualizar los nodos hijos
        }

    }
    //mover abajo
    if (fila0 < 2)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0 + 1, col0];
        nuevoEstado[fila0 + 1, col0] = 0;
        string key = TableroToKey(nuevoEstado);
        if (!nodos.Any(n => n.Estado == key))
        {
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado), padre.Pasos + 1);
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {
            //agregar aqui una parte recursiva para actualizar los nodos hijos
        }
    }
    //mover izquierda
    if (col0 > 0)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 - 1];
        nuevoEstado[fila0, col0 - 1] = 0;
        string key = TableroToKey(nuevoEstado);
        if (!nodos.Any(n => n.Estado == key))
        {
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado), padre.Pasos + 1);
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {
            //agregar aqui una parte recursiva para actualizar los nodos hijos
        }
    }
    //mover derecha
    if (col0 < 2)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 + 1];
        nuevoEstado[fila0, col0 + 1] = 0;
        string key = TableroToKey(nuevoEstado);
        if (!nodos.Any(n => n.Estado == key))
        {
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado), padre.Pasos + 1);
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {
            //agregar aqui una parte recursiva para actualizar los nodos hijos
        }
    }

    //elegir el nodo con menor costo
    Nodo? siguienteNodo = nodos.OrderBy(n => n.Costo).FirstOrDefault();
    if (siguienteNodo != null && siguienteNodo.Heuristica > 0)
    {
        nodos.Remove(siguienteNodo);
        generarNodos(siguienteNodo);
    }
    else
    {
        Console.WriteLine("Solucion encontrada");
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
            ImprimirTablero(KeyToTablero(paso.Estado));
            Console.WriteLine();
        }
    }




}



Inicializar();







Nodo nodo = new Nodo();

Console.WriteLine("Hello, World!");