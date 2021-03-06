﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class frmPagos : Form
    {
        frmMenu fForma2;
        Boolean gb_NuevoContrato;
        public frmPagos(frmMenu F2)
        {
            InitializeComponent();
            fForma2 = F2;
            OcultaMostrarrControlesIngreso(true);
            OcultarMotrarPnlGrid(false);
            OcultarMostrarPB(false);

            //Se marcan los controles que tienen "Ayuda"
            //txIDCliente.Tag = "true";
            txtIDContador.Tag = "true";
            dgvPagosPendientes.AutoGenerateColumns = false;
        }

        private void mntPersona_FormClosed(object sender, FormClosedEventArgs e)
        {
            fForma2.Show();
        }
        #region CONTROLES
        private void OcultaMostrarrControlesIngreso(Boolean  bCambio)
        {
            txtNombre.Visible = bCambio;
            //txIDCliente.Visible = bCambio;
            //txtNombreCliente.Visible = bCambio;
            txtIDContador.Visible = bCambio;
            txtDireccionContador.Visible = bCambio;
            txtDocumento.Visible = bCambio;
            //txtFechaFin.Visible = bCambio;
            lblCodigo.Visible = bCambio;
            //lblCliente.Visible = bCambio;
            lblContador.Visible = bCambio;
            lblFechaInicial.Visible = bCambio;
            //lblFechaFinal.Visible = bCambio;
            //pbxCliente.Visible = bCambio;
            pbxContador.Visible = bCambio;
            pbCobros.Visible=bCambio;
            lblCobros.Visible = bCambio;
        }
        private void LimpiarControlesIngreso()
        {
            txtNombre.Text = "";
            //txIDCliente.Text = "";
           // txtNombreCliente.Text = "";
            txtIDContador.Text = "";
            txtDireccionContador.Text = "";
            txtDocumento.Text = "";
            //txtFechaFin.Text = "";
        }
        private void BloquearDesbloquearControlesIngreso(Boolean bCambio) 
        {
            txtNombre.Enabled = bCambio;
            //txIDCliente.Enabled = bCambio;
            txtIDContador.Enabled = bCambio;
            //pbxCliente.Visible = bCambio;
        }
        private void OcultarMotrarPnlGrid(Boolean bCambio)
        {
            pnlResultados.Visible = bCambio;
        }
        private void OcultarMostrarPB(Boolean bCambio)
        {
            //pbEliminar.Visible = bCambio;
            pbGuardar.Visible = bCambio;
            pbCancelar.Visible = bCambio;
            //lblEliminar.Visible = bCambio;
            lblGuardar.Visible = bCambio;
            lblCancelar.Visible = bCambio;
            pbCobros.Visible = bCambio;
            lblCobros.Visible = bCambio;
        }

        #endregion

        private void pbNuevaPersona_Click(object sender, EventArgs e)
        {
            
            gb_NuevoContrato = true;
            OcultaMostrarrControlesIngreso(true);
            OcultarMostrarPB(false);
            BloquearDesbloquearControlesIngreso(true);
            pbGuardar.Visible = true;
            lblGuardar.Visible = true;
            pbCancelar.Visible = true;
            lblCancelar.Visible = true;
           
            OcultarMotrarPnlGrid(false);
        }

        private void CrearContrato()
        {
            int iResultado = 0;
            int iSecuencial = 0;
            string sMensaje = string.Empty;
            string sIDPersona = ""; //txIDCliente.Text;
            string sIDContador = txtIDContador.Text;
            Boolean bGrabar = true;

            // Se realizan validaciones

            // Se manda a almacenar en la BD
            if (bGrabar)
            {
                // AR20121129 Cambio para devolver el correlativo del contrato
                Datos.CrearContrato(sIDPersona, sIDContador, ref sMensaje, ref iResultado, ref iSecuencial);

                switch (iResultado)
                {
                    case 0:
                        MessageBox.Show("EL CONTRATO SE CREÓ CORRECTAMENTE.");
                        txtNombre.Text = iSecuencial.ToString();
                        BloquearDesbloquearControlesIngreso(false);
                        OcultarMostrarPB(false);
                        break;
                    case -1:
                        MessageBox.Show("NO EXISTE EL USUARIO INDICADO.");
                        break;
                    case -2:
                        MessageBox.Show("NO EXISTE EL CONTADOR INDICADO.");
                        break;
                    case -3:
                        MessageBox.Show("ERROR AL MARCAR COMO ELIMINADO EL CONTRATO ACTUAL PARA EL CONTADOR.");
                        break;
                    case -4:
                        MessageBox.Show("ERROR AL CREAR EL NUEVO CONTRATO PARA EL CONTADOR.");
                        break;
                    default:
                        MessageBox.Show("ERROR DESCONOCIDO AL ALMACENAR EL CONTRATO.");
                        break;
                }

            }
        }

        private void RealizarPago()
        {
            int iResultado = 0;
            string sMensaje = string.Empty;
            string sLectura = dgvPagosPendientes.SelectedRows[0].Cells["lectura"].Value.ToString(); //txIDCliente.Text;
            string dMonto = dgvPagosPendientes.SelectedRows[0].Cells["monto"].Value.ToString();
            string scliente = dgvPagosPendientes.SelectedRows[0].Cells["nombre"].Value.ToString();
            string sSaldo = tb_Saldo.Text;
            string sPago = tb_Pago.Text;
            string sPagoTotal = tb_TotalACancelar.Text;
            if (Convert.ToDouble(sPagoTotal)<=Convert.ToDouble(dMonto))
            {
                if (cb_UtilizaSaldo.Checked==true)
                {
                    Datos.RealizarPago(sPagoTotal, sLectura, ref sMensaje, ref iResultado);

                    switch (iResultado)
                    {
                        case 0:

                            MessageBox.Show("EL PAGO SE REALIZO CORRECTAMENTE.");
                            break;
                        default:
                            MessageBox.Show("ERROR AL REALIZAR EL PAGO");
                            break;
                    }
                    int contador = Convert.ToInt32(dgvPagosPendientes.SelectedRows[0].Cells["id_contador"].Value.ToString());
                    Datos.InsertarSaldoPersona(contador, Convert.ToDouble(sSaldo)*-1, ref iResultado);
                    Datos.IngresoSalidaCaja(Convert.ToDouble(sSaldo), -1, "Salida por saldo a favor del cliente: "+scliente,ref iResultado);
                    Datos.InsertarReciboPago(Convert.ToInt32(sLectura), Convert.ToDouble(sPago), Sesion.Log, ref iResultado);
                    Formularios.viewReciboPago fRecibo;
                    fRecibo = new Formularios.viewReciboPago(this,Convert.ToInt32(sLectura));
                    fRecibo.Show();
                }
                else
                {
                    Datos.RealizarPago(sPagoTotal, sLectura, ref sMensaje, ref iResultado);

                    switch (iResultado)
                    {
                        case 0:

                            MessageBox.Show("EL PAGO SE REALIZO CORRECTAMENTE.");
                            break;
                        default:
                            MessageBox.Show("ERROR AL REALIZAR EL PAGO");
                            break;
                    }
                    Datos.InsertarReciboPago(Convert.ToInt32(sLectura), Convert.ToDouble(sPago), Sesion.Log, ref iResultado);
                    Formularios.viewReciboPago fRecibo;
                    fRecibo = new Formularios.viewReciboPago(this, Convert.ToInt32(sLectura));
                    fRecibo.Show();
                }
            }
            else
            {
                double dSaldoAFavor = Convert.ToDouble(sPagoTotal) - Convert.ToDouble(dMonto);
                if (cb_UtilizaSaldo.Checked == true)
                {
                    Datos.RealizarPago(dMonto, sLectura, ref sMensaje, ref iResultado);

                    switch (iResultado)
                    {
                        case 0:

                            MessageBox.Show("EL PAGO SE REALIZO CORRECTAMENTE.");
                            break;
                        default:
                            MessageBox.Show("ERROR AL REALIZAR EL PAGO");
                            break;
                    }
                    int contador = Convert.ToInt32(dgvPagosPendientes.SelectedRows[0].Cells["id_contador"].Value.ToString());
                    Datos.InsertarSaldoPersona(contador, Convert.ToDouble(sSaldo) * -1, ref iResultado);
                    Datos.IngresoSalidaCaja(Convert.ToDouble(sSaldo), -1, "Salida por saldo a favor del cliente: "+scliente,ref iResultado);
                    Datos.InsertarSaldoPersona(contador, Convert.ToDouble(dSaldoAFavor), ref iResultado);
                    Datos.IngresoSalidaCaja(Convert.ToDouble(dSaldoAFavor), 1, "Ingreso por saldo a favor del cliente: "+scliente, ref iResultado);
                    Datos.InsertarReciboPago(Convert.ToInt32(sLectura), Convert.ToDouble(sPago), Sesion.Log, ref iResultado);
                    Formularios.viewReciboPago fRecibo;
                    fRecibo = new Formularios.viewReciboPago(this, Convert.ToInt32(sLectura));
                    fRecibo.Show();
                }
                else
                {
                    Datos.RealizarPago(dMonto, sLectura, ref sMensaje, ref iResultado);

                    switch (iResultado)
                    {
                        case 0:

                            MessageBox.Show("EL PAGO SE REALIZO CORRECTAMENTE.");
                            break;
                        default:
                            MessageBox.Show("ERROR AL REALIZAR EL PAGO");
                            break;
                    }
                    int contador = Convert.ToInt32(dgvPagosPendientes.SelectedRows[0].Cells["id_contador"].Value.ToString());
                    Datos.InsertarSaldoPersona(contador, Convert.ToDouble(dSaldoAFavor), ref iResultado);
                    Datos.IngresoSalidaCaja(Convert.ToDouble(dSaldoAFavor), 1, "Ingreso por saldo a favor del cliente: "+scliente,ref  iResultado);
                    Datos.InsertarReciboPago(Convert.ToInt32(sLectura), Convert.ToDouble(sPago), Sesion.Log, ref iResultado);
                    Formularios.viewReciboPago fRecibo;
                    fRecibo = new Formularios.viewReciboPago(this, Convert.ToInt32(sLectura));
                    fRecibo.Show();
                }
            }

            
            

            
            // Se realizan validaciones

            // Se manda a almacenar en la BD
            //Datos.RealizarPago(dMonto, sLectura, ref sMensaje, ref iResultado);

            //switch (iResultado)
            //{
            //    case 0:

            //        MessageBox.Show("EL PAGO SE REALIZO CORRECTAMENTE.");
            //        break;
            //    default:
            //        MessageBox.Show("ERROR AL REALIZAR EL PAGO");
            //        break;
            //}


        }

        private void EliminarContrato()
        {
            Int32 iDato = 0;
            Int32 iResultado = 0;
            string sMensaje = "";
            iDato = Convert.ToInt32( txtNombre.Text);
            DialogResult drResultado = MessageBox.Show("A CONTINUACIÓN SE ELIMINARÁ EL CONTRATO " + txtNombre.Text + " ¿DESEA CONTINUAR?", "ELIMINACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drResultado == DialogResult.Yes)
            {
                Datos.EliminarContrato(iDato, ref sMensaje, ref iResultado);
                if (iResultado == 0)
                {
                    MessageBox.Show("EL CONTRATO FUE ELIMINADO EXISTOSAMENTE");
                }
                else
                {
                    MessageBox.Show("ERROR EN LA ELIMINACIÓN");
                }
            }
            BuscarContrato();
        }

        private void BuscarContrato()
        {
            string sDatosBusqueda = "";
            string sDatosBusqueda2 = "";
            bool bDatosHistoricos = false;
            string sMensaje = "";
            Int32 iResultado = 0;
            DataTable dtResultado = new DataTable();
            
            //sDatosBusqueda =  txtNombreBusqueda.Text.Trim();
            //sDatosBusqueda2 = txtNombreBusqueda2.Text.Trim();
            //bDatosHistoricos = chkHistorico.Checked;

            LimpiarControlesIngreso();

            if ((sDatosBusqueda.Length > 0) || (sDatosBusqueda2.Length > 0))
            {
                Datos.BuscarContrato(sDatosBusqueda, sDatosBusqueda2, bDatosHistoricos, ref sMensaje, ref iResultado, ref dtResultado);
         
                if (iResultado == 0)
                {
                    if (dtResultado.DefaultView.Count > 0)
                    {
                        OcultarMotrarPnlGrid(true);
                        OcultarMostrarPB(true);
                        OcultaMostrarrControlesIngreso(true);
                        BloquearDesbloquearControlesIngreso(true);
                        dgvPagosPendientes.DataSource = dtResultado.DefaultView;
                        //MostrarDatosContrato(0);
                    }
                    else
                    {
                        MessageBox.Show("NO SE ENCONTRARON RESULTADOS");
                    }
                }
                else
                {
                    MessageBox.Show("ERROR EN LA BÚSQUEDA");
                }
            }
            else {
                MessageBox.Show("INGRESE UN NOMBRE PARA REALIZAR LA BÚSQUEDA");
            }
        }

        private void BuscarPagosPendientes()
        {
            string strIdContador = "";
            string sMensaje = "";
            Int32 iResultado = 0;
            DataTable dtResultado = new DataTable();
            
            strIdContador = txtIDContador.Text;

            //LimpiarControlesIngreso();
            dgvPagosPendientes.DataSource = null;
            dgvPagosPendientes.Rows.Clear();
            if ((strIdContador.Length > 0))
            {
                Datos.BuscarPagosPendientes(strIdContador, ref sMensaje, ref iResultado, ref dtResultado);

                if (iResultado == 0)
                {
                    if (dtResultado.DefaultView.Count > 0)
                    {
                        OcultarMotrarPnlGrid(true);
                        OcultarMostrarPB(true);
                        OcultaMostrarrControlesIngreso(true);
                        BloquearDesbloquearControlesIngreso(true);
                        dgvPagosPendientes.DataSource = dtResultado.DefaultView;
                        MostrarDatosPersona();
                    }
                    else
                    {
                        MessageBox.Show("NO SE ENCONTRARON PAGOS PENDIENTES PARA ESTE CONTADOR");
                    }
                }
                else
                {
                    MessageBox.Show("ERROR EN LA BÚSQUEDA");
                }
            }
        }

        private void MostrarDatosPersona()
        {
            //if ((iDato >= 0) && (iDato < dgvPagosPendientes.Rows.Count))
            //{
            //    txtCodigo.Text = dgvPagosPendientes[0, iDato].Value.ToString();
            //    //txIDCliente.Text = dgvContrato[1, iDato].Value.ToString();
            //    //txtNombreCliente.Text = dgvContrato[2, iDato].Value.ToString();
            //    txtIDContador.Text = dgvPagosPendientes[3, iDato].Value.ToString();
            //    txtFechaInicio.Text = dgvPagosPendientes[4, iDato].Value.ToString();
            //    txtFechaFin.Text = dgvPagosPendientes[5, iDato].Value.ToString();
            //}
            if (dgvPagosPendientes.Rows.Count>0)
            {
                txtDocumento.Text = dgvPagosPendientes["CUI", 0].Value.ToString();
                txtNombre.Text = dgvPagosPendientes["nombre", 0].Value.ToString();
                //******************mostrar Saldo*******************************
                DataTable dt_Saldo= new DataTable();
                int iresul=0;
                Datos.GetSaldoPersona(Convert.ToInt32(txtIDContador.Text), ref iresul, ref dt_Saldo);
                tb_Saldo.Text = dt_Saldo.Rows[0]["Saldo"].ToString();
                tb_Saldo.Visible = true;
                tb_Pago.Visible = true;
                tb_TotalACancelar.Visible = true;
                lb_pago.Visible = true;
                lb_Saldo.Visible = true;
                lb_total.Visible = true;
                if (tb_Saldo.Text!=""&& Convert.ToDouble(tb_Saldo.Text)>0)
                {
                    cb_UtilizaSaldo.Visible = true;
                    cb_UtilizaSaldo.Checked = true;
                }
                else
                {
                    cb_UtilizaSaldo.Visible = false;
                    cb_UtilizaSaldo.Checked = false;
                }
                //**********************************************************************
            }

        }

        private void pbCancelar_Click(object sender, EventArgs e)
        {
            if (gb_NuevoContrato )
            {
                OcultaMostrarrControlesIngreso(false);
                OcultarMotrarPnlGrid(false);
                OcultarMostrarPB(false);
            }
        }

        private void pbBuscar_Click(object sender, EventArgs e)
        {
            gb_NuevoContrato = false;
            BuscarContrato();
        }

        private void txtNombreBusqueda_TextChanged(object sender, EventArgs e)
        {
            //txtNombreBusqueda.CharacterCasing = CharacterCasing.Upper;
        }

        private void dgvPersonas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 iDato = 0;
            iDato = e.RowIndex;
            //MostrarDatosContrato(iDato);
        }

        private void pbGuardar_Click(object sender, EventArgs e)
        {
            if (tb_Pago.Text!="")
            {
                if (dgvPagosPendientes.Rows.Count > 0)
                {
                    if (dgvPagosPendientes.SelectedRows.Count > 0)
                    {
                        if (dgvPagosPendientes.SelectedRows[0].Index == 0)
                        {
                            RealizarPago();
                            tb_Pago.Text = "";
                            tb_TotalACancelar.Text = "";
                            tb_Saldo.Text = "";
                            txtIDContador_LostFocus(sender,e);
                        }
                        else
                        {
                            MessageBox.Show("DEBE PAGAR LA LECTURA MAS ANTIGUA");
                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar el monto a pagar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void pbEliminar_Click(object sender, EventArgs e)
        {
            EliminarContrato();
        }

        private void txtIDContador_TextChanged(object sender, EventArgs e)
        {
            txtIDContador.CharacterCasing = CharacterCasing.Upper;
        }

        private void txtNombreBusqueda2_TextChanged(object sender, EventArgs e)
        {
            //txtNombreBusqueda2.CharacterCasing = CharacterCasing.Upper;
        }

        private void txIDCliente_TextChanged(object sender, EventArgs e)
        {
            //txIDCliente.CharacterCasing = CharacterCasing.Upper;
        }

        private void pbxCliente_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    frmAyuda AyudaCliente;
            //    AyudaCliente = new frmAyuda("Listado de clientes", "A", ref txIDCliente, ref txtNombreCliente);
            //    AyudaCliente.ShowDialog();
            //}
            //catch
            //{
            //    MessageBox.Show("ERROR EN LA BÚSQUEDA");
            //}
        }

        private void txIDCliente_LostFocus(object sender, EventArgs e)
        {
            //try
            //{
            //    frmAyuda.BuscarAyudaIndividual("cliente", "A", ref txIDCliente, ref txtNombreCliente);
            //}
            //catch
            //{
            //    MessageBox.Show("ERROR EN LA BÚSQUEDA");
            //}
        }

        private void pbxContador_Click(object sender, EventArgs e)
        {
            try
            {
                frmAyuda AyudaContador;
                AyudaContador = new frmAyuda("Listado de contadores", "B", ref txtIDContador, ref txtDireccionContador);
                AyudaContador.ShowDialog();
            }
            catch
            {
                MessageBox.Show("ERROR EN LA BÚSQUEDA");
            }
        }

        private void txtIDContador_LostFocus(object sender, EventArgs e)
        {
            try
            {
                frmAyuda.BuscarAyudaIndividual("contador", "B", ref txtIDContador, ref txtDireccionContador);
                //BUSCAR DATOS DEL CONTRATO
                BuscarPagosPendientes();


            }
            catch
            {
                MessageBox.Show("ERROR EN LA BÚSQUEDA");
            }
        }

        private void txtIDContador_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dgvPagosPendientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvPagosPendientes.AutoGenerateColumns = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
          /* this.dgvPagosPendientes.it*/
            if (dgvPagosPendientes.RowCount>0)
            {

            int numero = dgvPagosPendientes.CurrentCell.RowIndex;
            frmSelectCobroAdicional frmSelect;
            frmSelect = new frmSelectCobroAdicional();
            frmSelect.Monto = Convert.ToDecimal(dgvPagosPendientes[1, numero].Value);
            frmSelect.Lectura = Convert.ToInt32(dgvPagosPendientes[2, numero].Value);
            frmSelect.ShowDialog();
            BuscarPagosPendientes();
                
   
        
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tb_Pago_TextChanged(object sender, EventArgs e)
        {
           if (cb_UtilizaSaldo.Checked==true)
           {
               double saldo, pago;
               try
               {
                   saldo = Convert.ToDouble(tb_Saldo.Text);
               }
               catch (Exception)
               {

                   saldo = 0;
               }
               try
               {
                   pago = Convert.ToDouble(tb_Pago.Text);
               }
               catch (Exception)
               {

                   pago = 0;
               }
                tb_TotalACancelar.Text = Convert.ToString(pago+saldo);
           }
           else
           {
                tb_TotalACancelar.Text = tb_Pago.Text;
           }
           
        }

        private void cb_UtilizaSaldo_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_UtilizaSaldo.Checked == true)
            {
                try
                {
                    tb_TotalACancelar.Text = Convert.ToString(Convert.ToDouble(tb_Saldo.Text) + Convert.ToDouble(tb_Pago.Text));
                }
                catch (Exception)
                {

                    tb_TotalACancelar.Text = tb_Saldo.Text;
                }
                
            }
            else
            {
                tb_TotalACancelar.Text = tb_Pago.Text;
            }
        }

        private void lblGuardar_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Formularios.frmEliminarPago fEPago;
            fEPago = new Formularios.frmEliminarPago(this);
            this.Hide();
            fEPago.Show();
        }

     

    }
}
