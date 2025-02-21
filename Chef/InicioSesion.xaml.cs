using System.Windows;
using Chef.View;


namespace Chef
{
    public partial class InicioSesion : Window
    {
        private readonly InicioSesionViewModel ViewModel;

        public InicioSesion()
        {
            InitializeComponent();
            ViewModel = new InicioSesionViewModel();
            DataContext = ViewModel;  // ASIGNA EL DATACONTEXT
  
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {

            ViewModel.InicioSesion();
        }

        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {

            ViewModel.Registrarse();
        }

    }
}
