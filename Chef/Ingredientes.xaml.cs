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

        public object Nombre { get; internal set; }

        public Ingredientes()
        {
            InitializeComponent();
            _viewModel = DataContext as IngredienteRecetaViewModel;
            if (_viewModel == null)
            {
                _viewModel = new IngredienteRecetaViewModel();
                DataContext = _viewModel;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Opcional: manejo de selección en el ListBox
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GuardarIngrediente();
            MessageBox.Show("Ingrediente guardado correctamente.");
            this.Close();
        }

        private void BtnAñadirIngrediente_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GuardarIngrediente();
            MessageBox.Show("Ingrediente añadido.");
            // Opcional: limpiar campos o actualizar la lista
        }
    }
}
