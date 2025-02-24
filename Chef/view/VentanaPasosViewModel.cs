using Chef.clases;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Chef.view
{
    public class VentanaPasosViewModel : INotifyPropertyChanged
    {
        private string _nuevoNombre;
        private string _nuevaDescripcion;

        public ObservableCollection<Paso> Pasos { get; set; } = new ObservableCollection<Paso>();
        public event Action<ObservableCollection<Paso>> OnAceptarCerrando;
        public string NuevoNombre
        {
            get => _nuevoNombre;
            set { _nuevoNombre = value; OnPropertyChanged(); }
        }

        public string NuevaDescripcion
        {
            get => _nuevaDescripcion;
            set { _nuevaDescripcion = value; OnPropertyChanged(); }
        }

        public ICommand AgregarPasoCommand { get; }
        public ICommand AceptarCommand { get; }

        public VentanaPasosViewModel()
        {
            AgregarPasoCommand = new RelayCommand(AgregarPaso);
            AceptarCommand = new RelayCommand(Aceptar);
        }

        private void AgregarPaso()
        {
            if (!string.IsNullOrWhiteSpace(NuevoNombre) && !string.IsNullOrWhiteSpace(NuevaDescripcion))
            {
                Pasos.Add(new Paso(0, NuevoNombre, 1, NuevaDescripcion, 0)); // Suponiendo id = 0, numPasos = 1, idReceta = 0
                NuevoNombre = string.Empty;
                NuevaDescripcion = string.Empty;

            }
        }

        private void Aceptar()
        {
            OnAceptarCerrando?.Invoke(Pasos); // Notifica a la vista que debe cerrarse y devuelve los pasos
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }
}
