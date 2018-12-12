using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            txt_buscar.Text = dialog.SelectedPath;
        }

        private void txt_buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ImageList.Items.Clear();
            /**/
            try
            {
                //aqui vamos a hcaer lo otro w
                string[] supportedExtensions = new[] { ".doc", ".docx"};
                //var files = Directory.GetFiles(System.IO.Path.Combine(root, "Documentos"), "*.*").Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower()));
                var files = Directory.GetFiles(txt_buscar.Text, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower()));
                List<FileDetails> documentos = new List<FileDetails>();
                int count = files.Count();
                int current = 0;
                foreach (var file in files)
                {
                    current++;
                    Progreso.Value = current / count * 30 + 70;
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
                    //now open the RTF file using word.
                    //Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                    //Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(id.Path);
                    //wordApp.Visible = false;
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
                    //Microsoft.Office.Interop.Word.Document wordDoc = msWord.Documents.Open(id.Path);
                    
                }

                ImageList.ItemsSource = documentos;
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.Message);
            }
        }
    }
}
