using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EleganteLetras
{
    /// <summary>
    /// Interaction logic for Letras.xaml
    /// </summary>
    public partial class Letras
    {

        private static BackgroundWorker backgroundWorker;

        public Letras()
        {
            InitializeComponent();
            ActualizrLetras();
        }

        private void ListarLetras()
        {
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorker.DoWork += delegate (object s, DoWorkEventArgs args)
            {

                this.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    dg_letras.ItemsSource = new Datos.Da_letras().GetLetras();
                }));

            };

            backgroundWorker.RunWorkerAsync();

        }

        public void ActualizrLetras()
        {
            ThreadPool.QueueUserWorkItem(delegate { ListarLetras(); });
        }

        private async void btn_eliminar_Click(object sender, RoutedEventArgs e)
        {
            //obtenemos el usuario desde el datagrid por medio de la fila seleccionada
            object IdLetra = ((Button)sender).CommandParameter;
            //mandamos un mensaje para preguntar si deseamos eliminar

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Sí, Eliminar",
                NegativeButtonText = "No, Cancelar",
                AnimateShow = true,
                AnimateHide = false
            };

            var result = await this.ShowMessageAsync("¿Desea Eliminar la Letra seleccionada?", "¡Una vez eliminada se borrará toda información relacionada con el!", MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result.ToString() == "Affirmative")
            {
                if (new Datos.Da_letras().Eliminar(Convert.ToInt32(IdLetra)))
                {
                    ActualizrLetras();
                    //await this.ShowMessageAsync("Listo", "¡La Letra fue eliminada correctamente!");
                }
            }

        }

        private void btn_nueva_letra_Click(object sender, RoutedEventArgs e)
        {
            NuevaLetra nl = new NuevaLetra();
            nl.Show();

        }
    }
}
