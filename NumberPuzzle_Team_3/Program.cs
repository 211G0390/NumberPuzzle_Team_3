// See https://aka.ms/new-console-template for more information



using NumberPuzzle_Team_3.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text;




byte[,] Tablero = new byte[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };
List<Nodo> nodos = new List<Nodo>();


void generarTablero() {
    Random rand = new Random();

    int ran = rand.Next(1,500);
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

//convierte el tablero a una cadena de texto
string TableroToKey(byte[,] tablero)
{
    StringBuilder sb = new StringBuilder(9);
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            sb.Append(tablero[i, j]);
    return sb.ToString();
}

//convierte una cadena de texto a un tablero
byte[,] KeyToTablero(string tablero)
{
    byte[,] result = new byte[3, 3];
    for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            result[i, j] = byte.Parse(tablero[i * 3 + j].ToString());
    return result;
}
//imprime el tablero en consola
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

//calcula la heuristica del tablero
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



//genera un nodo y lo agrega a la lista de nodos si no existe
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
        //agregar aqui una parte recursiva para actualizar los nodos hijos
        Nodo nodo1 = nodos.First(n => n.Estado == key);
        nodo1.Padre = padre;
        ActualizarNodosHijos(nodo1);
    }
}

//actualiza los nodos hijos de un nodo en caso de que se haya encontrado un camino mas corto
void ActualizarNodosHijos(Nodo nodo)
{
    nodo.Pasos= nodo.Padre != null ? nodo.Padre.Pasos + 1 : 0;
    nodo.Costo= nodo.Heuristica + nodo.Pasos;

    var hijos = nodos.Where(n => n.Padre == nodo).ToList();

    foreach (var hijo in hijos)
    {
        hijo.Padre= nodo;
        ActualizarNodosHijos(hijo);
    }
}


//genera los nodos a partir de un nodo padre es el juego central
void generarNodos(Nodo padre)
{
    byte[,] estado = KeyToTablero(padre.Estado);
   
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
        generarNodo(nuevoEstado, padre);


    }
    //mover abajo
    if (fila0 < 2)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0 + 1, col0];
        nuevoEstado[fila0 + 1, col0] = 0;

        generarNodo(nuevoEstado, padre);

        
    }
    //mover izquierda
    if (col0 > 0)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 - 1];
        nuevoEstado[fila0, col0 - 1] = 0;
        
        generarNodo(nuevoEstado, padre);

    }
    //mover derecha
    if (col0 < 2)
    {
        byte[,] nuevoEstado = (byte[,])estado.Clone();
        nuevoEstado[fila0, col0] = nuevoEstado[fila0, col0 + 1];
        nuevoEstado[fila0, col0 + 1] = 0;
        generarNodo(nuevoEstado, padre);

    }

    //elegir el nodo con menor costo
    Nodo? siguienteNodo = nodos.OrderBy(n => n.Costo).FirstOrDefault(x=>x.Visitado==false);

    //verificar si es el correcto
    if (siguienteNodo != null && siguienteNodo.Heuristica > 0)
    {
        // no se encontro la solucion aun asi que seguimos generando
        generarNodos(siguienteNodo);
    }
    else
    {
        // Console.WriteLine("Solucion encontrada");
        //imprimir la solucion
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






Console.WriteLine("Hello, World!");