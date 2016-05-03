using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTest.Model;

namespace WpfTest.ViewModel
{
    class WenskaartVM : ViewModelBase
    {
        private Wenskaart wenskaart;

        public WenskaartVM(Wenskaart wenskaart)
        {

            this.wenskaart = wenskaart;
            LetterGrootte = 12;
            Pad = "Nieuw";
            IsNotNew = false;
            Kerst = false;
            Geboorte = false;
            LetterGrootte = 20;
            
            Wens = "Vul hier uw wens in...";

            List<Kleur> kleurkes = new List<Kleur>();
            foreach (PropertyInfo info in typeof(Colors).GetProperties())
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush deKleur =
                (SolidColorBrush)bc.ConvertFromString(info.Name);
                Kleur kleurke = new Kleur();
                kleurke.Borstel = deKleur;
                kleurke.Naam = info.Name;
                kleurke.Hex = deKleur.ToString();
                kleurke.Rood = deKleur.Color.R;
                kleurke.Groen = deKleur.Color.G;
                kleurke.Blauw = deKleur.Color.B;
                kleurkes.Add(kleurke);

            }
            _Kleuren = kleurkes;
        }

        private List<Kleur> _Kleuren;

        public List<Kleur> Kleuren
        {
            get { return _Kleuren; }
            set
            {
                _Kleuren = value;
                RaisePropertyChanged("Kleuren");
            }
        }

        public List<Bal> Ballen
        {
            get { return wenskaart.Ballen; }
            set
            {
                wenskaart.Ballen = value;
                RaisePropertyChanged("Ballen");
            }
        }

        public string Wens
        {
            get { return wenskaart.Wens; }
            set
            {
                wenskaart.Wens = value;
                RaisePropertyChanged("Wens");

            }
        }

        public ImageSource Achtergrond
        {
            get { return wenskaart.Achtergrond; }
            set
            {
                wenskaart.Achtergrond = value;
                RaisePropertyChanged("Achtergrond");
            }
        }

        public string Pad
        {
            get { return wenskaart.Pad; }
            set
            {
                wenskaart.Pad = value;
                RaisePropertyChanged("Pad");
            }
        }

        public FontFamily LetterType
        {
            get { return wenskaart.LetterType; }
            set
            {
                wenskaart.LetterType = value;
                RaisePropertyChanged("LetterType");
            }
        }

        public int LetterGrootte
        {
            get { return wenskaart.LetterGrootte; }
            set
            {
                wenskaart.LetterGrootte = value;
                RaisePropertyChanged("LetterGrootte");
            }
        }


        public bool Kerst
        {
            get { return wenskaart.Kerst; }
            set
            {
                wenskaart.Kerst = value;
                RaisePropertyChanged("Kerst");
            }
        }

