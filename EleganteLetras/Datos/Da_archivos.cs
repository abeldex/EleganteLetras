using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EleganteLetras.Datos
{
    public class Da_archivos
    {
        public List<FileDetails> GetArchivos(string path)
        {
            try
            {
                var files = GetFiles(path, "*.doc", true);
                List<FileDetails> documentos = new List<FileDetails>();
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

                return documentos;
                //ImageList.ItemsSource = documentos;
                //progress_circular.Visibility = Visibility.Hidden;
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.Message);
                return null;
            }
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
    }
}
