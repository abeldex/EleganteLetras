using System;
using System.Collections.Generic;
using System.IO;
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

namespace EleganteLetras
{
    /// <summary>
    /// Interaction logic for NuevaLetra.xaml
    /// </summary>
    public partial class NuevaLetra
    {
        public NuevaLetra()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            ColorPicker1.SelectedColor = Colors.Black;
        }

        private void btn_nuevo_Click(object sender, RoutedEventArgs e)
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
                new Datos.Da_letras().Intertar_letra(0, txt_titulo.Text, rtfText, txt_grupo.Text);
                MessageBox.Show("Letra Guardada correctamente");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
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

        private void ColorPicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //txt_letra.Foreground = new SolidColorBrush(ColorPicker1.SelectedColor.Value);

            if (ColorPicker1.SelectedColor != null)
                txt_letra.Selection.ApplyPropertyValue(ForegroundProperty, ColorPicker1.SelectedColor.Value.ToString());
        }
    }
}
