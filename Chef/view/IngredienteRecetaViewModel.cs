using System.ComponentModel;
using Chef.clases;
using Chef.Data;
using System.Configuration;

namespace Chef.viewmodels
{
    public class IngredienteRecetaViewModel : INotifyPropertyChanged
    {
        private IngredienteReceta _ingredienteReceta;
        private readonly Repositorio.IngredienteRecetaService _service;

        public IngredienteRecetaViewModel()
        {
            _ingredienteReceta = new IngredienteReceta();
            // Obtener la cadena de conexión desde el App.config
            string connectionString = ConfigurationManager.ConnectionStrings["MySQLPersonaje"].ConnectionString;
            _service = new Repositorio.IngredienteRecetaService(connectionString);
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

        // Método público que guarda el ingrediente usando el servicio
        public void GuardarIngrediente()
        {
            int nuevoId = _service.Guardar(_ingredienteReceta);
            if (nuevoId > 0)
            {
                _ingredienteReceta.Id = nuevoId;
                // Aquí podrías mostrar un mensaje o realizar otra acción luego de guardar
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
