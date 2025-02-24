﻿using System.Windows;
using System.Windows.Media.Imaging;
using Chef.models;
using Chef.View;
using Microsoft.Win32;
using WpfApp2; // Para CrearRecetaViewModel
using System.IO;


namespace Chef
{
    /// <summary>
    /// Lógica de interacción para CrearReceta.xaml
    /// </summary>
    public partial class CrearReceta : Window
    {
        private Receta _receta; // Modo edición (opcional)

        public CrearRecetaViewModel ViewModel { get; set; }
        // Constructor por defecto: para crear una nueva receta
        public CrearReceta()
        {
            InitializeComponent();
            // Asigna el DataContext con el ViewModel; usa 1 como ejemplo para el id del usuario
            ViewModel = new CrearRecetaViewModel();
            DataContext = ViewModel;
        }

        // Constructor para editar una receta existente
        public CrearReceta(Receta receta) : this()
        {
            _receta = receta;
            if (DataContext is CrearRecetaViewModel vm)
            {
                vm.Nombre = receta.Nombre;
                vm.Tiempo = receta.Tiempo.ToString();
                vm.Descripcion = receta.Descripcion;
                vm.Dificultad = receta.Dificultad;
            }
        }

        //private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        //{
           // Ingredientes ventanaIngredientes = new Ingredientes();
           // ventanaIngredientes.Owner = this;
           // ventanaIngredientes.ShowDialog();
        //}

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            Ingredientes ventanaIngredientes = new Ingredientes();
            ventanaIngredientes.Owner = this;

            if (ventanaIngredientes.ShowDialog() == true) // Si el usuario acepta
            {
                foreach (var ingrediente in ventanaIngredientes.IngredientesSeleccionados)
                {
                    LBIngredientes.Items.Add(ingrediente); // Agrega los ingredientes seleccionados a la ListBox
                }
            }
        }


        private void AgregarPasos_Click(object sender, RoutedEventArgs e)
        {
            var ventanaEmergente = new VentanaEmergente();
            ventanaEmergente.Owner = this;
            ventanaEmergente.ShowDialog();

            if (ventanaEmergente.ShowDialog() == true)
            {
                var pasosSeleccionados = ventanaEmergente.ViewModel.Pasos; // Obtienes la lista de pasos
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CrearRecetaViewModel vm)
            {
                vm.SaveRecipe();
                this.DialogResult = true; // Solo se puede establecer cuando la ventana se muestra como diálogo (ShowDialog)
                this.Close();
            }
        }

        private void BtnDescartar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BotonImagen_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CrearRecetaViewModel vm)
            {
                vm.GuardarImagen();
            }
        }
    }
}
