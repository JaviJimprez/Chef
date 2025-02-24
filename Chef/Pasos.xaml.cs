using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chef.clases;
using Chef.models;

namespace Chef
{
    public partial class Pasos : Window
    {
        private List<Paso> _listaPasos; // 🔹 Lista de pasos
        private int _indicePasoActual = 0; // 🔹 Índice del paso actual
        private Receta _recetaSeleccionada; // 🔹 Receta que se está mostrando

        public Pasos(Receta receta)
        {
            InitializeComponent();
            _recetaSeleccionada = receta;
            this.Title = "Pasos de " + _recetaSeleccionada.Nombre;

            // 🔹 Cargar los pasos de la receta
            _listaPasos = ObtenerPasosDeLaReceta(_recetaSeleccionada.Id);

            if (_listaPasos.Count > 0)
            {
                MostrarPaso(_indicePasoActual);
            }
        }

        private List<Paso> ObtenerPasosDeLaReceta(int recetaId)
        {
            return new List<Paso>
    {
        new Paso("Preparar ingredientes", 1),
        new Paso("Sofreír las verduras", 2),
        new Paso("Añadir el pollo", 3),
        new Paso("Incorporar el tomate", 4),
        new Paso("Condimentar", 5),
        new Paso("Cocinar a fuego lento", 6),
        new Paso("Servir", 7)
    };
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