        public bool Geboorte
        {
            get { return wenskaart.Geboorte; }
            set
            {
                wenskaart.Geboorte = value;
                RaisePropertyChanged("Geboorte");
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }

        private bool _isNew;

        public bool IsNotNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                RaisePropertyChanged("IsNotNew");
            }
        }

        public RelayCommand<MouseEventArgs> DragCommand
        {
            get { return new RelayCommand<MouseEventArgs>(OnDrag); }
        }

        private Ellipse sleepEllipse = new Ellipse();



        public void OnDrag(MouseEventArgs e)
        {

            if ((e.MouseDevice.LeftButton == MouseButtonState.Pressed))
            {

                sleepEllipse = (Ellipse)e.OriginalSource;

                DataObject sleepKleur = new DataObject("deKleur", sleepEllipse.Fill);
                DragDrop.DoDragDrop(sleepEllipse, sleepKleur, DragDropEffects.Move);

            }
        }

        private Bal vindBal(string tag)
        {
            foreach (Bal bal in Ballen)
            {
                if (bal.Naam == tag)
                {
                    return bal;
                }
            }
            return null;
        }

        public RelayCommand<DragEventArgs> DropCommand
        {
            get { return new RelayCommand<DragEventArgs>(OnDrop); }
        }

        public void OnDrop(DragEventArgs e)
        {

            double ypos = e.GetPosition((IInputElement)e.OriginalSource).Y;
            double xpos = e.GetPosition((IInputElement)e.OriginalSource).X;
            List<Bal> tempList = new List<Bal>();

            if (sleepEllipse.Tag == null)
            {
                SolidColorBrush kleur = (SolidColorBrush)e.Data.GetData("deKleur");
                Bal bal = new Bal(kleur, xpos - 20, ypos - 20);
                bal.Naam = "bal" + Ballen.Count + 1;
                tempList.Add(bal);

                IsNotNew = true;
            }
            else
            {
                vindBal(sleepEllipse.Tag.ToString()).X = xpos - 20;
                vindBal(sleepEllipse.Tag.ToString()).y = ypos - 20;
                RaisePropertyChanged("Ballen");
            }
            tempList.AddRange(Ballen);

            Ballen = tempList;
        }

        public RelayCommand<DragEventArgs> RemoveCommand
        {
            get { return new RelayCommand<DragEventArgs>(Remove); }
        }

        public void Remove(DragEventArgs e)
        {
            if (sleepEllipse.Tag != null)
            {
                Ballen.RemoveAll(x => x.Naam == sleepEllipse.Tag.ToString());
            }
            List<Bal> tempList = new List<Bal>();
            tempList.AddRange(Ballen);

            Ballen = tempList;
        }

        public RelayCommand GroterCommand
        {
            get { return new RelayCommand(Groter); }
        }

        public RelayCommand KleinerCommand
        {
            get { return new RelayCommand(Kleiner); }
        }

        public void Kleiner()
        {
            if (LetterGrootte > 10)
                LetterGrootte = LetterGrootte - 1;
        }

        public void Groter()
        {
            if (LetterGrootte < 40)
                LetterGrootte = LetterGrootte + 1;
        }

        public RelayCommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw); }
        }

        private void Nieuw()
        {
            Ballen = new List<Bal>();
            Wens = null;
            IsNotNew = false;
        }

        public RelayCommand AfdrukvoorbeeldCommand
        {
            get { return new RelayCommand(Afdrukvoorbeeld); }
        }

        private void Afdrukvoorbeeld()
        {

        }

        public RelayCommand OpslaanCommand
        {
            get { return new RelayCommand(OpslaanBestand); }
        }

        private void OpslaanBestand()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "nieuweKaart";
                dlg.DefaultExt = ".kaart";
                dlg.Filter = "Kaart documents |*.kaart";
                if (dlg.ShowDialog() == true)
                {
                    using (StreamWriter bestand = new StreamWriter(dlg.FileName))
                    {
                        //bestand.WriteLine(dlg.)
                        bestand.WriteLine(dlg.FileName);
                        bestand.WriteLine(Geboorte == true ? "geboortekaart" : "kerstkaart");
                        bestand.WriteLine(Ballen.Count);
                        foreach (Bal bal in Ballen)
                        {

                            bestand.WriteLine(bal.Kleur);
                            bestand.WriteLine(bal.X);
                            bestand.WriteLine(bal.y);

                        }
                        bestand.WriteLine(Wens);
                        bestand.WriteLine(LetterType);
                    }
                }
                Pad = dlg.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("opslaan mislukt : " + ex.Message);
            }
        }

        public RelayCommand OpenenCommand
        {
            get { return new RelayCommand(OpenenBestand); }
        }
        private void OpenenBestand()
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".kaart";
                dlg.Filter = "Kaart documents |*.kaart";
                if (dlg.ShowDialog() == true)
                {
                    using (StreamReader bestand = new StreamReader(dlg.FileName))
                    {
                        Pad = bestand.ReadLine();
                        string kaart = bestand.ReadLine();
                        List<Bal> ballenList = new List<Bal>();
                        int teller = Convert.ToInt16(bestand.ReadLine());
                        for (int i = 0; i < teller; i++)
                        {
                            string kleurString = bestand.ReadLine();
                            int posx = Convert.ToInt16(bestand.ReadLine());
                            int posy = Convert.ToInt16(bestand.ReadLine());
                            SolidColorBrush kleur = (SolidColorBrush)(new BrushConverter().ConvertFrom(kleurString));
                            Bal bal = new Bal(kleur, posx, posy);
                            ballenList.Add(bal);
                        }
                        if (kaart == "kerstkaart")
                            DoeKerst();
                        else
                            DoeGeboorte();
                        Ballen = ballenList;
                        Wens = bestand.ReadLine();
                        LetterType = new FontFamily(bestand.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("openen mislukt : " + ex.Message);
            }
        }

        public RelayCommand KerstCommand
        {
            get { return new RelayCommand(DoeKerst); }
        }

        public void DoeKerst()
        {
            if (!Kerst)
            {
                Achtergrond = new BitmapImage(new Uri("pack://application:,,,/View/images/kerstkaart.jpg", UriKind.Absolute));
                Nieuw();
                Kerst = true;
                Geboorte = false;
            }
            if (!IsEnabled)
                IsEnabled = true;

        }

        public RelayCommand GeboorteCommand
        {
            get { return new RelayCommand(DoeGeboorte); }
        }

        public void DoeGeboorte()
        {
            if (!Geboorte)
            {
                Achtergrond = new BitmapImage(new Uri("pack://application:,,,/View/images/geboortekaart.jpg", UriKind.Absolute));
                Nieuw();
                Geboorte = true;
                Kerst = false;
            }
            if (!IsEnabled)
                IsEnabled = true;
        }

        public RelayCommand<CancelEventArgs> ClosingCommand
        {
            get { return new RelayCommand<CancelEventArgs>(OnWindowClosing); }
        }
        public void OnWindowClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("Afsluiten", "Wilt u het programma sluiten ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) ==
            MessageBoxResult.No)
                e.Cancel = true;
        }

        public RelayCommand PreviewCommand
        {
            get { return new RelayCommand(Preview); }
        }

        private void Preview()
        {
            Afdrukvoorbeeld preview = new Afdrukvoorbeeld();
            //preview.Owner = this;
            preview.AfdrukDocument = StelAfdrukSamen();
            preview.ShowDialog();
        }

        private FixedDocument StelAfdrukSamen()
        {
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(500, 500);

            PageContent inhoud = new PageContent();
            document.Pages.Add(inhoud);

            FixedPage page = new FixedPage();
            inhoud.Child = page;

            page.Width = 600;
            page.Height = 600;

            Canvas canvas = new Canvas();
            canvas.Width = 500;
            canvas.Height = 400;
            ImageBrush ib = new ImageBrush();
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri("pack://application:,,,/View/images/kerstkaart.jpg");
            b.EndInit();
            ib.ImageSource = b;
            canvas.Background = ib;
            page.Children.Add(canvas);

            foreach (Bal bal in Ballen)
            {
                Ellipse ell = new Ellipse();
                ell.Fill = bal.Kleur;
                ell.Height = 40;
                ell.Width = 40;

                Canvas.SetLeft(ell, bal.X);
                Canvas.SetTop(ell, bal.y);

                canvas.Children.Add(ell);
            }

            TextBlock deRegel = new TextBlock();
            deRegel.Text = Wens;
            deRegel.FontFamily = LetterType;
            deRegel.FontSize = LetterGrootte;
            deRegel.Margin = new Thickness(0, 400, 0, 0);

            page.Children.Add(deRegel);
            return document;
        }
    }
}
