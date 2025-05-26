using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public class Conexao
{
    private string conexao = "server=localhost;database=tarefa_app;uid=root;pwd=;";

    public MySqlConnection Conectar()
    {
        MySqlConnection conn = new MySqlConnection(conexao);
        conn.Open();
        return conn;
    }
}

