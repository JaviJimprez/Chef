using System.Windows;
using Chef.View;

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
            // Suponiendo que DataContext es una instancia de InicioSesionViewModel
            if (DataContext is InicioSesionViewModel vm)
            {
                vm.Usuario = txtUsuario.Text;
                vm.Contraseña = txtContraseña.Password;
                vm.IniciarSesion();
            }
        }

        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InicioSesionViewModel vm)
            {
                vm.Usuario = txtUsuario.Text;
                vm.Contraseña = txtContraseña.Password;
                vm.Registrarse();
            }
        }

    }
}
