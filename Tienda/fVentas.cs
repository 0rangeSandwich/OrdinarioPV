using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Tienda
{

    public partial class fVentas : Form
    {
        //valor para hacer el calculo total de las unidades a comprar (se usa en dataGridView1_CellClick)
        double valor1, valor2, Total;

        //Declaramos idProducto para usarlo dentro de los botones y la busqueda
        private string idProducto = "";

        //Declaramos idVenta para usarlo para la compra
        private string idVenta = "";
        //intRow para mostrar el numero de filas en la datagridview
        private int intRow = 0;




        public fVentas()
        {
            InitializeComponent();

        }

        //Al abrir la ventana, llamara a la funcion para cargar los datos de la base de datos
        private void fVentas_Load(object sender, EventArgs e)
        {
            //llamada a la funcion
            cargarDatos("");
        }


        private void LimpiarCampos()
        {
            //referencia al objeto que se esta usando y limpiar los campos
            this.idProducto = string.Empty;

            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPrecioPublico.Text = "";
            txtPrecioTotal.Text = "";

            //Dentro de los parentesis aparece id del producto que se haya seleccionado
            btnComprar.Text = "Comprar";

            //Limpiar campo de busqueda
            txtBusqueda.Clear();

            //Mostrara la primera fila en color azul
            if (txtBusqueda.CanSelect)
            {
                txtBusqueda.Select();
            }

        }

        //Metodo para mostrar toda la BD de productos
        private void cargarDatos(string busqueda)
        {
            //CONCAT(CAST(idproducto as varchar) Se hace una conversion con el operador CAST para pasar datos numericos a cadena, esto para la busqueda
            //La busqueda se puede hacer por idproducto //TRIM remueve categoria de la busqueda
            CRUD.sql = "SELECT idproducto, codigo, nombreproducto, descripcion, unidades, preciopublico FROM \"Productos\" " +
                       "WHERE CONCAT(CAST(idproducto as varchar), ' ',CAST(codigo as varchar), ' ', nombreproducto, ' ',  " +
                       "descripcion, ' ', unidades, ' ', preciopublico) LIKE @busqueda::varchar " +
                       " ORDER BY idProducto ASC";

            //Variable que nos servira para acomodar los datos en el datagridview
            string strBusqueda = string.Format("%{0}%", busqueda);

            //Instancia de NpgsqlCommand con la consulta y la conexion, junto con la busqueda realizada
            CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("busqueda", strBusqueda);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            //Calcula el numero de filas
            if (dt.Rows.Count > 0)
            {
                intRow = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                intRow = 0;
            }

            //StripStatus concatena con el numero de filas
            toolStripStatusLabel1.Text = "Numero de fila(s): " + intRow.ToString();

            //variable para configurar la datagridview
            DataGridView dgv1 = dataGridView1;

            //No se puede seleccionar varias filas, se autogeneran las columnas
            dgv1.MultiSelect = false;
            dgv1.AutoGenerateColumns = true;
            dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //obtiene el origen de los datos a traves de la variable dt (Datatable)
            dgv1.DataSource = dt;

            //Nombre las columnas y el orden en el que aparecen
            dgv1.Columns[0].HeaderText = "ID Producto";
            dgv1.Columns[1].HeaderText = "Codigo";
            dgv1.Columns[2].HeaderText = "Nombre";
            dgv1.Columns[3].HeaderText = "Descripcion";
            dgv1.Columns[4].HeaderText = "Unidades";
            dgv1.Columns[5].HeaderText = "precioPublico";

            //Tamano de los espacios de cada uno
            dgv1.Columns[0].Width = 65;
            dgv1.Columns[1].Width = 65;
            dgv1.Columns[2].Width = 150;
            dgv1.Columns[3].Width = 300;
            dgv1.Columns[4].Width = 90;
            dgv1.Columns[5].Width = 100;
        }

        //Metodo para mostrar la lista de productos a compras
        private void cargarDatos2(string busqueda)
        {
            //CONCAT(CAST(idproducto as varchar) Se hace una conversion con el operador CAST para pasar datos numericos a cadena, esto para la busqueda
            //La busqueda se puede hacer por idproducto //TRIM remueve categoria de la busqueda
            CRUD.sql = "SELECT codigo, nombreproducto, cantidad, preciounitario, preciototal FROM \"Ventas\" " +
                       "WHERE CONCAT(CAST(codigo as varchar), ' ', nombreproducto, ' ',  " +
                       "cantidad, ' ', preciounitario,' ', preciototal) LIKE @busqueda::varchar " +
                       " ORDER BY nombreProducto ASC";

            //Variable que nos servira para acomodar los datos en el datagridview
            string strBusqueda = string.Format("%{0}%", busqueda);

            //Instancia de NpgsqlCommand con la consulta y la conexion, junto con la busqueda realizada
            CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("busqueda", strBusqueda);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            //Calcula el numero de filas
            if (dt.Rows.Count > 0)
            {
                intRow = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                intRow = 0;
            }

            //StripStatus concatena con el numero de filas
            toolStripStatusLabel1.Text = "Numero de fila(s): " + intRow.ToString();

            //variable para configurar la datagridview
            DataGridView dgv1 = dataGridView2;

            //No se puede seleccionar varias filas, se autogeneran las columnas
            dgv1.MultiSelect = false;
            dgv1.AutoGenerateColumns = true;
            dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //obtiene el origen de los datos a traves de la variable dt (Datatable)
            dgv1.DataSource = dt;

            //Nombre las columnas y el orden en el que aparecen
            dgv1.Columns[0].HeaderText = "Codigo";
            dgv1.Columns[1].HeaderText = "Nombre";
            dgv1.Columns[2].HeaderText = "Cantidad";
            dgv1.Columns[3].HeaderText = "precio Unitario";
            dgv1.Columns[4].HeaderText = "precio Total";

            //Tamano de los espacios de cada uno
            dgv1.Columns[0].Width = 65;
            dgv1.Columns[1].Width = 150;
            dgv1.Columns[2].Width = 90;
            dgv1.Columns[3].Width = 90;
            dgv1.Columns[4].Width = 100;
        }

        //Metodo para ejecutar las respectivas consultas a la base de datos
        private void execute(string mySQL, string param)
        {
            //Instancia de NpgsqlCommand
            CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
            //se llama a la funcion y con el parametro de la opcion elegida (ej. insertar)
            agregarParametros(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }


        //los paramtros son declarados segun la base de datos, trim quita los espacios en blanco
        private void agregarParametros(string str)
        {


            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("codigo", txtCodigo.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("nombreProducto", txtNombre.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("cantidad", int.Parse(txtCantidad.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("precioPublico", decimal.Parse(txtPrecioPublico.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("precioTotal", decimal.Parse(txtPrecioTotal.Text.Trim()));

            //Si se selecciona una opcion y no esta vacio se modifican o elimina los parametros
            if (str == "Guardar" && !string.IsNullOrEmpty(this.idProducto))
            {
                CRUD.cmd.Parameters.AddWithValue("idVenta", this.idProducto);
            }


        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Comprobacion de campos vacios
            if (string.IsNullOrEmpty(txtBusqueda.Text.Trim()))
            {
                MessageBox.Show("Introduce un codigo.", "Buscar: AVISO",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

                //no regresa nada
                cargarDatos("");
            }
            else
            {
                //Recibe el campo de texto (Puede buscar por idproducto/codigo
                cargarDatos(txtBusqueda.Text.Trim());
            }

            
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            //verificacion de campos vacios
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()) || string.IsNullOrEmpty(txtNombre.Text.Trim())
                || string.IsNullOrEmpty(txtCantidad2.Text.Trim()) || string.IsNullOrEmpty(txtPrecioPublico.Text.Trim())
                || string.IsNullOrEmpty(txtPrecioTotal.Text.Trim()))
            {
                //Advertencia
                MessageBox.Show("Por favor, llena todos los campos", "Comprar: Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Se declara la consulta para insertar dentro de la variable sql
            CRUD.sql = "INSERT INTO \"Ventas\" (codigo, nombreproducto, cantidad, preciounitario, preciototal) " +
                "VALUES(@codigo, @nombreProducto, @cantidad,@precioPublico, @precioTotal)";

            //llama a la funcion y le pasa el parametro "comprar"
            execute(CRUD.sql, "Comprar");

            //Actualiza el DGV2
            cargarDatos2("");

            //Limpia el formulario
            LimpiarCampos();
            txtCantidad2.Text = "";

            //Actualiza el DGV
            cargarDatos("");
        }

        //Metodo para seleccionar un elemento de la tabla para eliminar/modificar
        //Mostrar los datos en el formulario
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridView dgv1 = dataGridView1;

                //convierte a cadena el idproducto segun la fila seleccionada  y lo inserta dentro de los parentesis
                this.idProducto = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                btnComprar.Text = "Comprar (" + this.idProducto + ")";


                //Total = Convert.ToInt32(txtCantidad.Text) * float.Parse(dgv1.CurrentRow.Cells[5].Value);

                //Convierte a cadena los campos
                txtCodigo.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                txtNombre.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                txtCantidad2.Text = (txtCantidad.Text);
                txtPrecioPublico.Text = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);

                //Conversion a numeros la cadena de los campos para calcular el total
                valor1 = Convert.ToDouble(txtCantidad2.Text);
                valor2 = Convert.ToDouble(txtPrecioPublico.Text);

                Total = valor1 * valor2;

                txtPrecioTotal.Text = Convert.ToString(Total);
            }
        }
    }
}
