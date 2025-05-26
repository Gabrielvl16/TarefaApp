using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TarefaApp
{
    public partial class FormLogin: Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Conexao con = new Conexao();
            var conn = con.Conectar();
            string query = "SELECT * FROM usuarios WHERE email=@email AND senha=@senha";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int userId = reader.GetInt32("id");
                string nome = reader.GetString("nome");
                MessageBox.Show("Bem-vindo, " + nome);

                FormPrincipal tf = new FormPrincipal(userId);
                tf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login inválido.");
            }
            conn.Close();
        }

        private void linkCadastrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormCadastrar cadastrar = new FormCadastrar();
            cadastrar.Show();
        }
    }
}
