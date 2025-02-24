using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Chef.Data;
using Chef.models;
using Chef.Singleton;
using Chef.View;

namespace Chef.view
{
    public class ValoracionViewModel : INotifyPropertyChanged
    {
        private readonly Repositorio _repositorio;
        private int _estrellas;
        private string _comentario;
        private int _recetaId;

        public ObservableCollection<Valoracion> Valoraciones { get; set; }

        public int Estrellas
        {
            get => _estrellas;
            set
            {
                if (_estrellas != value)
                {
                    _estrellas = value;
                    Console.WriteLine($"Nueva valoración: {_estrellas} estrellas"); // Verifica cambios
                    OnPropertyChanged(nameof(Estrellas));
                }
            }
        }

        public string Comentario
        {
            get => _comentario;
            set
            {
                if (_comentario != value)
                {
                    _comentario = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RecetaId
        {
            get => _recetaId;
            set
            {
                _recetaId = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarValoracionCommand { get; }

        public ValoracionViewModel(int recetaId)
        {
            _repositorio = new Repositorio();
            Valoraciones = new ObservableCollection<Valoracion>(_repositorio.ObtenerValoracionesPorReceta(recetaId));
            RecetaId = recetaId;
            Estrellas = 1;
            AgregarValoracionCommand = new RelayCommand(AgregarValoracion);
        }

        private void AgregarValoracion()
        {
            if (string.IsNullOrWhiteSpace(Comentario))
            {
                MessageBox.Show("Por favor, ingresa un comentario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int usuarioId = UsuarioIniciado.Id;
            int nuevaValoracionId = _repositorio.InsertarValoracion(RecetaId, usuarioId, Estrellas, Comentario);

            if (nuevaValoracionId > 0)
            {
                Valoraciones.Add(new Valoracion(RecetaId, usuarioId, Estrellas, Comentario) { Id = nuevaValoracionId });
                Comentario = string.Empty;
                Estrellas = 1;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
