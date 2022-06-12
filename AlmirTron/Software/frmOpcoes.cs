using System;
using AlmirTron.Background_Software;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlmirTron.Software
{
    public partial class frmOpcoes : Form
    {
        public frmOpcoes()
        {
            InitializeComponent();
        }


        //Instancia SQL
        //SQL        
        private SqlCommand command2 = new SqlCommand();
        private SqlDataReader reader;
        private void configuracoes_Iniciais()
        {
            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {
                try
                {
                    connectDMD.Open();
                    command2 = connectDMD.CreateCommand();
                    command2.CommandText = " select Adress_Servidor,Email_Notificacao from [UNIDB].dbo.[BOT_CONFIGURACAO] ";


                    reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["Adress_Servidor"] != null)
                        {
                            txtEnderecoServidor.Text = reader["Adress_Servidor"].ToString();
                            txtEmail.Text = reader["Email_Notificacao"].ToString();
                        }
                    }
                }
                finally
                {
                    connectDMD.Close();
                    
                }
           
            }
        }
       
        private void frmOpcoes_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.Robot;
            configuracoes_Iniciais();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {
                try
                {
                    connectDMD.Open();
                    //Comando sql
                    command2 = new SqlCommand(  "  UPDATE [UNIDB].dbo.[BOT_CONFIGURACAO] "
                                              + "  SET  Adress_Servidor =  '"+txtEnderecoServidor.Text+"'" 
                                              + ", Email_Notificacao = '"+txtEmail.Text+"'"                                                                                            
                                              ,connectDMD);
                    command2.ExecuteNonQuery();
                }
                finally
                {
                    connectDMD.Close();
                    this.Close();
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
