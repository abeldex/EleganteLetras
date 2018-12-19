using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;


namespace EleganteLetras
{
    /// <summary>
    /// Lógica de interacción para ImportarLetras.xaml
    /// </summary>
    public partial class ImportarLetras 
    {
        public ImportarLetras()
        {
            InitializeComponent();
        }

        async Task<List<FileDetails>> GetSelectedFiles()
        {
            List<FileDetails> docs_selected = new List<FileDetails>();
            foreach (var item in ImageList.Items)
            {
                var container = ImageList.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                FileDetails archivo = container.DataContext as FileDetails;

                var checkbox = ImageList.ItemTemplate.FindName("DocCheckBox", container) as CheckBox;

                if (checkbox.IsChecked.Value == true)
                {
                    //agregamos a una lista los elementos seleccionados
                    docs_selected.Add(archivo);
                }
            }
            return docs_selected;
        }

        private async void btn_seleccionar_Click(object sender, RoutedEventArgs e)
        {
            var controller = await this.ShowProgressAsync("Importando...", "Guardando los documentos seleccionados en la base de datos, no cierre la aplicación mientras se encuentra procesando...",
               true, new MetroDialogSettings() { AnimateShow = true, ColorScheme = MetroDialogColorScheme.Theme });
            controller.SetIndeterminate();
            await Task.Delay(3500);
            List<FileDetails> docs_selected = await GetSelectedFiles();
            await Dispatcher.BeginInvoke((Action)(async () =>
            {
                await controller.CloseAsync();

                if (controller.IsCanceled)
                {
                    await this.ShowMessageAsync("Importacion Cancelada", "La importacion de archivos word fue cancelada.");
                }
                else
                {
                    try
                    {

                        //Insertar los documentos en la base de datos si se han seleccionado
                        if (docs_selected.Count > 0)
                        {
                            new Datos.Da_letras().Intertar_letras(docs_selected);
                            await this.ShowMessageAsync("Importación Exitosa", "Los documentos seleccionados fueron cargados correctamente.");
                        }
                        else
                            await this.ShowMessageAsync("Atención", "Seleccione al menos un documento para iniciar la importación.");


                    }
                    catch (Exception err)
                    {
                        System.Windows.MessageBox.Show(err.Message);
                    }
                }
            }));
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            txt_buscar.Text = dialog.SelectedPath;
        }

        IEnumerable<string> GetFiles(string folder, string filter, bool recursive)
        {
            string[] found = null;
            try
            {
                found = Directory.GetFiles(folder, filter);
            }
            catch { }
            if (found != null)
                foreach (var x in found)
                    yield return x;
            if (recursive)
            {
                found = null;
                try
                {
                    found = Directory.GetDirectories(folder);
                }
                catch { }
                if (found != null)
                    foreach (var x in found)
                        foreach (var y in GetFiles(x, filter, recursive))
                            yield return y;
            }
        }

        private void LoadFolderFiles()
        {
            //progress_circular.Visibility = Visibility.Visible;
            LoadData();
        }

        private void txt_buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Dispatcher.BeginInvoke(new Action(LoadFolderFiles), null);
            LoadData();
        }

        async void LoadData()
        {
            Datos.Da_archivos da = new Datos.Da_archivos();
            //var metroWindow = (System.Windows.Application.Current.MainWindow as MetroWindow);
            var controller = await this.ShowProgressAsync("Procesando...", "Buscando documentos Word en la carpeta seleccionada, no cierre la aplicación mientras se encuentra procesando...",
                true, new MetroDialogSettings() { AnimateShow = true, ColorScheme = MetroDialogColorScheme.Theme });
            controller.SetIndeterminate();
            await Task.Delay(3500);
            //controller.SetTitle("Magnetometer Calibration");
            var docs = da.GetArchivos(txt_buscar.Text);
            
            await Dispatcher.BeginInvoke((Action)(async () =>
            {
                
                await controller.CloseAsync();

                if (controller.IsCanceled)
                {
                    await this.ShowMessageAsync("Busqueda Cancelada", "La busqueda de archivos word fue cancelada.");
                }
                else
                {
                    ImageList.ItemsSource = docs;
                    //await this.ShowMessageAsync("Busqueda finalizada", "Los archivos word encontrados se mostrarán en la lista.");
                }
            }));

           

        }

        
    }
}
