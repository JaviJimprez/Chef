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

        // Constructor sin parámetros: usa un id de usuario por defecto (por ejemplo, 1)
        public ListaRecetas() : this(1)
        {
        }

        // Constructor que recibe el id del usuario logueado
        public ListaRecetas(int idUsuario)
        {
            InitializeComponent();
            ViewModel = new ListaRecetasViewModel(idUsuario);
            DataContext = ViewModel;
            lsRecetas.ItemsSource = ViewModel.Recetas;
            lsRecetas.SelectionChanged += lsRecetas_SelectionChanged;
        }

        private void BtnNuevaReceta_Click(object sender, RoutedEventArgs e)
        {
            // Se crea la ventana para crear una nueva receta
            CrearReceta ventanaCrearReceta = new CrearReceta();
            ventanaCrearReceta.Owner = this;
            ventanaCrearReceta.Show();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se haya seleccionado una receta
            if (lsRecetas.SelectedItem is Receta recetaSeleccionada)
            {
                // Crear la instancia de CrearReceta pasando la receta seleccionada para editarla
                CrearReceta ventanaEditar = new CrearReceta(recetaSeleccionada);
                ventanaEditar.Owner = this; // Establece la ventana actual como propietaria

                // Mostrar la ventana como modal
                ventanaEditar.ShowDialog();
            }
            else
            {
                // Mostrar mensaje si no se ha seleccionado ninguna receta
                MessageBox.Show("Por favor, seleccione una receta para editar.",
                                "Atención",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void lsRecetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si se ha seleccionado una receta, se hace visible el botón de editar.
            if (lsRecetas.SelectedItem != null)
                btEditarReceta.Visibility = Visibility.Visible;
            else
                btEditarReceta.Visibility = Visibility.Hidden;
        }
    }
}
