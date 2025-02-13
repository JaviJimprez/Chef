using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using Chef.Data; // Asegúrate de incluir el namespace del repositorio
using MySql.Data.MySqlClient;

namespace Chef.View
{
    public class InicioSesionViewModel : INotifyPropertyChanged
    {
        private string _usuario;
        private string _contraseña;
        private readonly Repositorio _repositorio;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public string Contraseña
        {
            get => _contraseña;
            set
            {
                _contraseña = value;
                OnPropertyChanged(nameof(Contraseña));
            }
        }

        public ICommand IniciarSesionCommand { get; }
        public ICommand RegistrarseCommand { get; }

        public InicioSesionViewModel()
        {
            _repositorio = new Repositorio();
            IniciarSesionCommand = new RelayCommand(IniciarSesion);
            RegistrarseCommand = new RelayCommand(Registrarse);
        }

        public void IniciarSesion()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contraseña))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (!_repositorio.UsuarioExiste(Usuario))
                {
                    MessageBox.Show("Usuario no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string contraseñaDesdeBD = _repositorio.ObtenerContrasenia(Usuario);
                if (contraseñaDesdeBD == Contraseña)
                {
                    MessageBox.Show($"¡Bienvenido, {Usuario}!", "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);

                    ListaRecetas listaRecetas = new ListaRecetas();
                    listaRecetas.Show();
                    Application.Current.Windows[0]?.Close();
                }
                else
                {
                    MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Registrarse()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contraseña))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_repositorio.UsuarioExiste(Usuario))
                {
                    MessageBox.Show("Este usuario ya está registrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _repositorio.RegistrarUsuario(Usuario, Contraseña);

                MessageBox.Show("Usuario registrado exitosamente.", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);

                ListaRecetas listaRecetas = new ListaRecetas();
                listaRecetas.Show();
                Application.Current.Windows[0]?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
