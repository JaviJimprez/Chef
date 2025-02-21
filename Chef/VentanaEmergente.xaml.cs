using Chef.clases;
using Chef.Data;
using System.Windows.Controls;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Lógica de interacción para VentanaEmergente.xaml
    /// </summary>
    public partial class VentanaEmergente : Window
    {
        private List<Paso> pasos = new List<Paso>();
        private int numPaso = 0;

        public VentanaEmergente()
        {
            InitializeComponent();

        }

        private void cbPasos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPasos.SelectedItem != null)
            {
                Paso pasoSeleccionado = (Paso)cbPasos.SelectedItem; // Cast directo
                titulotxt.Text = pasoSeleccionado.Titulo;
                descripciontxt.Text = pasoSeleccionado.Descripcion;
            }
        }

        private void addPasoRecetaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(titulotxt.Text) && !string.IsNullOrWhiteSpace(descripciontxt.Text))
            {
                numPaso++;
                Paso p = new Paso(numPaso, titulotxt.Text, descripciontxt.Text);
                pasos.Add(p);
                cbPasos.Items.Add($"Paso {numPaso}: {p.Titulo}");
                titulotxt.Text= "";
                descripciontxt.Text = "";
            }
            else
            {
                MessageBox.Show("Debe ingresar un título y una descripción para el paso.");
            }
        }

        public List<Paso> ObtenerPasos()
        {
            return pasos;
        }

        private void AceptarRecetaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pasos.Count > 0)
            {
                MessageBox.Show("Pasos guardados correctamente.");
                this.DialogResult = true; // Indica que la ventana se cerró con éxito
                this.Close();
            }
            else
            {
                MessageBox.Show("No hay pasos para guardar.");
            }
        }

        /* Usar esto cuando se quiera obtener los pasos
         * 
         *VentanaEmergente ventana = new VentanaEmergente();
if (ventana.ShowDialog() == true) // Espera a que se cierre la ventana
{
    List<Paso> listaPasos = ventana.ObtenerPasos();
    // Ahora tienes la lista de pasos y puedes trabajar con ella
}

         * 
         * */

    }
}
