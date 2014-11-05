using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyL_Deckbuilder
{
    public class Carta
    {
        public string IdCarta { get; set; }
        public string Nombre { get; set; }
        public string Frecuencia { get; set; }
        public string Tipo { get; set; }
        public string Coste { get; set; }
        public string Fuerza { get; set; }
        public string Raza { get; set; }
        public bool esUnica { get; set; }
        public string Habilidad { get; set; }
        public string ImgSrc { get; set; }
    }
}
