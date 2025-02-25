using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Chef.view;

namespace Chef
{
    public partial class Reseñas : Window
    {
        private ValoracionViewModel _viewModel;
        private int valoracion = 0; // Variable para almacenar la calificación seleccionada
        private string rutaEstrellaVacia = "img/estrella.png";
        private string rutaEstrellaLlena = "img/rellena.png";
        private int _recetaId;

        public Reseñas(int recetaId)
        {
            InitializeComponent();
            _recetaId = recetaId;
            _viewModel = new ValoracionViewModel(recetaId);
            DataContext = _viewModel;

            ConfigurarEstrellas();
        }

        private void ConfigurarEstrellas()
        {
            for (int i = 1; i <= 5; i++)
            {
                Image estrella = (Image)FindName($"Star{i}");
                if (estrella != null)
                {
                    estrella.Tag = i; // Guarda el número de estrella en el Tag
                    estrella.Source = CargarImagen(rutaEstrellaVacia); // Imagen vacía por defecto
                    estrella.MouseEnter += Star_MouseEnter;
                    estrella.MouseLeave += Star_MouseLeave;
                    estrella.MouseLeftButtonDown += Star_MouseClick;
                }
            }
        }

        private void Star_MouseEnter(object sender, MouseEventArgs e)
        {
            int indiceEstrella = int.Parse(((Image)sender).Tag.ToString());
            HighlightStars(indiceEstrella);
        }

        private void Star_MouseLeave(object sender, MouseEventArgs e)
        {
            HighlightStars(valoracion);
        }

        private void Star_MouseClick(object sender, MouseButtonEventArgs e)
        {
            valoracion = int.Parse(((Image)sender).Tag.ToString());
            _viewModel.Estrellas = valoracion;
            HighlightStars(valoracion);
        }


        private void HighlightStars(int count)
        {
            for (int i = 1; i <= 5; i++)
            {
                Image estrella = (Image)FindName($"Star{i}");
                if (estrella != null)
                {
                    estrella.Source = CargarImagen(i <= count ? rutaEstrellaLlena : rutaEstrellaVacia);
                }
            }
        }

        private BitmapImage CargarImagen(string ruta)
        {
            try
            {
                return new BitmapImage(new Uri(ruta, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar imagen {ruta}: {ex.Message}");
                return null;
            }
        }

        private void GuardarValoracion_Click(object sender, RoutedEventArgs e)
        {
            
            if (_viewModel.Estrellas > 0 && !string.IsNullOrWhiteSpace(_viewModel.Comentario))
            {
                if (_viewModel.AgregarValoracionCommand.CanExecute(null))
                {
                    _viewModel.AgregarValoracionCommand.Execute(null);

                    MessageBox.Show("Reseña guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    ListaRecetas listaRecetas = new ListaRecetas();
                    listaRecetas.Show();

                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una valoración y escribe un comentario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void CerrarVentana(object sender, RoutedEventArgs e)
        {
            _viewModel.Comentario = string.Empty;
            _viewModel.Estrellas = 1;
            valoracion = 1;
            HighlightStars(valoracion);
            Close();
        }
    }
}
