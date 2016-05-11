using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTest.Model
{
    class Bal
    {
        public double X { get; set; }
        public double Y { get; set; }
        public SolidColorBrush Kleur { get; set; }

        public string Naam { get; set; }
        public Bal(SolidColorBrush kleur, double x, double y)
        {
            this.Kleur = kleur;
            this.X = x;
            this.Y = y;
        }
    }
}
