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
    public partial class FormPrincipal: Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        int usuarioId;

        public FormPrincipal(int id)
        {
            InitializeComponent();
            usuarioId = id;
            CarregarTarefas();
        }

        private void CarregarTarefas()
        {
            Conexao con = new Conexao();
            var conn = con.Conectar();
            string query = "SELECT * FROM tarefas WHERE usuario_id=@usuario_id";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId); // precisa usar o ID do usuário logado

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            Conexao con = new Conexao();
            var conn = con.Conectar();
            string query = "INSERT INTO tarefas (usuario_id, descricao, prazo) VALUES (@usuario, @desc, @prazo)";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@usuario", usuarioId);
            cmd.Parameters.AddWithValue("@desc", txtDescricao.Text);
            cmd.Parameters.AddWithValue("@prazo", dtPrazo.Value.Date);

            cmd.ExecuteNonQuery();
            conn.Close();
            CarregarTarefas();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se alguma linha está selecionada
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecione uma tarefa para editar.");
                    return;
                }

                // Pega o ID da linha selecionada (coluna 0 = ID)
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                string novaDescricao = txtDescricao.Text.Trim();
                DateTime novoPrazo = dtPrazo.Value;

                // Verifica se a descrição está vazia
                if (string.IsNullOrWhiteSpace(novaDescricao))
                {
                    MessageBox.Show("A descrição não pode estar vazia.");
                    return;
                }

                // Atualiza no banco de dados
                Conexao con = new Conexao();
                var conn = con.Conectar();

                string query = "UPDATE tarefas SET descricao=@desc, prazo=@prazo WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@desc", novaDescricao);
                cmd.Parameters.AddWithValue("@prazo", novoPrazo);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Tarefa atualizada com sucesso!");
                CarregarTarefas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar tarefa: " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se uma linha foi selecionada
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecione uma tarefa para excluir.");
                    return;
                }

                // Confirmação com o usuário
                DialogResult resultado = MessageBox.Show("Tem certeza que deseja excluir esta tarefa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.No)
                {
                    return;
                }

                // Pega o ID da tarefa selecionada
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value); // ou Cells["id"].Value se tiver certeza do nome

                // Conexão e exclusão
                Conexao con = new Conexao();
                var conn = con.Conectar();
                string query = "DELETE FROM tarefas WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Tarefa excluída com sucesso!");
                CarregarTarefas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir tarefa: " + ex.Message);
            }
        }

        private void btnConcluir_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se alguma linha está selecionada
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecione uma tarefa para marcar como concluída.");
                    return;
                }

                // Pega o ID da tarefa selecionada
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value); // Ou Cells["id"] se o nome estiver correto

                // Conexão e atualização
                Conexao con = new Conexao();
                var conn = con.Conectar();
                string query = "UPDATE tarefas SET concluida = 1 WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Tarefa marcada como concluída!");
                CarregarTarefas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao concluir tarefa: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtDescricao.Text = dataGridView1.Rows[e.RowIndex].Cells["descricao"].Value.ToString();
                dtPrazo.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["prazo"].Value);
            }
        }
    }
}
