﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tienda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formulario = new fProductos();
            formulario.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formulario = new fProveedores();
            formulario.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
