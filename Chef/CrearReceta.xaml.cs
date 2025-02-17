using System.Windows;
using Chef.models;
using Chef.View;
using WpfApp2; // Para CrearRecetaViewModel

namespace Chef
{
    /// <summary>
    /// Lógica de interacción para CrearReceta.xaml
    /// </summary>
    public partial class CrearReceta : Window
    {
        private Receta _receta; // Para modo edición (opcional)

        // Constructor por defecto: para crear una nueva receta
        public CrearReceta()
        {
            InitializeComponent();
            // Se establece el DataContext con el ViewModel (aquí se usa 1 como ejemplo para el id del usuario)
            DataContext = new CrearRecetaViewModel(1);
        }

        // Constructor para editar una receta existente
        public CrearReceta(Receta receta) : this()
        {
            _receta = receta;
            if (DataContext is CrearRecetaViewModel vm)
            {
                vm.Nombre = receta.Nombre;
                vm.Tiempo = receta.Tiempo.ToString();
                vm.Descripcion = receta.Descripcion;
                vm.Dificultad = receta.Dificultad;
            }
        }

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            Ingredientes ventanaIngredientes = new Ingredientes();
            ventanaIngredientes.Owner = this;
            ventanaIngredientes.ShowDialog();
        }

        private void AgregarPasos_Click(object sender, RoutedEventArgs e)
        {
            var ventanaEmergente = new VentanaEmergente();
            ventanaEmergente.Owner = this;
            ventanaEmergente.ShowDialog();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BtnGuardar_Click llamado");
            if (DataContext is CrearRecetaViewModel vm)
            {
                vm.SaveRecipe();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnDescartar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BtnDescartar_Click llamado");
            this.DialogResult = false;
            this.Close();
        }
    }
}
