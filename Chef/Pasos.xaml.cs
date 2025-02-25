using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chef.clases;
using Chef.models;
using Chef.Data;

namespace Chef
{
    public partial class Pasos : Window
    {
        private List<Paso> _listaPasos; // 🔹 Lista de pasos
        private int _indicePasoActual = 0; // 🔹 Índice del paso actual
        private Receta _recetaSeleccionada; // 🔹 Receta que se está mostrando
        private Repositorio _repositorio = new Repositorio(); // 🔹 Acceso a la base de datos

        public Pasos(Receta receta)
        {
            InitializeComponent();
            _recetaSeleccionada = receta;
            this.Title = "Pasos de " + _recetaSeleccionada.Nombre;

            // 🔹 Mostrar la información de la receta en la interfaz
            lbNombre.Content = _recetaSeleccionada.Nombre;
            lbDificultad2.Content = "🔥" + _recetaSeleccionada.Dificultad;
            lbDuracion.Content = _recetaSeleccionada.Tiempo + " min.";

            // 🔹 Cargar los ingredientes y pasos
            CargarPasosEIngredientes();
        }

        private void CargarPasosEIngredientes()
        {
            // 🔹 Obtener pasos desde la base de datos
            _listaPasos = _repositorio.ObtenerPasosDeLaReceta(_recetaSeleccionada.Id);

            // 🔹 Obtener ingredientes desde la base de datos
            lbIngredientes.ItemsSource = _repositorio.ObtenerIngredientesPorReceta(_recetaSeleccionada.Id);

            // 🔹 Si hay pasos, mostrar el primero
            if (_listaPasos.Count > 0)
            {
                MostrarPaso(_indicePasoActual);
            }
        }

        private void MostrarPaso(int indice)
        {
            if (_listaPasos.Count > 0 && indice >= 0 && indice < _listaPasos.Count)
            {
                // 🔹 Mostrar el paso actual en el Label
                lbPaso.Text = _listaPasos[indice].Descripcion;

                // 🔹 Actualizar el contador de pasos
                lbNumeroPaso.Text = $"{indice + 1}/{_listaPasos.Count}";

                // 🔹 Actualizar la barra de progreso
                pbProgreso.Value = (double)(indice + 1) / _listaPasos.Count * 100;
            }
        }

        private void btAlante_Click(object sender, RoutedEventArgs e)
        {
            if (_indicePasoActual < _listaPasos.Count - 1)
            {
                _indicePasoActual++;
                MostrarPaso(_indicePasoActual);
            }
        }

        private void btAtras_Click(object sender, RoutedEventArgs e)
        {
            if (_indicePasoActual > 0)
            {
                _indicePasoActual--;
                MostrarPaso(_indicePasoActual);
            }
        }
    }
}
