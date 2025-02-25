using System.Windows;
using System.Windows.Media.Imaging;
using Chef.models;
using Chef.View;
using Microsoft.Win32;
using WpfApp2; // Para CrearRecetaViewModel
using System.IO;
using Chef.clases;


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

        //private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        //{
        //  Ingredientes ventanaIngredientes = new Ingredientes();
        //ventanaIngredientes.Owner = this;

        //      if (ventanaIngredientes.ShowDialog() == true) // Si el usuario acepta
        //    {
        //      foreach (var ingrediente in ventanaIngredientes.IngredientesSeleccionados)
        //    {
        //      LBIngredientes.Items.Add(ingrediente); // Agrega los ingredientes seleccionados a la ListBox
        //}
        // }
        // }


        private void AgregarPasos_Click(object sender, RoutedEventArgs e)
        {
            if (_receta == null)
            {
                MessageBox.Show("Error: No se ha seleccionado una receta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var ventanaEmergente = new VentanaEmergente(_receta.Id); // 🔹 Pasar el ID de la receta
            ventanaEmergente.Owner = this;

            bool? resultado = ventanaEmergente.ShowDialog(); // Llamar a ShowDialog solo UNA vez

            if (resultado == true) // Si el usuario aceptó los pasos
            {
                foreach (var paso in ventanaEmergente.PasosSeleccionados)
                {
                    LBPasos.Items.Add($"{paso.Nombre}: {paso.Descripcion}"); // Agrega los pasos a la lista
                }
            }
        }



        //private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        // {
        //     if (DataContext is CrearRecetaViewModel vm)
        //    {
        //        vm.SaveRecipe();
        //         this.DialogResult = true; // Solo se puede establecer cuando la ventana se muestra como diálogo (ShowDialog)
        //         this.Close();
        //    }
        // }

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


        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CrearRecetaViewModel vm)
            {
                List<string> ingredientes = new List<string>();
                foreach (var item in LBIngredientes.Items)
                {
                    ingredientes.Add(item.ToString());
                }

                List<Paso> pasos = new List<Paso>();
                foreach (var item in LBPasos.Items)
                {
                    string[] datos = item.ToString().Split(':');
                    if (datos.Length >= 2)
                    {
                        pasos.Add(new Paso(0, datos[0].Trim(), 0, datos[1].Trim(), 0));
                    }
                }

                vm.GuardarRecetaCompleta(ingredientes, pasos);
                this.DialogResult = true;
                this.Close();
            }
        }



    }
}
