// See https://aka.ms/new-console-template for more information



using NumberPuzzle_Team_3.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text;




byte[,] Tablero = new byte[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
List<Nodo> nodos = new List<Nodo>();


void generarTablero() {
    Random rand = new Random();

    int ran = rand.Next(1,100);
    for (int i = 0; i < ran; i++)
    {
        int direccion = rand.Next(4);
        switch (direccion)
        {
            case 0:
                up();
                break;
            case 1:
                down();
                break;
            case 2:
                left();
                break;
            case 3:
                right();
                break;
        }
    }
    
    Console.WriteLine();


}

void left()
{
    int fila0 = 0, col0 = 0;
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            if (Tablero[i, j] == 0)
            {
                fila0 = i;
                col0 = j;
            }
        }
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
    {
        for (int j = 0; j < 3; j++)
        {
            if (Tablero[i, j] == 0)
            {
                fila0 = i;
                col0 = j;
            }
        }
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
    {
        for (int j = 0; j < 3; j++)
        {
            if (Tablero[i, j] == 0)
            {
                fila0 = i;
                col0 = j;
            }
        }
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
    {
        for (int j = 0; j < 3; j++)
        {
            if (Tablero[i, j] == 0)
            {
                fila0 = i;
                col0 = j;
            }
        }
    }
    if (fila0 < 2)
    {
        Tablero[fila0, col0] = Tablero[fila0 + 1, col0];
        Tablero[fila0 + 1, col0] = 0;
    }
}
TimeSpan delay = TimeSpan.FromMilliseconds(500);





//funciones
void ImprimirTablero(byte[,] tablero)
{
    for (int i = 0; i < tablero.GetLength(0); i++)
    {
        for (int j = 0; j < tablero.GetLength(1); j++)
        {
            Console.ForegroundColor = tablero[i, j] == 0 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(tablero[i, j] + " ");
            Console.ForegroundColor = ConsoleColor.White;

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


byte[,] KeyToTablero(string tablero)
{
    byte[,] result = new byte[3, 3];
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            result[i, j] = byte.Parse(tablero[i * 3 + j].ToString());
    return result;
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




Nodo generarNodo(byte[,] estado, Nodo? padre, int heuristica)
{
    Nodo nodo = new Nodo()
    {
        Estado = TableroToKey(estado),
        Padre = padre,
        Costo = heuristica + padre.Pasos+1,
        Heuristica = heuristica,
        Pasos = padre.Pasos + 1,



    };
    return nodo;
}

void ActualizarNodosHijos(Nodo nodo)
{
    var hijos = nodos.Where(n => n.Padre == nodo).ToList();
    foreach (var hijo in hijos)
    {
        hijo.Costo = hijo.Heuristica + nodo.Pasos + 1;
        hijo.Pasos = nodo.Pasos + 1;
        ActualizarNodosHijos(hijo);
    }
}



void generarNodos(Nodo padre)
{
    byte[,] estado = KeyToTablero(padre.Estado);
    //System.Threading.Thread.Sleep(delay);
    //Console.Clear();
    //Console.WriteLine($"Costo: {padre.Costo}, Heuristica: {padre.Heuristica}, Pasos: {padre.Pasos}");
    //ImprimirTablero(KeyToTablero(padre.Estado));
    padre.Visitado = true;
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
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado));
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {

            ////nodo existente con mayor costo, actualizar;
            //var nodoExistente = nodos.First(n => n.Estado == key);
            //nodoExistente.Padre = padre;

            //ActualizarNodosHijos(nodoExistente);
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
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado));
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
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado));
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
            Nodo nuevoNodo = generarNodo(nuevoEstado, padre, CalcularHeuristica(nuevoEstado));
            nodos.Add(nuevoNodo);
        }
        else if (nodos.Any(n => n.Estado == key && n.Pasos > padre.Pasos + 1))
        {
            //agregar aqui una parte recursiva para actualizar los nodos hijos
        }
    }

    //elegir el nodo con menor costo
    Nodo? siguienteNodo = nodos.OrderBy(n => n.Costo).FirstOrDefault(x=>x.Visitado==false);

    if (siguienteNodo != null && siguienteNodo.Heuristica > 0)
    {
        generarNodos(siguienteNodo);
    }
    else
    {
       // Console.WriteLine("Solucion encontrada");
        List<Nodo> solucion = new List<Nodo>();
        Nodo? actual = siguienteNodo;
        while (actual != null)
        {
            solucion.Add(actual);
            actual = actual.Padre;
        }
        solucion.Reverse();
        Console.Clear();

        foreach (var paso in solucion)
        {
            Console.Clear();
            Console.WriteLine($"Costo: {paso.Costo}, Heuristica: {paso.Heuristica}, Pasos: {paso.Pasos}");
            ImprimirTablero(KeyToTablero(paso.Estado));
            System.Threading.Thread.Sleep(delay);
        }
    }




}

void Inicializar()
{
    Tablero = new byte[3, 3] { { 1, 2, 0 }, { 4, 6, 3 }, { 7, 5, 8 } };

    nodos = new List<Nodo>();
    generarTablero();
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
try
{
    Inicializar();
}
catch (Exception ex)
{
    Inicializar();
}






Nodo nodo = new Nodo();

Console.WriteLine("Hello, World!");