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
using WpfApp2;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CrearReceta : Window
    {
        public CrearReceta()
        {
            InitializeComponent();
        }
        //ingredientes
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana emergente
            var ingredientes = new Ingredientes();

            // Establecer la ventana principal como propietaria de la ventana emergente
            ingredientes.Owner = this;

            // Mostrar la ventana como modal (bloquea la interacción con la ventana principal hasta que se cierre)
            ingredientes.ShowDialog();
        }
        //pasos
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la ventana emergente
            var ventanaEmergente = new VentanaEmergente();

            // Establecer la ventana principal como propietaria de la ventana emergente
            ventanaEmergente.Owner = this;

            // Mostrar la ventana como modal (bloquea la interacción con la ventana principal hasta que se cierre)
            ventanaEmergente.ShowDialog();

        }
    }
}