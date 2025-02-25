using System.Windows;
using System.Windows.Controls;
using Chef.clases;
using Chef.Data;
using Chef.models;
using CocinaRecetas.view;



namespace Chef
{
    public partial class ListaRecetas : Window
    {
        public ListaRecetasViewModel ViewModel { get; set; }

        public ListaRecetas() : this(1) { }

        public ListaRecetas(int idUsuario)
        {
            InitializeComponent();
            ViewModel = new ListaRecetasViewModel();
            DataContext = ViewModel;
            lsRecetas.ItemsSource = ViewModel.Recetas;
            lsRecetas.SelectionChanged += lsRecetas_SelectionChanged;
        }

        private void lsRecetas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsRecetas.SelectedItem is Receta recetaSeleccionada)
            {
                // Abre la ventana Pasos.xaml y le pasa la receta seleccionada
                Pasos ventanaPasos = new Pasos(recetaSeleccionada);
                ventanaPasos.Owner = this;
                ventanaPasos.ShowDialog();
            }
        }

        private void BtnNuevaReceta_Click(object sender, RoutedEventArgs e)
        {
            CrearReceta ventanaCrearReceta = new CrearReceta();
            ventanaCrearReceta.Owner = this;
            if (ventanaCrearReceta.ShowDialog() == true)
            {
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

        private void BtBorrarReceta_Click(object sender, RoutedEventArgs e)
        {
            if (lsRecetas.SelectedItem is Receta recetaSeleccionada)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar la receta '{recetaSeleccionada.Nombre}'?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Repositorio repositorio = new Repositorio();
                    bool eliminada = repositorio.BorrarReceta(recetaSeleccionada.Id);

                    if (eliminada)
                    {
                        MessageBox.Show("Receta eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        ViewModel.CargarRecetas(); // 🔹 Recargar la lista de recetas
                    }
                    else
                    {
                        MessageBox.Show("Hubo un problema al eliminar la receta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una receta para eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
