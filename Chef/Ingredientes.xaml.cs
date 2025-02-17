using System.Windows;
using System.Windows.Controls;
using Chef.viewmodels; // Asegúrate de que IngredienteRecetaViewModel esté en este namespace

namespace WpfApp2
{
    /// <summary>
    /// Lógica de interacción para Ingredientes.xaml
    /// </summary>
    public partial class Ingredientes : Window
    {
        private IngredienteRecetaViewModel _viewModel;

        public Ingredientes()
        {
            InitializeComponent();
            // El DataContext ya se asignó en el XAML, pero lo obtenemos aquí para facilitar su uso.
            _viewModel = DataContext as IngredienteRecetaViewModel;
            if (_viewModel == null)
            {
                _viewModel = new IngredienteRecetaViewModel();
                DataContext = _viewModel;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aquí puedes manejar la selección del ListBox, si es necesario.
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            // Invoca el método del ViewModel para guardar el ingrediente
            _viewModel.GuardarIngrediente();
            MessageBox.Show("Ingrediente guardado correctamente.");
            this.Close(); // Cierra la ventana después de aceptar, según tu lógica.
        }

        private void BtnAñadirIngrediente_Click(object sender, RoutedEventArgs e)
        {
            // Si deseas agregar el ingrediente a una lista (además de guardarlo),
            // puedes invocar el método del ViewModel y luego limpiar campos o actualizar la UI.
            _viewModel.GuardarIngrediente();
            MessageBox.Show("Ingrediente añadido.");
            // Opcional: limpiar campos o actualizar el ListBox.
        }
    }
}
