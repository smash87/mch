﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.Formularios.FormulariosAyuda
{
    public partial class frmAyudaVideoMenuConfiguración : Form
    {
        frmAyudaVideo fForma;
        public frmAyudaVideoMenuConfiguración(frmAyudaVideo F2)
        {
            InitializeComponent();
            fForma = F2;
        }

        private void pbCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            fForma.Show();
        }

        private void frmAyudaVideoMenuConfiguración_FormClosing(object sender, FormClosingEventArgs e)
        {
            fForma.Show();
        }

        private void btnConfiguración_Click(object sender, EventArgs e)
        {
            Formularios.frmReproducirVideo fvidereproducir;
            fvidereproducir = new Formularios.frmReproducirVideo("Configuracion Sistema SE.wmv", "Configuracion Sistema", this, "Configuracion Sistema.wmv");
            this.Hide();
            fvidereproducir.Show();
        }
    }
}
