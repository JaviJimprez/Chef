using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chef.clases;
using Chef.models;
using Chef.Data;
using System;
using Chef.viewmodels;
using Chef.view;

namespace WpfApp2
{
    public partial class VentanaEmergente : Window
    {
        private VentanaPasosViewModel _viewModel;
        private Repositorio _repositorio = new Repositorio(); // 🔹 Acceso a la base de datos
        public List<Paso> PasosSeleccionados { get; private set; } = new List<Paso>();

        public VentanaEmergente()
        {
            InitializeComponent();
            _viewModel = DataContext as VentanaPasosViewModel;
            if (_viewModel == null)
            {
                _viewModel = new VentanaPasosViewModel();
                DataContext = _viewModel;
            }

            // 🔹 Cargar la lista de pasos desde la base de datos (si es necesario)
            lbPasos.ItemsSource = PasosSeleccionados;
        }

        private void addPasoRecetaBtn_Click(object sender, RoutedEventArgs e)
        {
            string titulo = tituloTXT.Text.Trim();
            string descripcion = descripcionTXT.Text.Trim();

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descripcion))
            {
                MessageBox.Show("Por favor, ingrese un título y una descripción.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Paso nuevoPaso = new Paso(0, titulo, 1, descripcion, 0);
            PasosSeleccionados.Add(nuevoPaso);

            // 🔹 Refrescar la ListBox correctamente
            lbPasos.ItemsSource = null;
            lbPasos.ItemsSource = PasosSeleccionados;
            lbPasos.DisplayMemberPath = "Nombre";

            tituloTXT.Clear();
            descripcionTXT.Clear();
        }

        private void AceptarRecetaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Owner is Chef.CrearReceta ventanaCrearReceta)
            {
                foreach (var paso in PasosSeleccionados)
                {
                    ventanaCrearReceta.LBPasos.Items.Add(paso);
                }
            }

            DialogResult = true; // Confirmar los pasos seleccionados
            this.Close();
        }
    }
}
