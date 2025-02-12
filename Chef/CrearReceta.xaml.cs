using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CocinaRecetas.clases;
using WpfApp2;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CrearReceta : Window
    {
        // Variable para almacenar la receta a editar
        private Receta _receta;
        public CrearReceta()
        {
            InitializeComponent();
        }

        // Constructor para editar una receta existente
        public CrearReceta(Receta receta) : this() // Llama al constructor por defecto
        {
            _receta = receta;

            // Cargar los datos de la receta en los controles de la ventana
            txtNombre.Text = receta.Nombre;
            txtDuracion.Text = receta.Tiempo.ToString();
            txtDescripcion.Text = receta.Descripcion;

            // Si tienes otros controles (por ejemplo, imagen, dificultad, etc.), cárgalos aquí
        }
        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana Ingredientes
            Ingredientes ventanaIngredientes = new Ingredientes();

            // Opcional: establecer la ventana actual como propietaria
            ventanaIngredientes.Owner = this;

            // Mostrar la ventana de forma modal (bloquea la interacción con la ventana actual hasta que se cierre)
            ventanaIngredientes.ShowDialog();

            // Si prefieres que sea no modal, usa:
            // ventanaIngredientes.Show();
        }

        //pasos
        // Paso: Agregar paso
        private void AgregarPasos_Click(object sender, RoutedEventArgs e)
        {
            // 1. Crear una instancia de la ventana que mostrará los pasos
            var ventanaEmergente = new VentanaEmergente();

            // 2. (Opcional) Establecer la ventana actual como propietaria, para que la ventana emergente se posicione sobre ésta
            ventanaEmergente.Owner = this;

            // 3. Mostrar la ventana de forma modal, de modo que se bloquee la interacción con la ventana actual hasta que se cierre
            ventanaEmergente.ShowDialog();

            // Si prefieres que sea no modal, puedes usar:
            // ventanaEmergente.Show();
        }

    }
}