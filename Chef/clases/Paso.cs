using System;
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
                _numPasos = value < 1 ? 1 : value; // 🔹 Si es 0 o menor, se pone 1 automáticamente
                OnPropertyChanged();
            }
        }

        // 🔹 Nueva propiedad 'Numero' como alias de 'NumPasos' para compatibilidad
        public int Numero
        {
            get => _numPasos;
            set => NumPasos = value;
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
            NumPasos = numPasos > 0 ? numPasos : 1; // 🔹 Si se pasa 0, lo cambia a 1
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
