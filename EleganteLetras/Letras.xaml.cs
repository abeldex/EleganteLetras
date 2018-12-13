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
            ThreadPool.QueueUserWorkItem(delegate { ListarLetras(); });
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


    }
}
