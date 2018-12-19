using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLicense.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ActivarLicencia.xaml
    /// </summary>
    public partial class ActivarLicencia : UserControl
    {
        public string AppName { get; set; }

        public byte[] CertificatePublicKeyData { private get; set; }

        public bool ShowMessageAfterValidation { get; set; }

        public Type LicenseObjectType { get; set; }

        public string LicenseBASE64String
        {
            get
            {
                string richText = new TextRange(txtLicense.Document.ContentStart, txtLicense.Document.ContentEnd).Text;
                return richText.Trim();
                //return txtLicense.Text.Trim();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivarLicencia()
        {
            InitializeComponent();
            ShowMessageAfterValidation = true;
        }

        /// <summary>
        /// Muestra el ID Generado
        /// </summary>
        public void ShowUID()
        {
            txtUID.Text = LicenseHandler.GenerateUID(AppName);
        }

        /// <summary>
        /// Metodo para validar la licencia
        /// </summary>
        /// <returns></returns>
        public bool ValidateLicense()
        {
            string richText = new TextRange(txtLicense.Document.ContentStart, txtLicense.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(richText))
            {
                MessageBox.Show("¡Por favor ingrese la licencia!");
                return false;
            }

            //Check the activation string
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            string _msg = string.Empty;
            LicenseEntity _lic = LicenseHandler.ParseLicenseFromBASE64String(LicenseObjectType, richText.Trim(), CertificatePublicKeyData, out _licStatus, out _msg);
            switch (_licStatus)
            {
                case LicenseStatus.VALID:
                    if (ShowMessageAfterValidation)
                    {
                        MessageBox.Show(_msg, "Felicidades ¡La licencia es VALIDA!");
                    }

                    return true;

                case LicenseStatus.CRACKED:
                case LicenseStatus.INVALID:
                case LicenseStatus.UNDEFINED:
                    if (ShowMessageAfterValidation)
                    {
                        MessageBox.Show(_msg, "¡La licencia ingresada es INVALIDA!");
                    }

                    return false;

                default:
                    return false;
            }
        }

        private void link_copiar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(txtUID.Text);
            MessageBox.Show("¡ID copiado!");
        }
    }
}
