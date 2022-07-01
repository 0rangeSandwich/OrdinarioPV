using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Tienda
{
    public partial class fProveedores : Form
    {

        private string idProveedor = "";
        private int intRow = 0;

        public fProveedores()
        {
            InitializeComponent();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {

            this.idProveedor = string.Empty;

            txtNombre.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";

            //limpiar comboBox
            if (comboBoxDias.Items.Count > 0)
            {
                comboBoxDias.SelectedIndex = 0;
            }

            txtCantidades.Text = "";

            //limpiar comboBox
            if (comboBoxUnidad.Items.Count > 0)
            {
                comboBoxUnidad.SelectedIndex = 0;
            }

            //limpiar comboBox
            if (comboBoxFormapago.Items.Count > 0)
            {
                comboBoxFormapago.SelectedIndex = 0;
            }

            //limpiar comboBox
            if (comboBoxCredito.Items.Count > 0)
            {
                comboBoxCredito.SelectedIndex = 0;
            }


            txtPlazo.Text = "";

            

            btnModificar.Text = "Modificar ()";
            btnEliminar.Text = "Eliminar ()";

            txtBusqueda.Clear();

            if (txtBusqueda.CanSelect)
            {
                txtBusqueda.Select();
            }

        }

        private void fProveedores_Load(object sender, EventArgs e)
        {
            cargarDatos("");
        }


        //Metodo para mostrar toda la BD
        private void cargarDatos(string busqueda)
        {
            
            //CONCAT(CAST(idproducto as varchar) Se hace una conversion con el operador CAST para pasar datos numericos a cadena, esto para la busqueda
            CRUD.sql = "SELECT idproveedor, nombreproveedor, direccion, telefono, dias, cantidades, cajaunidad, formapago, pagocredito, plazo FROM \"Proveedores\" " +
                       "WHERE CONCAT(CAST(idproveedor as varchar), ' ',CAST(nombreproveedor as varchar), ' ' , direccion, ' ' , telefono, ' ' , dias, ' ' , " +
                       "cantidades, ' ' , cajaunidad, ' ' , formapago, ' ' , pagocredito, ' ' , plazo) LIKE @busqueda::varchar " +
                       "ORDER BY idProveedor ASC";

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

            dgv1.Columns[0].HeaderText = "ID Proveedor";
            dgv1.Columns[1].HeaderText = "Nombre";
            dgv1.Columns[2].HeaderText = "Direccion";
            dgv1.Columns[3].HeaderText = "Telefono";
            dgv1.Columns[4].HeaderText = "Dia Surtido";
            dgv1.Columns[5].HeaderText = "Cantidades a surtir";
            dgv1.Columns[6].HeaderText = "Unidad/Caja";
            dgv1.Columns[7].HeaderText = "Forma de pago";
            dgv1.Columns[8].HeaderText = "Pago a credito";
            dgv1.Columns[9].HeaderText = "Tiempo de plazo";

            dgv1.Columns[0].Width = 65;
            dgv1.Columns[1].Width = 210;
            dgv1.Columns[2].Width = 210;
            dgv1.Columns[3].Width = 70;
            dgv1.Columns[4].Width = 60;
            dgv1.Columns[5].Width = 90;
            dgv1.Columns[6].Width = 90;
            dgv1.Columns[7].Width = 70;
            dgv1.Columns[8].Width = 70;
            dgv1.Columns[9].Width = 150;
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
            CRUD.cmd.Parameters.AddWithValue("nombreProveedor", txtNombre.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("direccion", txtDireccion.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("telefono", int.Parse(txtTelefono.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("dias", comboBoxDias.SelectedItem.ToString());
            CRUD.cmd.Parameters.AddWithValue("cantidades", int.Parse(txtCantidades.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("cajaunidad", comboBoxUnidad.SelectedItem.ToString());
            CRUD.cmd.Parameters.AddWithValue("formapago", comboBoxFormapago.SelectedItem.ToString());
            CRUD.cmd.Parameters.AddWithValue("pagoCredito", comboBoxCredito.SelectedItem.ToString());
            CRUD.cmd.Parameters.AddWithValue("plazo", txtPlazo.Text.Trim());

            

            if (str == "Modificar" || str == "Eliminar" && !string.IsNullOrEmpty(this.idProveedor))
            {
                CRUD.cmd.Parameters.AddWithValue("idProveedor", this.idProveedor);
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()) || string.IsNullOrEmpty(txtDireccion.Text.Trim()) || string.IsNullOrEmpty(txtTelefono.Text.Trim()) 
                || string.IsNullOrEmpty(txtCantidades.Text.Trim()) || string.IsNullOrEmpty(txtPlazo.Text.Trim()))
            {
                MessageBox.Show("Por favor, llena todos los campos", "Insertar: Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "INSERT INTO \"Proveedores\" (nombreproveedor, direccion, telefono, dias, cantidades, cajaunidad, formapago, pagocredito, plazo) " +
                "VALUES(@nombreProveedor, @direccion, @telefono, @dias, @cantidades, @cajaunidad, @formapago, @pagoCredito, @plazo)";

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

                this.idProveedor = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                btnModificar.Text = "Modificar (" + this.idProveedor + ")";
                btnEliminar.Text = "Eliminar (" + this.idProveedor + ")";

                txtNombre.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                txtDireccion.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                txtTelefono.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                comboBoxDias.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
                txtCantidades.Text = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);
                comboBoxUnidad.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[6].Value);
                comboBoxFormapago.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[7].Value);
                comboBoxCredito.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[8].Value);
                txtPlazo.Text = Convert.ToString(dgv1.CurrentRow.Cells[9].Value);

            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.idProveedor))
            {
                MessageBox.Show("Selecciona un proveedor de la lista.", "Modificar : AVISO",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(txtNombre.Text.Trim()) || string.IsNullOrEmpty(txtDireccion.Text.Trim()) 
                || string.IsNullOrEmpty(txtTelefono.Text.Trim()) || string.IsNullOrEmpty(txtCantidades.Text.Trim()) 
                || string.IsNullOrEmpty(txtPlazo.Text.Trim()))
            {
                MessageBox.Show("Por favor, llena todos los campos", "Insertar: Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "UPDATE \"Proveedores\" SET nombreproveedor=@nombreproveedor, direccion=@direccion, telefono=@telefono, " +
                "dias=@dias, cantidades=@cantidades, cajaunidad=@cajaunidad, formapago=@formapago, pagocredito=@pagocredito, " +
                "plazo=@plazo WHERE idproveedor = @idProveedor::integer";

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

            if (string.IsNullOrEmpty(this.idProveedor))
            {
                MessageBox.Show("Selecciona un proveedor de la lista.", "Eliminar: AVISO",
                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Deseas eliminar este proveedor?", "Eliminar: AVISO",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                CRUD.sql = "DELETE FROM \"Proveedores\" WHERE idproveedor = @idProveedor::integer";

                execute(CRUD.sql, "Modificar");

                MessageBox.Show("El proveedor fue eliminado.", "Eliminar: Completado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                cargarDatos("");

                LimpiarCampos();

            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusqueda.Text.Trim()))
            {
                MessageBox.Show("Introduce un idproveedor o nombre.", "Buscar: AVISO",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

                cargarDatos("");
            }
            else
            {
                //Recibe el campo de texto (Puede buscar por idproveedor/nombre
                cargarDatos(txtBusqueda.Text.Trim());
            }

            LimpiarCampos();
        }
    }
}
