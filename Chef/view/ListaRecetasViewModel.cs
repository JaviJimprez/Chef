using System.Collections.ObjectModel;
using System.ComponentModel;
using Chef.models;
using Chef.Data; // Para acceder a Repositorio
using System.Collections.Generic;

namespace CocinaRecetas.view
{
    public class ListaRecetasViewModel : INotifyPropertyChanged
    {
        private int _idUsuario;
        private readonly Repositorio _repositorio;

        public ObservableCollection<Receta> Recetas { get; set; }

        public ListaRecetasViewModel(int idUsuario)
        {
            _idUsuario = idUsuario;
            _repositorio = new Repositorio();
            Recetas = new ObservableCollection<Receta>();
            CargarRecetas();
        }

        public void CargarRecetas()
        {
            List<Receta> recetasDesdeBD = _repositorio.ObtenerRecetasPorUsuario(_idUsuario);
            Recetas.Clear();
            foreach (var rec in recetasDesdeBD)
            {
                Recetas.Add(rec);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}