using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Chef.models
{
    public class Receta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Tiempo { get; set; }
        public string Descripcion { get; set; }
        public int Dificultad { get; set; }
        public int IdUsuarioReceta { get; set; }
        public string Imagen { get; set; }

        private BitmapImage _imagenReceta;

        public BitmapImage ImagenReceta
        {
            get
            {
                if (_imagenReceta == null && !string.IsNullOrEmpty(Imagen))
                {
                    Console.WriteLine($"Convirtiendo imagen Base64 a BitmapImage para receta {Id}");
                    _imagenReceta = ConvertirBase64AImagen(Imagen);
                }
                return _imagenReceta;
            }
        }

        private BitmapImage ConvertirBase64AImagen(string base64String)
        {
            try
            {
                // Verificar si la cadena Base64 es válida antes de convertirla
                if (string.IsNullOrEmpty(base64String))
                {
                    Console.WriteLine($"⚠️ Imagen Base64 vacía para receta");
                    return null;
                }

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
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al convertir Base64 a imagen: {ex.Message}");
                return null;
            }
        }
    }
}
