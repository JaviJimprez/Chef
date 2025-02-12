using System.Windows;

namespace Chef
{
    public partial class InicioSesion : Window
    {
        private readonly View.InicioSesionViewModel _viewModel;

        public InicioSesion()
        {
            InitializeComponent();
            _viewModel = new View.InicioSesionViewModel();
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Usuario = txtUsuario.Text;
            _viewModel.Contraseña = txtContraseña.Password;
            _viewModel.IniciarSesion();
        }

        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Usuario = txtUsuario.Text;
            _viewModel.Contraseña = txtContraseña.Password;
            _viewModel.Registrarse();
        }
    }
}
