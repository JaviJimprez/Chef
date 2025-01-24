using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chef
{
    /// <summary>
    /// Lógica de interacción para Reseñas.xaml
    /// </summary>
    public partial class Reseñas : Window
    {
        private int valoracion = 0;

        public Reseñas()
        {
            InitializeComponent();
        }

        //Metodo se ejecuta cuando el raton entra sobre la imagen de la estrella
        private void Star_MouseEnter(object sender, MouseEventArgs e)
        {
            int indiceEstrella = int.Parse(((Image)sender).Tag.ToString());
            HighlightStars(indiceEstrella);
        }

        //Metodo que se ejecuta cuando el raton se aleja de la imagen de la estrella
        private void Star_MouseLeave(object sender, MouseEventArgs e)
        {
            HighlightStars(valoracion);
        }

        //Metodo que se ejecuta cuando clickamos con el raton sobre la imagen de la estrella
        private void Star_MouseClick(object sender, MouseButtonEventArgs e)
        {
            valoracion = int.Parse(((Image)sender).Tag.ToString());
            HighlightStars(valoracion);
        }

        //Metodo que se ejecuta cuando ya hemos clickado y cambia el valor de la estrella dependiendo cual haya sido pulsada
        private void HighlightStars(int count)
        {
            for (int i = 1; i <= 5; i++)
            {
                //Busca la posicion de la imagen por nombre
                Image estrella = (Image)FindName($"Star{i}");
                if (estrella != null)
                {
                    if (i <= count)
                    {
                        //Aqui se cambia a rellena
                        estrella.Source = new BitmapImage(new Uri("pack://application:,,,/img/rellena.png"));
                    }
                    else
                    {
                        //Aqui se cambia a vacia
                        estrella.Source = new BitmapImage(new Uri("pack://application:,,,/img/estrella.png"));
                    }
                }
            }
        }

    }
}
