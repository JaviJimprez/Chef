using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Chef.models;
using Chef.Data;

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

        private int _dificultad;
        public int Dificultad
        {
            get => _dificultad;
            set { _dificultad = value; OnPropertyChanged(nameof(Dificultad)); }
        }

        private string _imagenBase64;
        public string ImagenBase64
        {
            get => _imagenBase64;
            set { _imagenBase64 = value; OnPropertyChanged(nameof(ImagenBase64)); }
        }

        private string _rutaImagen;
        public string RutaImagen
        {
            get => _rutaImagen;
            set { _rutaImagen = value; OnPropertyChanged(nameof(RutaImagen)); }
        }

        private readonly Repositorio _repositorio;

        public CrearRecetaViewModel()
        {
            _repositorio = new Repositorio();
            Dificultad = 1;
        }

        public void GuardarImagen()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleccionar una imagen",
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                RutaImagen = openFileDialog.FileName; // Mostrar en el TextBox

                try
                {
                    byte[] imageBytes = File.ReadAllBytes(RutaImagen);
                    ImagenBase64 = Convert.ToBase64String(imageBytes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al convertir la imagen: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void SaveRecipe()
        {
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

            Receta nuevaReceta = new Receta
            {
                Nombre = this.Nombre,
                Tiempo = tiempoInt,
                Descripcion = this.Descripcion,
                Dificultad = this.Dificultad,
            };

            int newId = _repositorio.InsertarReceta(nuevaReceta);
            nuevaReceta.Id = newId;

            if (!string.IsNullOrEmpty(ImagenBase64))
            {
                _repositorio.GuardarImagenReceta(newId, ImagenBase64);
            }

            MessageBox.Show("Receta guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
