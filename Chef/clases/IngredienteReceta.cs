using System.ComponentModel;

namespace Chef.clases
{
    public class IngredienteReceta : INotifyPropertyChanged
    {
        private int _id;
        private int _ingredienteId;
        private int _recetaId;
        private double _cantidad;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public int IngredienteId
        {
            get => _ingredienteId;
            set
            {
                _ingredienteId = value;
                OnPropertyChanged(nameof(IngredienteId));
            }
        }

        public int RecetaId
        {
            get => _recetaId;
            set
            {
                _recetaId = value;
                OnPropertyChanged(nameof(RecetaId));
            }
        }

        public double Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
