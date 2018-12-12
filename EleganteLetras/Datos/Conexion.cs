using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;

namespace EleganteLetras.Datos
{
    public class Conexion
    {
        private SQLiteConnection sqliteConnection;

        /// <summary>
        /// Constructor que inicializa la base de datos Sqlite
        /// </summary>
        public Conexion()
        {
            sqliteConnection = new SQLiteConnection("Data Source=letrasdb.sqlite;Version=3;");
        }

        /// <summary>
        /// Metodo para abrir la conexion a la base de datos
        /// </summary>
        /// <returns></returns>
        public bool AbrirConexion()
        {
            try
            {
                if (sqliteConnection != null && sqliteConnection.State == ConnectionState.Closed)
                {
                    sqliteConnection.Open();
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error de coexión con la base de datos:" + ex.ToString(), "Mensaje del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Metodo para cerrar conexion de la base de datos
        /// </summary>
        /// <returns></returns>
        public bool CerrarConexion()
        {
            try
            {
                sqliteConnection.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString(), "Mensaje del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Metodo que retorna la conexion a la base de datos
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection RetornarConexion()
        {
            return sqliteConnection;
        }


    }
}
