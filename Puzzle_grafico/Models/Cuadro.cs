using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle_grafico.Models
{
    public partial class Cuadro:ObservableObject
    {
        [ObservableProperty]
        private int fila;
        [ObservableProperty]
        private int columna;
        public int Valor { get; set; }
       
    }
}
