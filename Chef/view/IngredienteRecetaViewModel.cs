using System.Windows.Input;
using System.ComponentModel;
using Chef.clases;
using Chef.View;

namespace Chef.viewmodels
{
    public class IngredienteRecetaViewModel : INotifyPropertyChanged
    {
        private IngredienteReceta _ingredienteReceta;
        private readonly Repositorio.IngredienteRecetaService _service;

        public IngredienteRecetaViewModel()
        {
            _ingredienteReceta = new IngredienteReceta();
            _service = new Repositorio.IngredienteRecetaService();
            GuardarCommand = new RelayCommand(GuardarIngrediente);
        }

        public int IngredienteId
        {
            get => _ingredienteReceta.IngredienteId;
            set
            {
                _ingredienteReceta.IngredienteId = value;
                OnPropertyChanged(nameof(IngredienteId));
            }
        }

        public int RecetaId
        {
            get => _ingredienteReceta.RecetaId;
            set
            {
                _ingredienteReceta.RecetaId = value;
                OnPropertyChanged(nameof(RecetaId));
            }
        }

        public double Cantidad
        {
            get => _ingredienteReceta.Cantidad;
            set
            {
                _ingredienteReceta.Cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }

        public ICommand GuardarCommand { get; }

        private void GuardarIngrediente()
        {
            int nuevoId = _service.Guardar(_ingredienteReceta);
            if (nuevoId > 0)
            {
                _ingredienteReceta.Id = nuevoId;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
