﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EleganteLetras
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        MaterialDesignThemes.Wpf.PaletteHelper materialPaletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();

        
        public MainWindow()
        {
            InitializeComponent();
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            border.Visibility = Visibility.Collapsed;

            try
            {
                Datos.Tablas tablas = new Datos.Tablas();
                tablas.VerificarTablaLetras();
                //MessageBox.Show("Tablas Verificadas");
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void txt_buscar_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            var data = new Datos.Da_letras().GetLetras();

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear   
                resultStack.Children.Clear();
                border.Visibility = Visibility.Collapsed;
            }
            else
            {
                border.Visibility = Visibility.Visible;
            }

            // Clear the list   
            resultStack.Children.Clear();

            // Add the result   
            foreach (var obj in data)
            {
                if (obj.Nombre.ToLower().Contains(query.ToLower()))
                {
                    // The wordcontains this... Autocomplete must work   
                    addItem(obj.Nombre, obj.Id.ToString());
                    found = true;
                }
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No se encontró ningun resultado." });
            }

        }

        private void addItem(string text, string id)
        {
            TextBlock block = new TextBlock();
            block.FontSize = 16; // 24 points
            block.Inlines.Add(new Bold(new Run(text)));
            block.TextWrapping = TextWrapping.Wrap;
            // Add the text   
            //block.Text = text;

            // A little style...   
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events   
            block.MouseLeftButtonUp += (sender, e) =>
            {
                //txt_buscar.Text = (sender as TextBlock).Text;
                var inline = (Bold)(sender as TextBlock).Inlines.ElementAt(0);
                var textRange = new TextRange(inline.ContentStart, inline.ContentEnd);
                txt_buscar.Text = textRange.Text;
                lbl_id_evento.Content = id;
                var border = (resultStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = Visibility.Collapsed;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Foreground = Brushes.LightGreen;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Foreground = Brushes.Black;
            };

            // Add to the panel   
            resultStack.Children.Add(block);
            Separator separator = new Separator();
            resultStack.Children.Add(separator);
        }



        private void LightThemeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            materialPaletteHelper.SetLightDark(false);
            DarkThemeCheckBox.IsChecked = false;
        }

        private void DarkThemeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            materialPaletteHelper.SetLightDark(true);
            LightThemeCheckBox.IsChecked = false;
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt_buscar.Text = "";
            lbl_id_evento.Content = "";
            txt_buscar.Focus();
        }

        private void btn_seleccionar_Click(object sender, RoutedEventArgs e)
        {
            //obtener el contenido de la letra y mostrarla en el textview
            card_letra.Visibility = Visibility.Visible;
            Entidades.e_letras letra = new Datos.Da_letras().ObtenerLetra(lbl_id_evento.Content.ToString());

            //Creamos un nuevo tab para la cancion seleccionada
            TabItem aTabItem = new TabItem();
            aTabItem.Name = "tab"+letra.Id;
            aTabItem.Header = letra.Nombre;
            aTabItem.Content = new TabLetraContent(letra.Letra);
            aTabItem.IsSelected = true;
            aTabItem.AllowDrop = true;
            mainTabControl.Items.Insert(mainTabControl.Items.Count, aTabItem);
            
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NuevaLetra nl = new NuevaLetra();
            nl.Show();
        }

        private void ListBoxItem_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            ImportarLetras il = new ImportarLetras();
            il.Show();
        }
    }


}