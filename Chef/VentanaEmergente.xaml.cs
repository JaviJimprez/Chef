using Chef.clases;
using Chef.view;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp2
{
    public partial class VentanaEmergente : Window
    {
        public VentanaPasosViewModel ViewModel { get; }
        public VentanaEmergente()
        {
            InitializeComponent();
            ViewModel = new VentanaPasosViewModel();
            ViewModel.OnAceptarCerrando += ViewModel_OnAceptarCerrando;
            DataContext = ViewModel;
        }

        private void ViewModel_OnAceptarCerrando(ObservableCollection<Paso> pasos)
        {
            this.DialogResult = true; // Cierra la ventana si se abrió con ShowDialog()
            this.Close();
        }
    }
}
