using System.Collections.ObjectModel;
using System.ComponentModel;
using Chef.models;
using Chef.Data; // Para acceder a Repositorio
using System.Collections.Generic;

namespace CocinaRecetas.view
{
    public class ListaRecetasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Receta> Recetas { get; }

        // Suponemos que guardamos el id del usuario que inició sesión
        private int _idUsuario;

        // Repositorio para acceder a la base de datos
        private readonly Repositorio _repositorio;

        public ListaRecetasViewModel(int idUsuario)
        {
            _idUsuario = idUsuario;
            _repositorio = new Repositorio();
            Recetas = new ObservableCollection<Receta>();

            CargarRecetas();
        }

        private void CargarRecetas()
        {
            List<Receta> recetasDesdeBD = _repositorio.ObtenerRecetasPorUsuario(_idUsuario);
            Recetas.Clear();
            foreach (var rec in recetasDesdeBD)
            {
                Recetas.Add(rec);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}