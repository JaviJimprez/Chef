using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Chef.models;
using Chef.Data;


namespace CocinaRecetas.view
{
    public class ListaRecetasViewModel : INotifyPropertyChanged
    {
        private readonly Repositorio _repositorio;

        public ObservableCollection<Receta> Recetas { get; set; }

        public ListaRecetasViewModel()
        {
            _repositorio = new Repositorio();
            Recetas = new ObservableCollection<Receta>();
            CargarRecetas();
        }

        public void CargarRecetas()
        {
            List<Receta> recetasDesdeBD = _repositorio.ObtenerRecetasPorUsuario();
            Recetas.Clear();
            foreach (var rec in recetasDesdeBD)
            {
                string imagenBase64 = _repositorio.ObtenerImagenReceta(rec.Id);

                if (!string.IsNullOrEmpty(imagenBase64))
                {
                    rec.Imagen = imagenBase64;
                    Console.WriteLine($"✅ Imagen cargada para receta {rec.Id}");
                }
                else
                {
                    Console.WriteLine($"⚠️ No se encontró imagen para receta {rec.Id}");
                }

                Recetas.Add(rec);
            }

            OnPropertyChanged(nameof(Recetas)); // Notificar cambios en la UI
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
