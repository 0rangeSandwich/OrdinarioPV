using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Tienda
{
    public partial class fProductos : Form
    {

        private string idProducto = "";
        private int intRow = 0;

        public fProductos()
        {
            InitializeComponent();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {

            this.idProducto = string.Empty;

            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtUnidades.Text = "";
            txtPrecioProveedor.Text = "";
            txtPrecioUnitario.Text = "";
            txtPrecioPublico.Text = "";
            txtPresentacion.Text = "";
            txtFechaEntrada.Text = "";

            //limpiar comboBox
            if (comboBoxCategoria.Items.Count > 0)
            {
                comboBoxCategoria.SelectedIndex = 0;
            }

            btnModificar.Text = "Modificar ()";
            btnEliminar.Text = "Eliminar ()";

            txtBusqueda.Clear();

            if (txtBusqueda.CanSelect)
            {
                txtBusqueda.Select();
            }

        }


        private void fProductos_Load(object sender, EventArgs e)
        {
            cargarDatos("");
        }

        //Metodo para mostrar toda la BD
        private void cargarDatos(string busqueda)
        {
            //CONCAT(CAST(idproducto as varchar) Se hace una conversion con el operador CAST para pasar datos numericos a cadena, esto para la busqueda
            CRUD.sql = "SELECT idproducto, codigo, nombreproducto, descripcion, unidades, precioproveedor, preciounitario, preciopublico, presentacion, fechaentrada, categoria FROM \"Productos\" " +
                       "WHERE CONCAT(CAST(idproducto as varchar), ' ',CAST(codigo as varchar), ' ', nombreproducto, ' ',  descripcion, ' ', unidades, ' ', precioproveedor, ' ', preciounitario, ' ', preciopublico, ' ', presentacion, ' ', fechaentrada, ' ', categoria) LIKE @busqueda::varchar " +
                       "OR TRIM(categoria) LIKE @busqueda::varchar ORDER BY idProducto ASC";

            string strBusqueda = string.Format("%{0}%", busqueda);

            CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("busqueda", strBusqueda);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

            if (dt.Rows.Count > 0)
            {
                intRow = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                intRow = 0;
            }

            toolStripStatusLabel1.Text = "Numero de fila(s): " + intRow.ToString();

            DataGridView dgv1 = dataGridView1;

            dgv1.MultiSelect = false;
            dgv1.AutoGenerateColumns = true;
            dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv1.DataSource = dt;

            dgv1.Columns[0].HeaderText = "ID Producto";
            dgv1.Columns[1].HeaderText = "Codigo";
            dgv1.Columns[2].HeaderText = "Nombre";
            dgv1.Columns[3].HeaderText = "Descripcion";
            dgv1.Columns[4].HeaderText = "Unidades";
            dgv1.Columns[5].HeaderText = "precioProveedor";
            dgv1.Columns[6].HeaderText = "precioUnitario";
            dgv1.Columns[7].HeaderText = "precioPublico";
            dgv1.Columns[8].HeaderText = "Presentacion";
            dgv1.Columns[9].HeaderText = "FechaEntrada";
            dgv1.Columns[10].HeaderText = "Categoria";

            dgv1.Columns[0].Width = 65;
            dgv1.Columns[1].Width = 65;
            dgv1.Columns[2].Width = 150;
            dgv1.Columns[3].Width = 300;
            dgv1.Columns[4].Width = 60;
            dgv1.Columns[5].Width = 90;
            dgv1.Columns[6].Width = 90;
            dgv1.Columns[7].Width = 90;
            dgv1.Columns[8].Width = 150;
            dgv1.Columns[9].Width = 80;
            dgv1.Columns[10].Width = 70;
        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
            agregarParametros(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void agregarParametros(string str)
        {
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("codigo", txtCodigo.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("nombreProducto", txtNombre.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("descripcion", txtDescripcion.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("unidades", int.Parse(txtUnidades.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("precioProveedor", decimal.Parse(txtPrecioProveedor.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("precioUnitario", decimal.Parse(txtPrecioUnitario.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("precioPublico", decimal.Parse(txtPrecioPublico.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("presentacion", txtPresentacion.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("fechaEntrada", txtFechaEntrada.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("categoria", comboBoxCategoria.SelectedItem.ToString());

            if (str == "Modificar" || str == "Eliminar" && !string.IsNullOrEmpty(this.idProducto))
            {
                CRUD.cmd.Parameters.AddWithValue("idProducto", this.idProducto);
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()) || string.IsNullOrEmpty(txtNombre.Text.Trim()) || string.IsNullOrEmpty(txtDescripcion.Text.Trim()) || string.IsNullOrEmpty(txtUnidades.Text.Trim()) || string.IsNullOrEmpty(txtPrecioProveedor.Text.Trim()) || string.IsNullOrEmpty(txtPrecioUnitario.Text.Trim()) || string.IsNullOrEmpty(txtPrecioPublico.Text.Trim()) || string.IsNullOrEmpty(txtPresentacion.Text.Trim()) || string.IsNullOrEmpty(txtFechaEntrada.Text.Trim()))
            {
                MessageBox.Show("Por favor, llena todos los campos", "Insertar: Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "INSERT INTO \"Productos\" (codigo, nombreproducto, descripcion, unidades, precioproveedor, preciounitario, preciopublico, presentacion, fechaentrada, categoria) VALUES(@codigo, @nombreProducto, @descripcion, @unidades, @precioProveedor, @precioUnitario, @precioPublico, @presentacion, @fechaEntrada, @categoria)";

            execute(CRUD.sql, "Insertar");

            MessageBox.Show("Se han guardado los datos.", "Insertar: Completado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            cargarDatos("");

            LimpiarCampos();
        }

        //Metodo para seleccionar un elemento de la tabla para eliminar/modificar
        //Mostrar los datos en el formulario
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridView dgv1 = dataGridView1;

                this.idProducto = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                btnModificar.Text = "Modificar (" + this.idProducto + ")";
                btnEliminar.Text = "Eliminar (" + this.idProducto + ")";

                txtCodigo.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                txtNombre.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                txtDescripcion.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                txtUnidades.Text = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
                txtPrecioProveedor.Text = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);
                txtPrecioUnitario.Text = Convert.ToString(dgv1.CurrentRow.Cells[6].Value);
                txtPrecioPublico.Text = Convert.ToString(dgv1.CurrentRow.Cells[7].Value);
                txtPresentacion.Text = Convert.ToString(dgv1.CurrentRow.Cells[8].Value);
                txtFechaEntrada.Text = Convert.ToString(dgv1.CurrentRow.Cells[9].Value);

                comboBoxCategoria.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[10].Value);

            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.idProducto))
            {
                MessageBox.Show("Selecciona un producto de la lista.", "Modificar : AVISO",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()) || string.IsNullOrEmpty(txtNombre.Text.Trim()) || string.IsNullOrEmpty(txtDescripcion.Text.Trim()) || string.IsNullOrEmpty(txtUnidades.Text.Trim()) || string.IsNullOrEmpty(txtPrecioProveedor.Text.Trim()) || string.IsNullOrEmpty(txtPrecioUnitario.Text.Trim()) || string.IsNullOrEmpty(txtPrecioPublico.Text.Trim()) || string.IsNullOrEmpty(txtPresentacion.Text.Trim()) || string.IsNullOrEmpty(txtFechaEntrada.Text.Trim()))
            {
                MessageBox.Show("Por favor, llena todos los campos", "Insertar: Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "UPDATE \"Productos\" SET codigo=@codigo, nombreproducto=@nombreProducto, descripcion=@descripcion, unidades=@unidades, precioproveedor=@precioProveedor, preciounitario=@precioUnitario, preciopublico=@precioPublico, presentacion=@presentacion, fechaentrada=@fechaEntrada, categoria=@categoria WHERE idproducto = @idProducto::integer";

            execute(CRUD.sql, "Modificar");

            MessageBox.Show("Registro actualizado.", "Modificar : Operacion Exitosa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            cargarDatos("");

            LimpiarCampos();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.idProducto))
            {
                MessageBox.Show("Selecciona un producto de la lista.", "Eliminar: AVISO",
                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Deseas eliminar este producto?", "Eliminar: AVISO",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                CRUD.sql = "DELETE FROM \"Productos\" WHERE idproducto = @idProducto::integer";

                execute(CRUD.sql, "Modificar");

                MessageBox.Show("El producto fue eliminado.", "Eliminar: Completado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                cargarDatos("");

                LimpiarCampos();

            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusqueda.Text.Trim()))
            {
                MessageBox.Show("Introduce un idproducto o codigo.", "Buscar: AVISO",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

                cargarDatos("");
            }
            else
            {
                //Recibe el campo de texto (Puede buscar por idproducto/codigo
                cargarDatos(txtBusqueda.Text.Trim());
            }

            LimpiarCampos();

        }

    }
}
