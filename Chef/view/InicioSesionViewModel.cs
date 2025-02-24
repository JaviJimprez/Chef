using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using Chef.Data; // Asegúrate de tener este namespace para Repositorio
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

        // Constructor: inicializa el repositorio
        public InicioSesionViewModel()
        {
            _repositorio = new Repositorio();
        }

        public void InicioSesion()
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

                if (_repositorio.ObtenerContrasenia(Usuario) == Contraseña)
                {
                    _repositorio.InicioSesion(Usuario, Contraseña);
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

        public void RegistrarUsuario()
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
                    MessageBox.Show("El usuario ya existe. Intente con otro nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool registrado = _repositorio.RegistrarUsuario(Usuario, Contraseña);

                if (registrado)
                {
                    MessageBox.Show("Usuario registrado con éxito. Ahora puede iniciar sesión.", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el usuario. Intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
