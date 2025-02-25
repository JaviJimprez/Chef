using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chef.clases;
using Chef.models;
using Chef.Data;
using System;
using Chef.viewmodels;

namespace WpfApp2
{
    public partial class Ingredientes : Window
    {
        private IngredienteRecetaViewModel _viewModel;
        private Repositorio _repositorio = new Repositorio(); // 🔹 Acceso a la base de datos
        public List<string> IngredientesSeleccionados { get; private set; } = new List<string>();

        public Ingredientes()
        {
            InitializeComponent();
            _viewModel = DataContext as IngredienteRecetaViewModel;
            if (_viewModel == null)
            {
                _viewModel = new IngredienteRecetaViewModel();
                DataContext = _viewModel;
            }

            // 🔹 Cargar la lista de ingredientes desde la base de datos
            CargarIngredientes();
        }

        private void CargarIngredientes()
        {
            LbIngredientes.ItemsSource = _repositorio.ObtenerTodosLosIngredientes();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LbIngredientes.SelectedItems)
            {
                IngredientesSeleccionados.Add(item.ToString());
            }

            DialogResult = true; // Cierra la ventana y confirma la selección
        }

        private void BtnAñadirIngrediente_Click(object sender, RoutedEventArgs e)
        {
            string nombreIngrediente = TBNombreIngrediente.Text.Trim();

            if (string.IsNullOrEmpty(nombreIngrediente))
            {
                MessageBox.Show("Por favor, ingrese un nombre de ingrediente.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool agregado = _repositorio.InsertarIngrediente2(new Ingrediente(nombreIngrediente));

            if (agregado)
            {
                MessageBox.Show("Ingrediente añadido a la base de datos.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarIngredientes(); // 🔹 Recargar la lista de ingredientes
                TBNombreIngrediente.Clear();
            }
            else
            {
                MessageBox.Show("No se pudo añadir el ingrediente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
