﻿using Proyecto.Logica;
using Proyecto.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class frmTipoMoneda : Form
    {
        public frmTipoMoneda()
        {
            InitializeComponent();
        }

        private void frmTipoMoneda_Load(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            var lista = TipoMonedaLogica.Instancia.Listar(out mensaje);

            foreach (TipoMoneda item in lista)
            {
                dgvdata.Rows.Add(new object[] { item.IdTipoMoneda, item.Descripcion, "", "" });
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 2)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.edit16.Width;
                var h = Properties.Resources.edit16.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.edit16, new Rectangle(x, y, w, h));
                e.Handled = true;
            }

            if (e.ColumnIndex == 3)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.delete16.Width;
                var h = Properties.Resources.delete16.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.delete16, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (index >= 0) {
                if (dgvdata.Columns[e.ColumnIndex].Name == "btneliminar")
                {
                    if (MessageBox.Show("¿Desea eliminar el tipo de moneda?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int _id = int.Parse(dgvdata.Rows[index].Cells["Id"].Value.ToString());
                        string mensaje = string.Empty;

                        int valida = TipoMonedaLogica.Instancia.Validar(_id, out mensaje);

                        if (valida > 0)
                        {
                            MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            int respuesta = TipoMonedaLogica.Instancia.Eliminar(_id);

                            if (respuesta > 0)
                            {
                                dgvdata.Rows.RemoveAt(index);
                            }
                            else
                                MessageBox.Show("No se pudo eliminar el tipo de moneda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else if (dgvdata.Columns[e.ColumnIndex].Name == "btneditar")
                {
                    txtindice.Text = index.ToString();
                    txtid.Text = dgvdata.Rows[index].Cells["Id"].Value.ToString();
                    txtdescripcion.Text = dgvdata.Rows[index].Cells["Descripcion"].Value.ToString();
                }
            }

            
        }
        private void Limpiar()
        {

            txtindice.Text = "-1";
            txtid.Text = "0";
            txtdescripcion.Text = "";
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            int id = Convert.ToInt32(txtid.Text);

            if (txtdescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar una descripcion correcta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            int existe = TipoMonedaLogica.Instancia.Existe(txtdescripcion.Text, id, out mensaje);

            if (existe == 1)
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (id == 0)
            {
                int idgenerado = TipoMonedaLogica.Instancia.Guardar(new TipoMoneda() { Descripcion = txtdescripcion.Text }, out mensaje);

                if (idgenerado < 1)
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                dgvdata.Rows.Add(new object[] { idgenerado, txtdescripcion.Text, "", "" });
            }
            else
            {
                int respuesta = TipoMonedaLogica.Instancia.Editar(new TipoMoneda() { Descripcion = txtdescripcion.Text, IdTipoMoneda = id }, out mensaje);

                if (respuesta < 1)
                {
                    MessageBox.Show("No se pudo editar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    int index = Convert.ToInt32(txtindice.Text);
                    dgvdata.Rows[index].Cells["Descripcion"].Value = txtdescripcion.Text;
                }
            }
            Limpiar();
        }
    }
}
