using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EleganteLetras.Datos
{
    public class Da_letras
    {
        //creacion del objeto de la base de datos
        private Conexion conexion;

        public Da_letras()
        {
            //inicializacion de la base de datos
            conexion = new Conexion();
        }

        /// <summary>
        /// Metodo para insertar una nueva letra
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="letra"></param>
        /// <param name="grupo"></param>
        public void Intertar_letra(int id, string nombre, string letra, string grupo)
        {
            try
            {
                if (conexion.AbrirConexion())
                {
                    //Create the command
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = @" INSERT INTO Letras VALUES (null, @nombre,@letra,@grupo)";
                    //sqlite_cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                    sqlite_cmd.Parameters.AddWithValue("@nombre",nombre);
                    sqlite_cmd.Parameters.AddWithValue("@letra", letra);
                    sqlite_cmd.Parameters.AddWithValue("@grupo", grupo);
                    // Now lets execute the SQL ;-)
                    sqlite_cmd.ExecuteNonQuery();
                    //MessageBox.Show("Letra guardada correctamente!");
                    conexion.CerrarConexion();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void Actualizar_letra(int id, string nombre, string letra, string grupo)
        {
            try
            {
                if (conexion.AbrirConexion())
                {
                    //Create the command
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = @" UPDATE Letras SET NOMBRE = @nombre, LETRA = @letra, GRUPO = @grupo WHERE Id = @id";
                    //sqlite_cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                    sqlite_cmd.Parameters.AddWithValue("@nombre", nombre);
                    sqlite_cmd.Parameters.AddWithValue("@letra", letra);
                    sqlite_cmd.Parameters.AddWithValue("@grupo", grupo);
                    sqlite_cmd.Parameters.AddWithValue("@id", id);
                    // Now lets execute the SQL ;-)
                    sqlite_cmd.ExecuteNonQuery();
                    //MessageBox.Show("Letra guardada correctamente!");
                    conexion.CerrarConexion();
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public Entidades.e_letras ObtenerLetra(string id)
        {
            Entidades.e_letras letra = new Entidades.e_letras();
            try
            {
                if (conexion.AbrirConexion())
                {
                    //Create the command
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = @" SELECT * FROM Letras WHERE Id ="+ id;
                    // Now lets execute the SQL ;-)


                    using (var reader = sqlite_cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            letra.Id = reader.GetInt32(0);
                            letra.Nombre = reader.GetString(1);
                            letra.Letra = reader.GetString(2);
                            letra.Grupo = reader.GetString(3);
                        }

                    }

                    conexion.CerrarConexion();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return letra;
        }

        public List<Entidades.e_letras> GetLetras()
        {
            List<Entidades.e_letras> data = new List<Entidades.e_letras>();
            
            try
            {
                if (conexion.AbrirConexion())
                {
                    //Create the command
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    // Let the SQLiteCommand object know our SQL-Query:
                    sqlite_cmd.CommandText = @" SELECT * FROM Letras";
                    // Now lets execute the SQL ;-)
                    

                    using (var reader = sqlite_cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entidades.e_letras letras = new Entidades.e_letras();
                            letras.Id = reader.GetInt32(0);
                            letras.Nombre = reader.GetString(1);
                            letras.Letra = reader.GetString(2);
                            letras.Grupo = reader.GetString(3);
                            data.Add(letras);
                        }
                       
                    }

                    conexion.CerrarConexion();
                }
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message); 
            }
            return data;
        }

        /// <summary>
        /// Metodo para eliminar una letra seleccionada
        /// </summary>
        /// <param name="cod"></param>
        /// <returns></returns>
        public bool Eliminar(int cod)
        {
            try
            {
                if (conexion.AbrirConexion())
                {
                    SQLiteCommand sqlite_cmd = conexion.RetornarConexion().CreateCommand();
                    sqlite_cmd.CommandText = @"DELETE FROM Letras WHERE Id="+cod;
                    sqlite_cmd.ExecuteNonQuery();
                }
                conexion.CerrarConexion();
                return true;
            }
            catch
            {
                conexion.CerrarConexion();
                return false;
            }
        }

    }
}
