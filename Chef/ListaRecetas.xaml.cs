using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CocinaRecetas.clases;
using CocinaRecetas.view;

namespace Chef
{
    /// <summary>
    /// Lógica de interacción para ListaRecetas.xaml
    /// </summary>
    public partial class ListaRecetas : Window
    {
        public ListaRecetasViewModel ViewModel { get; set; }

        public ListaRecetas()
        {
            InitializeComponent();

            ViewModel = new ListaRecetasViewModel();

            DataContext = ViewModel;

            lsRecetas.ItemsSource = ViewModel.Recetas;
        }

    }
}
