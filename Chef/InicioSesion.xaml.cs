using System.Windows;
using Chef.View;

namespace Chef
{
    public partial class InicioSesion : Window
    {
        private readonly InicioSesionViewModel _viewModel;

        public InicioSesion()
        {
            InitializeComponent();
            _viewModel = new InicioSesionViewModel();
            DataContext = _viewModel;  // ASIGNA EL DATACONTEXT
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            // Ahora DataContext es _viewModel, puedes usarlo directamente:
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
