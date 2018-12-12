using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleganteLetras.Datos
{
    public class Tablas
    {
        //creacion del objeto de la base de datos
        private Conexion conexion;

        /// <summary>
        /// Constructor 
        /// </summary>
        public Tablas()
        {
            //inicializacion de la base de datos
            conexion = new Conexion();
        }

        public void VerificarTablaLetras()
        {
            try
            {
                if (conexion.AbrirConexion())
                {
                    //Create the command
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS
                                              [Letras] (
                                              [Id]     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                              [NOMBRE]   TEXT NOT NULL,
                                              [LETRA] TEXT NOT NULL,
                                              [GRUPO] TEXT NULL)";
                    // Now lets execute the SQL ;-)
                    sqlite_cmd.ExecuteNonQuery();
                    conexion.CerrarConexion();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
