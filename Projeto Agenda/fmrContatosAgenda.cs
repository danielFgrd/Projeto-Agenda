using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ativando bibliotecas para trabalhar com banco de dados Acces
using System.Data.OleDb;
using System.Data.SqlClient;


namespace Projeto_Agenda
{
    public partial class fmrContatosAgenda : Form
    {
        static string strCn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\danie\\Documents\\DBAgendaDeContatos.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection conexao = new SqlConnection(strCn);//objeto da conexão  com o banco
        //OleDbConnection conexao = new OleDbConnection(strCn);


        public fmrContatosAgenda()
        {
            InitializeComponent();
        }

        private void fmrContatosAgenda_Load(object sender, EventArgs e)
        {

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            //instrução sql responsável por pesquisar o banco de dados (CRUD - Read)
            string pesquisa = "select * from tbContatos where idContatos = " + txbId.Text;
            SqlCommand cmd = new SqlCommand(pesquisa, conexao); //objeto responsável pelos comandos(querys) no sql.
            //criando objeto de nome cmd tendo como modelo a classe OleDbCommand para execultar  a instução  sql
            // OleDbCommand cmd = new OleDbCommand(pesquisa, conexao);
            // Atravé da classe OleDbDataReader que faz parte do SqlCliente, criamos uma //variável chamada DR que será usada na leitura dos dados (instrução select)

            SqlDataReader DR;
            //OleDbDataReader DR;
            //tratamento de exeções: try - catch - finally (em caso de erro de erro capturamos o erro)


            try
            {
                //abrindo conexão com o bando de dados
                conexao.Open();
                cmd.ExecuteNonQuery();//Executa o cmd e pega todos os dados que buscamos com o nosso comando

                DataSet ds = new DataSet();//setar dados
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;//adapta o cmd ao projeto
                da.Fill(ds);// preenche todas as informações dentro do DataSet
                //executando a instrução e armazenando o resultado do reader DR
                DR = cmd.ExecuteReader();
                if (DR.Read())
                {
                    txbId.Text = DR.GetValue(0).ToString();
                    txbNome.Text = DR.GetValue(1).ToString();
                    txbTelefone.Text = DR.GetValue(2).ToString();
                    txbEmail.Text = DR.GetValue(3).ToString();
                }
                //Exibe as Informações nas caisas de texto(textBox) correspondente (#)
                //nova pesquisa
                else
                {
                    MessageBox.Show("Registro não encontrado");
                    txbNome.Text = "";
                    txbTelefone.Text = "";
                    txbEmail.Text = "";
                    txbId.Focus();
                }
                //encerrando o uso do Reader DR
                DR.Close();
                //encerra o cmd
                cmd.Dispose();

            }
            catch (Exception ex)
            {
                //exibe o erro
                MessageBox.Show(ex.Message);
            }
            // de qualquer forma sempre fechar a conexão com o banco ("lembrar da porta da //geladeira rsrsrs") 
            finally
            {
                conexao.Close();
            }

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            string adiciona = "INSERT INTO tbContatos(conNome, conTelefone, conEmail) values ('" + txbNome.Text + "','" + txbTelefone + "','" + txbEmail + "')"; //query de insert
            SqlCommand cmd = new SqlCommand(adiciona, conexao);
            SqlDataReader DR;

            try
            {
                //abrindo conexão
                conexao.Open();
               
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();

      //          cmd.Parameters.Add("idContatos", SqlDbType.Int).Value = txtId.Text;
                cmd.Parameters.Add("conNome", SqlDbType.NVarChar).Value = txtNome.Text;
                cmd.Parameters.Add("conTelefone", SqlDbType.NVarChar).Value = txtTelefone.Text;
                cmd.Parameters.Add("conEmail", SqlDbType.NVarChar).Value = txtEmail.Text;
                da.InsertCommand = cmd;
                cmd.ExecuteNonQuery();// execulta todos os comandos que foram estabelecidos pela query
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
            finally
            {
                cmd.Dispose();
                conexao.Close();
            }
            

        }
    }
}