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

        public TabLetraContent(string letra)
        {
            InitializeComponent();
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
    }
}
