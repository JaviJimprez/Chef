using System.Windows;
using System.Windows.Controls;
using Chef.models;
using CocinaRecetas.view;

namespace Chef
{
    /// <summary>
    /// Lógica de interacción para ListaRecetas.xaml
    /// </summary>
    public partial class ListaRecetas : Window
    {
        public ListaRecetasViewModel ViewModel { get; set; }

        // Constructor que recibe el id del usuario
        public ListaRecetas()
        {
            InitializeComponent();
            ViewModel = new ListaRecetasViewModel();
            DataContext = ViewModel;
            lsRecetas.ItemsSource = ViewModel.Recetas;
            lsRecetas.SelectionChanged += lsRecetas_SelectionChanged;
        }

        private void BtnNuevaReceta_Click(object sender, RoutedEventArgs e)
        {
            CrearReceta ventanaCrearReceta = new CrearReceta();
            ventanaCrearReceta.Owner = this;
            // Mostrar como diálogo para poder usar DialogResult
            if (ventanaCrearReceta.ShowDialog() == true)
            {
                // Refrescar la lista de recetas
                ViewModel.CargarRecetas();
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (lsRecetas.SelectedItem is Receta recetaSeleccionada)
            {
                CrearReceta ventanaEditar = new CrearReceta(recetaSeleccionada);
                ventanaEditar.Owner = this;
                ventanaEditar.ShowDialog();
                // Si editas la receta, podrías refrescar la lista también
                ViewModel.CargarRecetas();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una receta para editar.",
                                "Atención",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void lsRecetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btEditarReceta.Visibility = lsRecetas.SelectedItem != null ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
