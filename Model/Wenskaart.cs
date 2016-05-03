using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTest.Model
{
    class Wenskaart
    {
        public string Wens { get; set; }

        public List<Bal> Ballen { get; set; }

        public ImageSource Achtergrond { get; set; }

        public bool Kerst { get; set; }

        public bool Geboorte { get; set; }

        public string Pad { get; set; }

        public FontFamily LetterType { get; set; }

        public int LetterGrootte { get; set; }

        public Wenskaart()
        {
            this.Ballen = new List<Bal>();
        }
    }
}
