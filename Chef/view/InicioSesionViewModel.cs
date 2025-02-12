using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Chef.View
{
    public class InicioSesionViewModel : INotifyPropertyChanged
    {
        private string _usuario;
        private string _contraseña;
        private Dictionary<string, string> _usuariosRegistrados = new Dictionary<string, string>();

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

            if (_usuariosRegistrados.ContainsKey(Usuario) && _usuariosRegistrados[Usuario] == Contraseña)
            {
                MessageBox.Show($"¡Bienvenido, {Usuario}!", "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);

                ListaRecetas listaRecetas = new ListaRecetas();
                listaRecetas.Show();
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Registrarse()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contraseña))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_usuariosRegistrados.ContainsKey(Usuario))
            {
                MessageBox.Show("Este usuario ya está registrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _usuariosRegistrados[Usuario] = Contraseña;
            MessageBox.Show("Usuario registrado exitosamente.", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);

            ListaRecetas listaRecetas = new ListaRecetas();
            listaRecetas.Show();
            Application.Current.Windows[0]?.Close();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
