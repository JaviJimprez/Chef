using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Chef.clases;
using Chef.Data;
using Chef.View; // Agregar para acceder a Repositorio

namespace Chef.view
{
    public class VentanaPasosViewModel : INotifyPropertyChanged
    {
        private string _nuevoNombre;
        private string _nuevaDescripcion;
        private string _imagenBase64;
        private BitmapImage _imagenReceta;
        private readonly Repositorio _repositorio;

        public ObservableCollection<Paso> Pasos { get; set; } = new ObservableCollection<Paso>();
        public event Action<ObservableCollection<Paso>> OnAceptarCerrando;

        public string NuevoNombre
        {
            get => _nuevoNombre;
            set { _nuevoNombre = value; OnPropertyChanged(); }
        }

        public string NuevaDescripcion
        {
            get => _nuevaDescripcion;
            set { _nuevaDescripcion = value; OnPropertyChanged(); }
        }

        public BitmapImage ImagenReceta
        {
            get => _imagenReceta;
            private set { _imagenReceta = value; OnPropertyChanged(); }
        }

        public ICommand AgregarPasoCommand { get; }
        public ICommand AceptarCommand { get; }

        public VentanaPasosViewModel(int recetaId)
        {
            _repositorio = new Repositorio();
            CargarImagenReceta(recetaId);

            AgregarPasoCommand = new RelayCommand(AgregarPaso);
            AceptarCommand = new RelayCommand(Aceptar);
        }

        private void CargarImagenReceta(int recetaId)
        {
            _imagenBase64 = _repositorio.ObtenerImagenReceta(recetaId);
            if (!string.IsNullOrEmpty(_imagenBase64))
            {
                ImagenReceta = ConvertirBase64AImagen(_imagenBase64);
                Console.WriteLine($"✅ Imagen cargada en ViewModel para receta {recetaId}");
            }
            else
            {
                Console.WriteLine($"⚠️ No hay imagen para la receta {recetaId}");
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
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al convertir Base64 a imagen: {ex.Message}");
                return null;
            }
        }

        private void AgregarPaso()
        {
            if (!string.IsNullOrWhiteSpace(NuevoNombre) && !string.IsNullOrWhiteSpace(NuevaDescripcion))
            {
                Pasos.Add(new Paso(0, NuevoNombre, 1, NuevaDescripcion, 0));
                NuevoNombre = string.Empty;
                NuevaDescripcion = string.Empty;
            }
        }

        private void Aceptar()
        {
            OnAceptarCerrando?.Invoke(Pasos);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
