using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CocinaRecetas.clases;
using CocinaRecetas.view;

namespace Chef
{
    /// <summary>
    /// Lógica de interacción para ListaRecetas.xaml
    /// </summary>
    public partial class ListaRecetas : Window
    {
        public ListaRecetasViewModel ViewModel { get; set; }

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
                // Crear la instancia de CrearReceta pasando la receta seleccionada
                CrearReceta ventanaEditar = new CrearReceta(recetaSeleccionada);
                ventanaEditar.Owner = this; // Opcional: establece la ventana actual como propietaria

                // Mostrar la ventana como modal para bloquear la interacción hasta que se cierre
                ventanaEditar.ShowDialog();
            }
            else
            {
                // Mostrar un mensaje de advertencia si no se ha seleccionado ninguna receta
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
