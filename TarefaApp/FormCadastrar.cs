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
    public partial class FormCadastrar: Form
    {
        public FormCadastrar()
        {
            InitializeComponent();
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void btnCadastrar_Click_1(object sender, EventArgs e)
        {
            Conexao con = new Conexao();
            var conn = con.Conectar();
            string query = "INSERT INTO usuarios (nome, email, senha) VALUES (@nome, @email, @senha)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuário cadastrado com sucesso!");
            }
            catch
            {
                MessageBox.Show("Erro ao cadastrar. Email já existe?");
            }
            conn.Close();
        }
    }
}
