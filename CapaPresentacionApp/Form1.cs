using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacionApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string texto = txtCliente.Text;
            string limpio = new string(texto.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

            if (texto != limpio)
            {
                int pos = txtCliente.SelectionStart - 1;
                txtCliente.Text = limpio;
                txtCliente.SelectionStart = Math.Max(pos, 0);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Remueve separadores previos
            string entrada = txtMonto.Text.Replace(",", "");

            if (decimal.TryParse(entrada, out decimal monto))
            {
                // Formatea con separador de miles, sin decimales
                txtMonto.Text = monto.ToString("N0", CultureInfo.InvariantCulture);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime fecha = dateTimePicker1.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoPedido = comboBox1.SelectedItem.ToString();

            if (tipoPedido == "Online")
            {
                texurl.Enabled = true;
                texMetodoPago.Enabled = true;
                texSucursal.Enabled = false;
                texSucursal.Text = ""; // limpiar si cambia
            }
            else if (tipoPedido == "Presencial")
            {
                texurl.Enabled = false;
                texurl.Text = ""; // limpiar si cambia
                texMetodoPago.Enabled = false;
                texMetodoPago.Text = "";
                texSucursal.Enabled = true;
            }
        }

        private void texurl_TextChanged(object sender, EventArgs e)
        {
            string url = texurl.Text.Trim();
        }

        private void texMetodoPago_TextChanged(object sender, EventArgs e)
        {
            string metodoPago = texMetodoPago.Text;
        }

        private void texSucursal_TextChanged(object sender, EventArgs e)
        {
            string sucursal = texSucursal.Text;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                string tipoPedido = comboBox1.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtMonto.Text))
                {
                    MessageBox.Show("Por favor complete todos los campos obligatorios.");
                    return;
                }

                if (!decimal.TryParse(txtMonto.Text, out decimal monto))
                {
                    MessageBox.Show("El monto ingresado no es válido.");
                    return;
                }

                if (tipoPedido == "Online")
                {
                    PedidoOnline pedido = new PedidoOnline
                    {
                        Cliente = txtCliente.Text,
                        MontoTotal = monto,
                        Fecha = dateTimePicker1.Value,
                        TipoPedido = tipoPedido,
                        Url = texurl.Text,
                        MetodoPago = texMetodoPago.Text
                    };

                    negocio.InsertarPedidoOnline(pedido);
                }
                else if (tipoPedido == "Presencial")
                {
                    PedidoPresencial pedido = new PedidoPresencial
                    {
                        Cliente = txtCliente.Text,
                        MontoTotal = monto,
                        Fecha = dateTimePicker1.Value,
                        TipoPedido = tipoPedido,
                        Sucursal = texSucursal.Text
                    };

                    negocio.InsertarPedidoPresencial(pedido);
                }
                else
                {
                    MessageBox.Show("Seleccione un tipo de pedido válido.");
                    return;
                }

                MessageBox.Show("Pedido guardado correctamente.");
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el pedido: " + ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            txtCliente.Clear();
            txtMonto.Clear();
            texurl.Clear();
            texMetodoPago.Clear();
            texSucursal.Clear();
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Today;
            txtIdPedido.Clear();
        }

        private void txtIdPedido_TextChanged(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                dataGridView1.DataSource = negocio.ListarPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar pedidos: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPedido.Text))
            {
                MessageBox.Show("Por favor seleccione un pedido para actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPedido = int.Parse(txtIdPedido.Text);
            string cliente = txtCliente.Text;
            decimal monto;
            if (!decimal.TryParse(txtMonto.Text, out monto))
            {
                MessageBox.Show("Ingrese un monto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime fecha = dateTimePicker1.Value;
            string tipoPedido = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tipoPedido))
            {
                MessageBox.Show("Seleccione el tipo de pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                PedidoNegocio negocio = new PedidoNegocio();

                if (tipoPedido == "Online")
                {
                    PedidoOnline pedido = new PedidoOnline
                    {
                        IdPedido = idPedido,
                        Cliente = cliente,
                        MontoTotal = monto,
                        Fecha = fecha,
                        TipoPedido = tipoPedido,
                        Url = texurl.Text,
                        MetodoPago = texMetodoPago.Text
                    };

                    negocio.ActualizarPedidoOnline(pedido);
                }
                else if (tipoPedido == "Presencial")
                {
                    PedidoPresencial pedido = new PedidoPresencial
                    {
                        IdPedido = idPedido,
                        Cliente = cliente,
                        MontoTotal = monto,
                        Fecha = fecha,
                        TipoPedido = tipoPedido,
                        Sucursal = texSucursal.Text
                    };

                    negocio.ActualizarPedidoPresencial(pedido);
                }
                else
                {
                    MessageBox.Show("Tipo de pedido no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Pedido actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtIdPedido.Text))
                {
                    MessageBox.Show("Por favor, seleccione un pedido de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idPedido = int.Parse(txtIdPedido.Text);

                PedidoNegocio negocio = new PedidoNegocio();
                negocio.EliminarPedido(idPedido);

                MessageBox.Show("Pedido eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = negocio.ListarPedidos();
                LimpiarFormulario();
            }
            catch (FormatException)
            {
                MessageBox.Show("El Id del pedido no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtIdPedido.Text = row.Cells["IdPedido"].Value?.ToString();
                txtCliente.Text = row.Cells["Cliente"].Value?.ToString();
                txtMonto.Text = row.Cells["MontoTotal"].Value?.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["Fecha"].Value);
                comboBox1.SelectedItem = row.Cells["TipoPedido"].Value?.ToString();

                string tipo = row.Cells["TipoPedido"].Value?.ToString();
                texurl.Clear();
                texMetodoPago.Clear();
                texSucursal.Clear();

                if (tipo == "Online")
                {
                    texurl.Text = row.Cells["Url"].Value?.ToString();
                    texMetodoPago.Text = row.Cells["MetodoPago"].Value?.ToString();
                }
                else if (tipo == "Presencial")
                {
                    texSucursal.Text = row.Cells["Sucursal"].Value?.ToString();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Se puede dejar vacío o usar en otro caso si es necesario
        }
    }
}
