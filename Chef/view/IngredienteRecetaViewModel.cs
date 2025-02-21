using System.Collections.ObjectModel;
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
        private readonly Repositorio _repositorioMain;

        public IngredienteRecetaViewModel()
        {
            _ingredienteReceta = new IngredienteReceta();
            string connectionString = ConfigurationManager.ConnectionStrings["mariadb"].ConnectionString;
            _service = new Repositorio.IngredienteRecetaService(connectionString);
            _repositorioMain = new Repositorio();
            ListaIngredientes = new ObservableCollection<IngredienteReceta>();
        }

        public int IngredienteId
        {
            get => _ingredienteReceta.IngredienteId;
            set { _ingredienteReceta.IngredienteId = value; OnPropertyChanged(nameof(IngredienteId)); }
        }

        public int RecetaId
        {
            get => _ingredienteReceta.RecetaId;
            set { _ingredienteReceta.RecetaId = value; OnPropertyChanged(nameof(RecetaId)); }
        }

        public double Cantidad
        {
            get => _ingredienteReceta.Cantidad;
            set { _ingredienteReceta.Cantidad = value; OnPropertyChanged(nameof(Cantidad)); }
        }

        private string _nombreIngrediente;
        public string NombreIngrediente
        {
            get => _nombreIngrediente;
            set { _nombreIngrediente = value; OnPropertyChanged(nameof(NombreIngrediente)); }
        }

        // Nueva propiedad: Lista de ingredientes agregados
        private ObservableCollection<IngredienteReceta> _listaIngredientes;
        public ObservableCollection<IngredienteReceta> ListaIngredientes
        {
            get => _listaIngredientes;
            set { _listaIngredientes = value; OnPropertyChanged(nameof(ListaIngredientes)); }
        }

        public void GuardarIngrediente()
        {
            // Si el IngredienteId es 0, se busca o inserta el ingrediente por su nombre
            if (_ingredienteReceta.IngredienteId == 0 && !string.IsNullOrWhiteSpace(NombreIngrediente))
            {
                int idIngrediente = _repositorioMain.ObtenerIdIngrediente(NombreIngrediente);
                if (idIngrediente == 0)
                {
                    Ingrediente ing = new Ingrediente(NombreIngrediente);
                    idIngrediente = _repositorioMain.InsertarIngrediente(ing);
                }
                _ingredienteReceta.IngredienteId = idIngrediente;
            }

            int nuevoId = _service.Guardar(_ingredienteReceta);
            if (nuevoId > 0)
            {
                _ingredienteReceta.Id = nuevoId;
                // Agregar el ingrediente guardado a la lista
                ListaIngredientes.Add(_ingredienteReceta);
                // Reiniciar para nuevos datos
                _ingredienteReceta = new IngredienteReceta();
                IngredienteId = 0;
                Cantidad = 0;
                NombreIngrediente = string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
