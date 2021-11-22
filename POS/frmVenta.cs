using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POS
{
    public partial class frmVenta : Form
    {
        public string strUsuario = "";
        private bool editando = false;
        private int editandoRenglon = 0;
        public frmVenta()
        {
            InitializeComponent();
        }
        public frmVenta(string usuario)
        {
            InitializeComponent();
            this.strUsuario = usuario;
        }
        private void CalcularTotales()
        {
           float totales = 0.0f;
            for(int i = 0; i < dgv_Productos.Rows.Count; i++)
            {
                totales += float.Parse(dgv_Productos[3,i].Value.ToString());
            }
            txt_Total.Text=totales.ToString();

        }
        private void txt_Codigo_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.KeyValue == 80)
            {
                
                float pago = float.Parse(txt_Codigo.Text);
                float total = float.Parse(txt_Total.Text);
                labMensajes.Text = "Su cambio es de: " + (pago - total);
                MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNo;
                DialogResult dialogResult = MessageBox.Show(labMensajes.Text,"... Pagando ...", messageBoxButtons);
                
                if (dialogResult== DialogResult.Yes)
                {
                    dgv_Productos.Rows.Clear();
                    txt_Codigo.Text = "";
                    txt_Total.Text = "";
                    labMensajes.Text = "";
                }
                else
                {
                    labMensajes.Text = "";
                    txt_Codigo.Text = "";
                }
                
              
                return;
            }
            if (e.KeyValue == 13)
            {
                try
                {
                    if (txt_Codigo.Text == "")
                    {

                    }
                    else
                    {
                        var query = "select id, nombre, precio, precio as total from productos where id='" + txt_Codigo.Text + "'";

                        MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost; user=root; database=pos; SSL Mode=none");
                        mySqlConnection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                        MySqlDataReader mySqlDataReader = cmd.ExecuteReader();
                        if (mySqlDataReader.HasRows)
                        {
                            mySqlDataReader.Read();
                            dgv_Productos.Rows.Add("1", mySqlDataReader.GetString(1), mySqlDataReader.GetString(2), mySqlDataReader.GetString(3),"Eliminar");
                            CalcularTotales();
                            txt_Codigo.Text = "";
                            editando = false;
                        }
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Erro al consultar codigo--> "+ex.Message);
                }
            }
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            dgv_Productos.Width = this.Width - 30;
            lab_Usuario.Text = strUsuario;

            editando = false;

           
        }

        private void frmVenta_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dgv_Productos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                dgv_Productos.Rows.RemoveAt(e.RowIndex);
                CalcularTotales();
            }
            if (e.ColumnIndex == 0)
            {
                editandoRenglon = e.RowIndex;
                editando = true;
               
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labHora.Text = DateTime.Now.ToString("hh:mm:ss");
            labFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
  
        }

        private void dgv_Productos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.ColumnIndex == 0 & editando==true)
            {
               // MessageBox.Show("cambio valor");
                dgv_Productos.Rows[e.RowIndex].Cells[4].Value =  int.Parse( dgv_Productos.Rows[e.RowIndex].Cells[0].ToString()) * float.Parse( dgv_Productos.Rows[e.RowIndex].Cells[3].ToString());
                CalcularTotales();

            }
        }

        private void dgv_Productos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Test");
        }
    }
}
