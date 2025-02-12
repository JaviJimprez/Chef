using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocinaRecetas.clases;

namespace CocinaRecetas.view
{
    public class ListaRecetasViewModel : INotifyPropertyChanged
    {
        private string _nombre;
        private string _tiempo;
        private string _imagen;
        private string _descripcion;

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Tiempo
        {
            get => _tiempo;
            set
            {
                _tiempo = value;
                OnPropertyChanged(nameof(Tiempo));
            }
        }

        public string Imagen
        {
            get => _imagen;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Imagen));
            }
        }

        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged(nameof(Descripcion));
            }
        }
        public ObservableCollection<Receta> Recetas { get; }

        public ListaRecetasViewModel()
        {
            Recetas = new ObservableCollection<Receta>
            {
                new Receta
                {
                    Nombre = "Pollo al Limón",
                    Tiempo = 45,
                    Imagen = "/img/pollo_limon.jpg",
                    Descripcion = "Receta orientada en la comida china"
                },
                new Receta
                {
                    Nombre = "Paella",
                    Tiempo = 60,
                    Imagen = "/img/paella.jpg",
                    Descripcion = "Receta orientada a la comida de españa"
                },
                new Receta
                {
                    Nombre = "Tarta de Queso",
                    Tiempo = 120,
                    Imagen = "/img/tarta_queso.jpg",
                    Descripcion = "Receta orientada en el mediterranio"
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
