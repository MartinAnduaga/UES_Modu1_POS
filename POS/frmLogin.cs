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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void txt_Usuario_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void btn_Login_MouseClick(object sender, MouseEventArgs e)
        {

            //procLogin();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txt_Usuario.Text = "";
            txt_Password.Text = "";
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            procLogin();
        }
        private void procLogin()
        {
            //codigo para cargar validar
            if (txt_Usuario.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre de usuario");
                return;
            }
            else if (txt_Password.Text == "")
            {
                MessageBox.Show("Debe ingresar una contraseña");
                return;
            }
            else
            {
                string query = "SELECT * FROM USUARIOS WHERE nombreUser='" + txt_Usuario.Text + "' AND pass='" + txt_Password.Text + "'";
                MySqlConnection conn = new MySqlConnection("Server=localhost; User=root; database=pos; SSL Mode=none");
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader mySqlDataReader = cmd.ExecuteReader();
                    if (mySqlDataReader.HasRows)
                    {
                        mySqlDataReader.Read();
                        this.Hide();
                       
                        
                        new frmVenta(txt_Usuario.Text).ShowDialog();
                        this.Show();

                    }
                    else
                    {
                        MessageBox.Show("Error en usuario y/o contraseña");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error --> " + ex.Message);
                }

            }
        }
    }
}
