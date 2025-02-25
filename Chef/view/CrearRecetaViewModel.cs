using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Chef.models;
using Chef.Data;
using Chef.clases;
using System.Collections.Generic;

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

        // 🔹 Ahora manejamos ingredientes y pasos dentro del ViewModel
        public ObservableCollection<string> IngredientesSeleccionados { get; set; }
        public ObservableCollection<Paso> PasosSeleccionados { get; set; }

        public CrearRecetaViewModel()
        {
            _repositorio = new Repositorio();
            Dificultad = 1;
            IngredientesSeleccionados = new ObservableCollection<string>();
            PasosSeleccionados = new ObservableCollection<Paso>();
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

            // 1️⃣ Crear la nueva receta
            Receta nuevaReceta = new Receta
            {
                Nombre = this.Nombre,
                Tiempo = tiempoInt,
                Descripcion = this.Descripcion,
                Dificultad = this.Dificultad,
            };

            // 2️⃣ Insertar la receta en la base de datos
            int newId = _repositorio.InsertarReceta(nuevaReceta);
            nuevaReceta.Id = newId;

            // 3️⃣ Guardar la imagen si existe
            if (!string.IsNullOrEmpty(ImagenBase64))
            {
                _repositorio.GuardarImagenReceta(newId, ImagenBase64);
            }

            // 4️⃣ 🔹 GUARDAR INGREDIENTES ASOCIADOS (Corrección aquí)
            foreach (var ingrediente in IngredientesSeleccionados)
            {
                _repositorio.GuardarIngredienteReceta(newId, ingrediente);
            }

            // 5️⃣ 🔹 GUARDAR PASOS ASOCIADOS
            int numPaso = 1;
            foreach (var paso in PasosSeleccionados)
            {
                paso.Numero = numPaso++;
                _repositorio.GuardarPasoReceta(newId, paso);
            }

            // 6️⃣ Mensaje de éxito
            MessageBox.Show("Receta guardada exitosamente con ingredientes y pasos.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void GuardarRecetaCompleta(List<string> ingredientes, List<Paso> pasos)
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

            // 1️⃣ Guardar la receta principal
            Receta nuevaReceta = new Receta
            {
                Nombre = this.Nombre,
                Tiempo = tiempoInt,
                Descripcion = this.Descripcion,
                Dificultad = this.Dificultad,
            };

            int recetaId = _repositorio.InsertarReceta(nuevaReceta);
            if (recetaId <= 0)
            {
                MessageBox.Show("Error al guardar la receta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2️⃣ Guardar ingredientes seleccionados
            foreach (var ingrediente in ingredientes)
            {
                _repositorio.GuardarIngredienteReceta(recetaId, ingrediente);
            }

            // 3️⃣ Guardar pasos y asignarlos al ViewModel
            int numPaso = 1;
            PasosSeleccionados.Clear(); // 🔹 Asegurar que la lista está limpia antes de añadir nuevos pasos
            foreach (var paso in pasos)
            {
                paso.Numero = numPaso++;
                _repositorio.GuardarPasoReceta(recetaId, paso);
                PasosSeleccionados.Add(paso); // 🔹 Asignarlos a la propiedad vinculada al ListBox
            }

            Console.WriteLine($"Pasos guardados: {PasosSeleccionados.Count}");

            MessageBox.Show("Receta guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // 🔹 Método para agregar un paso al ViewModel
        public void AgregarPaso(Paso paso)
        {
            if (paso != null)
            {
                PasosSeleccionados.Add(paso);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
