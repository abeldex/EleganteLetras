using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using SautinSoft.Document;


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


        private void btn_seleccionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = GetFiles(txt_buscar.Text, "*.doc", true);
                List<FileDetails> documentos = new List<FileDetails>();
                foreach (var file in files)
                {
                    FileDetails id = new FileDetails()
                    {
                        Path = file,
                        FileName = System.IO.Path.GetFileNameWithoutExtension(file),
                        Extension = System.IO.Path.GetExtension(file)
                    };

                    ImageSource imageSource;

                    using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(file))
                    {
                        imageSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }

                    // I couldn't find file size in BitmapImage
                    FileInfo fi = new FileInfo(file);
                    id.Size = fi.Length;
                    id.icono = imageSource;
                    documentos.Add(id);
                    //guardar en la base de datos si es un archivo de word
                    string rtfText;
                    // Step 1: Load a docx document.
                    DocumentCore dc = DocumentCore.Load(id.Path, LoadOptions.DocxDefault);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        dc.Save(ms, SaveOptions.RtfDefault);
                        //tr.Save(ms, DataFormats.Rtf);
                        rtfText = Encoding.UTF8.GetString(ms.ToArray());
                    }
                    new Datos.Da_letras().Intertar_letra(0, id.FileName, rtfText, "");

                }
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
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
            progress_circular.Visibility = Visibility.Visible;
            try
            {

                var files = GetFiles(txt_buscar.Text, "*.doc", true);
                List <FileDetails> documentos = new List<FileDetails>();
                foreach (var file in files)
                {
                    //current++;
                    //Progreso.Value = current / count * 30 + 70;
                    FileDetails id = new FileDetails()
                    {
                        Path = file,
                        FileName = System.IO.Path.GetFileNameWithoutExtension(file),
                        Extension = System.IO.Path.GetExtension(file)
                    };

                    ImageSource imageSource;

                    using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(file))
                    {
                        imageSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }

                    // I couldn't find file size in BitmapImage
                    FileInfo fi = new FileInfo(file);
                    id.Size = fi.Length;
                    id.icono = imageSource;
                    documentos.Add(id);
                }

                ImageList.ItemsSource = documentos;
                progress_circular.Visibility = Visibility.Hidden;
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.Message);
            }
        }

        private void txt_buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(LoadFolderFiles), null);
        }
    }
}
