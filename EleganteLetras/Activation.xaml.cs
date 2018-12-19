using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace EleganteLetras
{
    /// <summary>
    /// Interaction logic for Activation.xaml
    /// </summary>
    public partial class Activation : IDisposable
    {
        public byte[] CertificatePublicKeyData { private get; set; }

        public Activation()
        {
            InitializeComponent();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Assign the application information values to the license control
            licActCtrl.AppName = "EleganteLetrasApp";
            licActCtrl.LicenseObjectType = typeof(DemoLicense.MyLicense);
            licActCtrl.CertificatePublicKeyData = this.CertificatePublicKeyData;
            //Display the device unique ID
            licActCtrl.ShowUID();
        }

        private async void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Call license control to validate the license string
            if (licActCtrl.ValidateLicense())
            {
                //If license if valid, save the license string into a local file
                File.WriteAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "license.lic"), licActCtrl.LicenseBASE64String);
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "OK",
                    AnimateShow = true,
                    AnimateHide = false
                };
                //System.Windows.Forms.MessageBox.Show("Licencia aceptada, La aplicación se cerrará para aplicar los cambios. Por favor vuelva a abrirla de nuevo");
                await this.ShowMessageAsync("¡Producto Activado!", "¡La aplicación se cerrará para aplicar los cambios. Por favor vuelva a abrirla de nuevo!", MessageDialogStyle.Affirmative, mySettings);
                this.Close();
            }
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            /*if (System.Windows.Forms.MessageBox.Show("¿Estas seguro que quieres cancelar?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }*/
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Si",
                NegativeButtonText = "No",
                AnimateShow = true,
                AnimateHide = false
            };

            var result = await this.ShowMessageAsync("¿Cancelar la activación?", "¡Si cancelas la activación no podrás utilizar la aplicación!", MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result.ToString() == "Affirmative")
            {
                this.Close();
            }

        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
