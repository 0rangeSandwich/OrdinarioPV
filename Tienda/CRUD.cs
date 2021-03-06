using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Tienda
{
    internal class CRUD
    {
        private static string getConnectionString()
        {
            //Datos de configuración para acceder a la db
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string db = "Database=tienda;";
            string user = "Username=postgres;";
            string pass = "Password=root;";

            //Cadena de conexión a la db
            string conString = string.Format("{0}{1}{2}{3}{4}", host, port, db, user, pass);

            //devuelve la conexion
            return conString;

        }
        
        //Creamos una instancia de la clase  NpgsqlConnection para establecer la conexión
        public static NpgsqlConnection con = new NpgsqlConnection(getConnectionString());
        public static NpgsqlCommand cmd = default(NpgsqlCommand);
        public static string sql = string.Empty;

        public static DataTable PerformCRUD(NpgsqlCommand com)
        {
            //Puente entre la BD y el formulario
            //NpgsqlDataAdapte para realizar las operaciones a la BD
            NpgsqlDataAdapter da = default(NpgsqlDataAdapter);
            //Creamos una instancia de DT
            DataTable dt = new DataTable();

            try
            {

                da = new NpgsqlDataAdapter();
                da.SelectCommand = com;
                //fill llena la tabla segun los datos de la BD
                da.Fill(dt);

                return dt;

            }
            catch (Exception ex)
            {
                //Advertencia y no podra continuar
                MessageBox.Show("Error: " + ex.Message, "Error al realizar las operaciones del CRUD",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                dt = null;
            }

            return dt;

        }
    }
}
