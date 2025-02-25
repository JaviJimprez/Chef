using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Chef.clases;
using Chef.models;
using Chef.Data;

namespace Chef
{
    public partial class Pasos : Window
    {
        private List<Paso> _listaPasos; // 🔹 Lista de pasos
        private int _indicePasoActual = 0; // 🔹 Índice del paso actual
        private Receta _recetaSeleccionada; // 🔹 Receta que se está mostrando
        private Repositorio _repositorio = new Repositorio(); // 🔹 Acceso a la base de datos

        public Pasos(Receta receta)
        {
            InitializeComponent();
            _recetaSeleccionada = receta;
            this.Title = "Pasos de " + _recetaSeleccionada.Nombre;

            // 🔹 Mostrar la información de la receta en la interfaz
            lbNombre.Content = _recetaSeleccionada.Nombre;
            lbDificultad2.Content = "🔥" + _recetaSeleccionada.Dificultad;
            lbDuracion.Content = _recetaSeleccionada.Tiempo + " min.";

            btFinalizar.Visibility = Visibility.Hidden;

            // 🔹 Cargar los ingredientes y pasos
            CargarPasosEIngredientes();

            // 🔹 Cargar la imagen del plato
            CargarImagenPlato();
        }

        private void CargarPasosEIngredientes()
        {
            // 🔹 Obtener pasos desde la base de datos
            _listaPasos = _repositorio.ObtenerPasosDeLaReceta(_recetaSeleccionada.Id);

            // 🔹 Obtener ingredientes desde la base de datos
            lbIngredientes.ItemsSource = _repositorio.ObtenerIngredientesPorReceta(_recetaSeleccionada.Id);

            // 🔹 Si hay pasos, mostrar el primero
            if (_listaPasos.Count > 0)
            {
                MostrarPaso(_indicePasoActual);
            }
        }

        private void CargarImagenPlato()
        {
            // 🔹 Obtener la imagen en Base64 desde la base de datos
            string imagenBase64 = _repositorio.ObtenerImagenReceta(_recetaSeleccionada.Id);

            if (!string.IsNullOrEmpty(imagenBase64))
            {
                // 🔹 Convertir la imagen Base64 a BitmapImage
                BitmapImage imagenPlato = ConvertirBase64AImagen(imagenBase64);

                if (imagenPlato != null)
                {
                    // 🔹 Asignar la imagen al fondo del Border
                    imPlato.Background = new ImageBrush(imagenPlato)
                    {
                        Stretch = Stretch.UniformToFill,
                        AlignmentY = AlignmentY.Center
                    };
                }
                else
                {
                    Console.WriteLine($"❌ No se pudo convertir la imagen Base64 para la receta {_recetaSeleccionada.Id}");
                }
            }
            else
            {
                Console.WriteLine($"⚠️ No se encontró imagen para la receta {_recetaSeleccionada.Id}");
            }
        }

        private BitmapImage ConvertirBase64AImagen(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"❌ Error al convertir Base64 a imagen: {ex.Message}");
                return null;
            }
        }

        private void MostrarPaso(int indice)
        {
            if (_listaPasos.Count > 0 && indice >= 0 && indice < _listaPasos.Count)
            {
                // 🔹 Mostrar el paso actual en el Label
                lbPaso.Text = _listaPasos[indice].Descripcion;

                // 🔹 Actualizar el contador de pasos
                lbNumeroPaso.Text = $"{indice + 1}/{_listaPasos.Count}";

                // 🔹 Actualizar la barra de progreso
                pbProgreso.Value = (double)(indice + 1) / _listaPasos.Count * 100;

                // 🔹 Mostrar el botón solo al completar el último paso
                btFinalizar.Visibility = (_indicePasoActual == _listaPasos.Count - 1) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void btAlante_Click(object sender, RoutedEventArgs e)
        {
            if (_indicePasoActual < _listaPasos.Count - 1)
            {
                _indicePasoActual++;
                MostrarPaso(_indicePasoActual);
            }
        }

        private void btAtras_Click(object sender, RoutedEventArgs e)
        {
            if (_indicePasoActual > 0)
            {
                _indicePasoActual--;
                MostrarPaso(_indicePasoActual);
            }
        }

        private void btFinalizar_Click(object sender, RoutedEventArgs e)
        {
            int recetaId = _recetaSeleccionada.Id;
            Reseñas reseñas = new Reseñas(recetaId);
            reseñas.Show();
            this.Close();
        }
    }
}
