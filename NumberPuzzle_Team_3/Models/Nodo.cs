using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberPuzzle_Team_3.Models
{
    public class Nodo
    {
        public string Estado;
        public Nodo? Padre;
        public int Costo;
        public int Heuristica;
        public int Pasos;
        public bool Visitado = false;


    }
}
