using System;
using System.ComponentModel;
using System.Windows;
using Chef.models;
using Chef.Data; // Asegúrate de que Repositorio esté en este namespace

namespace Chef.View
{
    public class CrearRecetaViewModel : INotifyPropertyChanged
    {
        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        private string _tiempo;
        public string Tiempo
        {
            get => _tiempo;
            set { _tiempo = value; OnPropertyChanged(nameof(Tiempo)); }
        }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; OnPropertyChanged(nameof(Descripcion)); }
        }

        // Propiedad Dificultad (entero, de 1 a 10)
        private int _dificultad;
        public int Dificultad
        {
            get => _dificultad;
            set { _dificultad = value; OnPropertyChanged(nameof(Dificultad)); }
        }

        // Id del usuario que crea la receta
        public int IdUsuario { get; set; }

        private readonly Repositorio _repositorio;

        // Constructor que recibe el id del usuario (por ejemplo, del login)
        public CrearRecetaViewModel(int idUsuario)
        {
            IdUsuario = idUsuario;
            _repositorio = new Repositorio();
            // Valor por defecto para dificultad
            Dificultad = 1;
        }

        // Método que se invoca para guardar la receta
        public void SaveRecipe()
        {
            // Validación básica de los datos
            if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Tiempo) ||
                string.IsNullOrWhiteSpace(Descripcion))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(Tiempo, out int tiempoInt))
            {
                MessageBox.Show("El tiempo debe ser un número.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear el objeto Receta con los datos ingresados
            Receta nuevaReceta = new Receta
            {
                Nombre = this.Nombre,
                Tiempo = tiempoInt,
                Descripcion = this.Descripcion,
                Dificultad = this.Dificultad,
                IdUsuarioReceta = this.IdUsuario,
            };

            try
            {
                // Insertar la receta en la base de datos
                int newId = _repositorio.InsertarReceta(nuevaReceta);
                nuevaReceta.Id = newId;
                MessageBox.Show("Receta guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la receta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
