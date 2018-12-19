using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace EleganteLetras
{
    /// <summary>
    /// Interaction logic for TabLetraContent.xaml
    /// </summary>
    public partial class TabLetraContent : UserControl
    {
        public TabLetraContent()
        {
            InitializeComponent();
        }

        int _id;
        string _nombre;

        public TabLetraContent(string letra, int id, string nombre)
        {
            InitializeComponent();
            _id = id;
            _nombre = nombre;
            var documentBytes = Encoding.UTF8.GetBytes(letra);
            using (var reader = new MemoryStream(documentBytes))
            {
                reader.Position = 0;
                txt_letra.SelectAll();
                txt_letra.Selection.Load(reader, DataFormats.Rtf);
            }

            ColorPickerTab.SelectedColor = Colors.Black;
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                txt_letra.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            txt_letra.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void ColorPickerTab_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

            if (ColorPickerTab.SelectedColor != null)
                txt_letra.Selection.ApplyPropertyValue(ForegroundProperty, ColorPickerTab.SelectedColor.Value.ToString());
        }

        private async void btn_actualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rtfText; //string to save to db
                TextRange tr = new TextRange(txt_letra.Document.ContentStart, txt_letra.Document.ContentEnd);
                using (MemoryStream ms = new MemoryStream())
                {
                    tr.Save(ms, DataFormats.Rtf);
                    rtfText = Encoding.UTF8.GetString(ms.ToArray());
                }

                //insertamos en la base de datos
                new Datos.Da_letras().Actualizar_letra(_id, _nombre, rtfText, "");
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                await metroWindow.ShowMessageAsync("Información", "Los cambios realizados en la letra fueron guardados correctamente");
                //MessageBox.Show("Letra Actualizada correctamente");

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //EJEMPLO PARA CARGAR DATOS
        /*async void LoadData()
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            var controller = await metroWindow.ShowProgressAsync("Procesando", "Obtener datos de la base de datos",
                false, new MetroDialogSettings() { AnimateShow = true, ColorScheme = MetroDialogColorScheme.Theme });
            controller.SetIndeterminate();

            await viewModel.LoadData();

            await Dispatcher.BeginInvoke((Action)(async () =>
            {
                DataGrid1.ItemsSource = viewModel.AModels;

                await controller.CloseAsync();
            }));
        }*/
    }
}
