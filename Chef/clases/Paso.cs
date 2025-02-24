using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chef.clases
{
    public class Paso : INotifyPropertyChanged
    {
        private int _id;
        private string _nombre;
        private int _numPasos;
        private string _descripcion;
        private int _idReceta;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public int NumPasos
        {
            get => _numPasos;
            set
            {
                if (value < 1)
                    throw new ArgumentException("El número de pasos debe ser al menos 1.");
                _numPasos = value;
                OnPropertyChanged();
            }
        }

        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; OnPropertyChanged(); }
        }

        public int IdReceta
        {
            get => _idReceta;
            set { _idReceta = value; OnPropertyChanged(); }
        }

        public Paso(int id, string nombre, int numPasos, string descripcion, int idReceta)
        {
            Id = id;
            Nombre = nombre;
            NumPasos = numPasos;
            Descripcion = descripcion;
            IdReceta = idReceta;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
