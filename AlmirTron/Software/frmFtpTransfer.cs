using AlmirTron.Background_Software;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlmirTron.Software
{
    public partial class frmAlmirTron_v3 : Form
    {
        public frmAlmirTron_v3()
        {
            InitializeComponent();
        }

        //Instancia SQL
        //SQL        
        private SqlCommand command2 = new SqlCommand();
        private SqlDataReader reader;

        //Message Box
        String mTittle;
        String mText;
        MessageBoxButtons mButton;
        MessageBoxIcon mIcon;
        DialogResult mResult;

        //Instância
        string servidor_Operante = null;
        string email_Global = null;
        //ID do servidor        
        List<string> lPedidos_FTP = new List<string>();
        List<string> lPedidos_Server = new List<string>();
        Queue<string> qPedidos_Importar = new Queue<string>();
        // Server_Bkp = Pasta para onde os retornos exportadas são salvas dentro da estrutura do visão
        List<string> lRetornos_Server_Bkp = new List<string>();
        List<string> lRetornos_Server = new List<string>();
        Queue<string> qRetornos_Exportar = new Queue<string>();
        List<string> lRetornos_Ajustes = new List<string>();
        // Server_Bkp = Pasta para onde os notas exportadas são salvas dentro da estrutura do visão
        List<string> lNotas_Server_Bkp = new List<string>();
        List<string> lNotas_Server = new List<string>();
        Queue<string> qNotas_Exportar = new Queue<string>();



        //Gerar log do Pedido
        public void gerar_LogPedido(string PedidoOL, string ano, string mes, string dia, string hora, string minuto, string Email)
        {
            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {
                try
                {
                    connectDMD.Open();
                    //Comando sql
                    command2 = new SqlCommand(" INSERT INTO [UNIDB].dbo.[ROBOT_LogOL]                                                                 "
                                             + " (Num_PedidoOL, Dat_Emissao_PedidoOL, Dat_Entrada_PedidoOL, Email_Notificacao)          "
                                             + " VALUES                                                                                               "
                                             + " ('" + PedidoOL + "', '" + ano + "-" + mes + "-" + dia + " " + hora + ":" + minuto + "', GETDATE(), '" + Email + "')  "
                                              , connectDMD);
                    command2.ExecuteNonQuery();
                }
                finally
                {
                    connectDMD.Close();

                }
            }
        }

        //Enviar e-mail de notificação de pedido
        protected void enviar_EmailAlerta_Pedido(string pedido, string caminho)
        {
            //string CNPJ = ler_CNPJ_Pedido(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\" + item);
            //string EAN = ler_Produto_Pedido(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\" + item);
            //int Qtd = ler_Qtd_Pedido()




            string Titulo = " NOVO PEDIDO OL |" + pedido + "|";
            string Pedido_OL = pedido;
            string CNPJ = ler_CNPJ_Pedido(caminho);
            string Cod_Cliente = null;
            string Cliente = null;
            List<int> Qtd_Itens = new List<int>();
            List<String> EAN_Itens = new List<String>();
            List<String> Produtos = new List<String>();
            ler_Produto_Pedido(caminho, EAN_Itens);
            ler_Qtd_Pedido(caminho, Qtd_Itens);

            int index = 0;
            foreach (var item in EAN_Itens)
            {
                
                using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {

                try
                {

                    connectDMD.Open();

                    command2 = connectDMD.CreateCommand();
                    command2.CommandText = " SELECT Codigo,Descricao from [DMD].dbo.[PRODU] "
                                           +" where Cod_EAN LIKE '"+item+"'";


                    reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["Codigo"] != null)
                        {
                                Produtos.Add(
                                     "<td style =  \"padding: 5px;  text-align: left; border: 1px solid black; \">"+ reader["Codigo"].ToString()+" </td>  "
                                    +"<td style = \"padding: 5px;  text-align: left; border: 1px solid black; \">"+ reader["Descricao"].ToString()+"</td>"
                                    +"<td style = \"padding: 5px;  text-align: left; border: 1px solid black; \">"+ item +"</td>"
                                    +"<td style = \"padding: 5px;  text-align: left; border: 1px solid black; \">"+ Qtd_Itens[index].ToString()+"</td>")
                                    ;             
                        }
                    }
                }
                finally
                {
                    connectDMD.Close();
                        index++;
                }
            }

            }



            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {

                try
                {

                    connectDMD.Open();

                    command2 = connectDMD.CreateCommand();
                    command2.CommandText = " SELECT Codigo, Razao_Social FROM [DMD].dbo.[CLIEN] "
                                          +" WHERE Cgc_Cpf LIKE '"+CNPJ+"'";


                    reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["Codigo"] != null)
                        {                            
                            Cod_Cliente = reader["Codigo"].ToString();
                            Cliente = reader["Razao_Social"].ToString();                            
                        }
                    }
                }
                finally
                {
                    connectDMD.Close();

                }
            }
            //Envia o email com o anexo
            using (MailMessage mm = new MailMessage("inteligence@unihospitalar.com.br", email_Global))
                {

                    try
                    {

                        StringBuilder CorpoEmail = new StringBuilder();
                        mm.CC.Add("ti@unihospitalar.com.br");
                        mm.Subject = Titulo;
                        // Define o corpo do E-mail --------------------------------------------------------------------------------

                        CorpoEmail.Append("<b> Pedido OL: </b> <p>" + Pedido_OL + "</p>");
                        CorpoEmail.Append("<b> CNPJ: </b> <p>" + CNPJ + "</p>");
                    CorpoEmail.Append("<b> Cod. Cliente: </b> <p>" + Cod_Cliente + "</p>");
                    CorpoEmail.Append("<b> Cliente: </b> <p>" + Cliente + "</p>");
                    CorpoEmail.Append("<b> Itens: </b>");
                    CorpoEmail.Append("<table style = \" border: 1px solid black; border-collapse: collapse; \">");
                        CorpoEmail.Append("<tr style = \" padding: 5px; text-align: left; border: 1px solid black; \">");
                        CorpoEmail.Append("<th style = \" padding: 5px; text-align: left; border: 1px solid black; \"> Cód.Produto </th>");
                        CorpoEmail.Append("<th style = \" padding: 5px; text-align: left; border: 1px solid black;\"> Produto    </th>");
                        CorpoEmail.Append("<th style = \" padding: 5px; text-align: left; border: 1px solid black;\"> Cód.EAN    </th>");
                        CorpoEmail.Append("<th style = \" padding: 5px; text-align: left; border: 1px solid black;\"> Qtd.Pedido </th>");
                        CorpoEmail.Append("</tr>");
                    
                        CorpoEmail.Append("<tr>");

                        foreach (var item in Produtos)
                            CorpoEmail.Append(@item);

                        CorpoEmail.Append("</tr>");

                        CorpoEmail.Append("</table>");

                        // Insere a imagem superior centralizada ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨                        
                        CorpoEmail.Append("<p>Prezado(a),</p>");

                        CorpoEmail.Append("<b>Agradecemos pela sua parceria.</b><br>");
                        CorpoEmail.Append("<b>Atenciosamente,</b><br>");

                        CorpoEmail.Append("<img src=\"https://i.ibb.co/fD2PfKS/Assina-Email-Inteligence.png\" />"); //assinatura                                                                        


                        // Insere uma mensagem padrão ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨                        
                        CorpoEmail.Append("<b>E-mail gerado automaticamente pelo sistema UHC 2. Favor não responder!</b>");

                        mm.IsBodyHtml = true;

                        mm.BodyEncoding = Encoding.GetEncoding("ISO-8859-1"); // <-- Define o Encoding para aceitar caracteres especiais
                        mm.Body = CorpoEmail.ToString();
                        //Add Byte array as Attachment.
                        //mm.Attachments.Add(new Attachment(new MemoryStream(bytes), NomeArquivo));


                        //Configuracao do SMTP
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                        credentials.UserName = "inteligence@unihospitalar.com.br";
                        credentials.Password = "!@#asd253";



                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credentials;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }


                    finally
                    {

                    }
                }
            }
        
    

        //Upload arquivo ftp
        private void Upload_FTP_Ache(string local, string arquivo, string destino)
        {
            var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create("ftp://ftp.visaogrupo.com.br/" + destino + "/" + arquivo);
            request.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new System.Net.NetworkCredential(Parametros_Empresa.login_FTP_Ache, Parametros_Empresa.senha_FTP_Ache);


            using (var stream = new System.IO.StreamReader(@"\\" + servidor_Operante + @"\c$\Allypharma\PEDIDO\TUDOFARMA\" + local + @"\" + arquivo))
            {
                var conteudoArquivo = Encoding.UTF8.GetBytes(stream.ReadToEnd());
                request.ContentLength = conteudoArquivo.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(conteudoArquivo, 0, conteudoArquivo.Length);
                requestStream.Close();
            }

            var response = (System.Net.FtpWebResponse)request.GetResponse();
            response.Close();
        }


        //O AlmirTron possui a finalidade de TRANSPORTE de PEDIDOS, RETORNOS E NFs, além do REPARO dos retornos. 
        //Qualquer outra finalidade aquém desta citada é fora do escopo da ferramenta, então não é recomendado a manipulação do AlmirTron para novas funções, e sim a criação de uma aplicação paralela.

        //Pedidos OL da Aché
        //Etapas técnicas
        //1 - Recepção dos Pedidos
        //1.1 - Consulta o FTP da Aché                 (Adress: ftp.visaogrupo.com.br | Folder: Pedido)
        //1.2 - Compara com a pasta /PEDIDO/TUDOFARMA/Pedidos
        //1.3 - Baixar os arquivos que não pertecem pertecem a unidade acima para a pasta, afim de serem processados


        //Função genérica de leitura dos arquivos do FTP
        private void ler_ArquivosFTP(String server, String path, String user, String pass, List<String> list)
            {
                FtpWebRequest fwrr = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + server + path));
                fwrr.Credentials = new NetworkCredential(user, pass);
                fwrr.Method = WebRequestMethods.Ftp.ListDirectory;
                fwrr.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                StreamReader srr = new StreamReader(fwrr.GetResponse().GetResponseStream());
                string str = srr.ReadLine();
                List<string> strList = new List<string>();

                while (str != null)
                {
                    if (!list.Contains(str))
                        list.Add(str);
                    str = srr.ReadLine();
                }
            }

        //Função genérica de leitura dos arquivos do Servidor
        private void ler_ArquivosServidor(String path, String directory, List<String> list)
            {
                DirectoryInfo dir = new DirectoryInfo(path + directory);

                foreach (FileInfo file in dir.GetFiles())
                {
                    if (!list.Contains(file.Name))
                        list.Add(file.Name);
                }


            }
    
      

        //Função que lê o arquivo de retorno para identificAr o CNPJ
        private String ler_CNPJ_Pedido(String arquivo)
        {
            String CNPJ = null;
            
                using (StreamReader FluxoTexto = new StreamReader(arquivo))
                {
                    while (true)
                    {
                        string LinhaTexto = FluxoTexto.ReadLine();
                        if (LinhaTexto == null)
                        {
                            break;
                        }
                        try
                        {
                            if (LinhaTexto.Substring(0, 1).Equals("1"))
                            {
                                CNPJ = LinhaTexto.Substring(2, 14);
                                break;
                            }
                        }
                        finally
                        {

                        }
                    }
                }
            return CNPJ;
        }

        private void ler_Produto_Pedido(String arquivo, List<String>Produto)
        {
            //Ler produto            
                using (StreamReader FluxoTexto = new StreamReader(arquivo))
                {
                    while (true)
                    {
                        string LinhaTexto = FluxoTexto.ReadLine();
                        if (LinhaTexto == null)
                        {
                            break;
                        }
                        try
                        {
                            if (LinhaTexto.Substring(0, 1).Equals("2"))
                            {
                                Produto.Add(LinhaTexto.Substring(13, 13));                                
                            }
                        }
                        finally
                        {

                        }
                    }
                }            
        }


        private void ler_Qtd_Pedido(String arquivo, List<int> Qtd)
        {            
            using (StreamReader FluxoTexto = new StreamReader(arquivo))
            {
                while (true)
                {
                    string LinhaTexto = FluxoTexto.ReadLine();
                    if (LinhaTexto == null)
                    {
                        break;
                    }
                    try
                    {
                        if (LinhaTexto.Substring(0, 1).Equals("2"))
                        {
                            Qtd.Add(Convert.ToInt32(LinhaTexto.Substring(26,5)));
                        }
                    }
                    finally
                    {

                    }
                }
            }
            
        }



        //Etapas do Pedido
        //1 - Ler os pedidos existentes no FTP 
        //Irá ler os pedidos no FTP e alimentar uma lista com o nome
        //2 - Ler os pedidos existes no servidor
        //Irá ler os pedidos no Servidor e alimentar uma lista com o nome
        //3 - Comparar os pedidos
        //4 - Baixar diferença 
        //5 - Listar LOGs





        //Etapa para inserção do Pedido
        //1- Ler os pedidos existentes no FTP            
        //Método que executa a leitura dos pedidos no servidor FTP e trás a nomenclatura para uma lista dentro do Software. A lista é carregada inicialmente ao início do Software e ao decorrer do tempo funciona de forma incremental.
        private void ler_Pedido_FTP()
        {
             ler_ArquivosFTP("ftp.visaogrupo.com.br/", "Pedido", Parametros_Empresa.login_FTP_Ache, Parametros_Empresa.senha_FTP_Ache, lPedidos_FTP);
        }

        //2 - Ler os pedidos existes no servidor
        private void ler_Pedido_Server()
        {
            ler_ArquivosServidor(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\", "Pedido", lPedidos_Server);
        }
    
        //3 - Comparar os pedidos
        private void comparar_Pedidos()
            {

                foreach (var item in lPedidos_FTP)
                {
                    //MessageBox.Show(item.Replace("Pedido/","").ToString().Replace(".txt","._RM"));
                    if (!lPedidos_Server.Contains(item.Replace("Pedido/", "").Replace(".txt", "._RM")) && !lPedidos_Server.Contains(item.Replace("Pedido/", "")))
                    {
                        lPedidos_Server.Add(item);
                        qPedidos_Importar.Enqueue(item);
                        //lsbLogList.Invoke(new MethodInvoker(delegate { lsbLogList.Items.Add(item); }));
                        

                        lsbLogList.Invoke(new MethodInvoker(delegate
                        {
                            lsbLogList.Items.Add("Aché - PEDIDO - " + item.Substring(44).Replace(".txt", "") + " - [  " + item.Substring(35, 2).Replace(".txt", "") + "/" + item.Substring(33, 2).Replace(".txt", "") + "/" + item.Substring(29, 4).Replace(".txt", "") + " | " + item.Substring(37, 2).Replace(".txt", "") + ":" + item.Substring(39, 2).Replace(".txt", "") + ":" + item.Substring(41, 2).Replace(".txt", "") + " ]");

                            //vendas@unihospitalar.com.br
                            gerar_LogPedido(Convert.ToInt32(item.Substring(44).Replace(".txt", "")).ToString(), item.Substring(29, 4).Replace(".txt", ""), item.Substring(33, 2).Replace(".txt", ""), item.Substring(35, 2).Replace(".txt", ""), item.Substring(37, 2).Replace(".txt", ""), item.Substring(39, 2).Replace(".txt", ""), email_Global);
                           

                            //enviar_EmailAlerta_Pedido(Convert.ToInt32(item.Substring(44).Replace(".txt", "")).ToString(), @"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\" + item);
                        }));

                    }
                }
            }

        //4 - Baixar diferença 
        private void baixar_Pedidos()
        {
            while (qPedidos_Importar.Count > 0)
            {
                baixar_ArquivoFTP("ftp://ftp.visaogrupo.com.br/" + qPedidos_Importar.Peek().ToString(), @"\\" + servidor_Operante + @"\c$\Allypharma\PEDIDO\TUDOFARMA\" + qPedidos_Importar.Peek().ToString(), Parametros_Empresa.login_FTP_Ache, Parametros_Empresa.senha_FTP_Ache);
                enviar_EmailAlerta_Pedido(Convert.ToInt32(qPedidos_Importar.Peek().ToString().Substring(44).Replace(".txt", "")).ToString(), @"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\" + qPedidos_Importar.Peek().ToString());
                qPedidos_Importar.Dequeue();
            }

        }
            
        //Rotina de importar o pedido
        private void rotina_Pedido()
        {
            ler_Pedido_FTP();
            ler_Pedido_Server();
            comparar_Pedidos();
            baixar_Pedidos();
        }


        //Retorno
        //Analisar possíveis erros de retorno
        private String analisar_Retorno(String path, String file_Name)
            {
                String path_file = path + file_Name;
                String Error = "";                
                using (StreamReader FluxoTexto = new StreamReader(path_file))
                {
                    while (true)
                    {
                        string LinhaTexto = FluxoTexto.ReadLine();
                        if (LinhaTexto == null)
                        {
                            break;
                        }
                        try
                        {
                            if (LinhaTexto.Substring(0, 1).Equals("2"))
                            {
                                if (LinhaTexto.Substring(45, 2).Equals("13") || LinhaTexto.Substring(45, 2).Equals("14"))
                                {
                                    Error = "13 Aprovado";
                                }
                                else
                                {
                                    if (LinhaTexto.Substring(45, 2).Equals("03"))
                                    {
                                        Error = "03 Cliente Bloqueado";
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 1).Equals("3"))
                                    {
                                        Error = "3 Numero de caracteres errado, ausência de 0";
                                    }
                                else if (LinhaTexto.Substring(45, 2).Equals("04"))
                                    {
                                        Error = ("04 Produto não cadastrado/Desativado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("05"))
                                    {
                                        Error = ("05 Falta de limite de Crédito");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("06"))
                                    {
                                        Error = ("06 Falta no estoque");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("07"))
                                    {
                                        Error = ("07 Produto Bloqueado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("08"))
                                    {
                                        Error = ("08 Pedido criado e cliente sem negociação");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("09"))
                                    {
                                        Error = ("09 Horário da venda inter já encerrado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("10"))
                                    {
                                        Error = ("10 Pedido faturado com condição diferente");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("11"))
                                    {
                                        Error = ("11 Pedido duplicado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("12"))
                                    {
                                        Error = ("12 Venda interestadual não autorizada");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("15"))
                                    {
                                        Error = ("15 Produto alocado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("21"))
                                    {
                                        Error = ("21 Código do cliente inválido");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("22"))
                                    {
                                        Error = ("22 Cliente com CEP inválido");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("23"))
                                    {
                                        Error = ("23 Cliente com rota/roteiro inválido");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("24"))
                                    {
                                        Error = ("24 Cliente com setor inválido");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("51"))
                                    {
                                        Error = ("51 Pedido não processado por horário ultrapassado");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("52"))
                                    {
                                        Error = ("52 Pedido não alcançou o valor mínimo");
                                        break;
                                    }
                                    else if (LinhaTexto.Substring(45, 2).Equals("53"))
                                    {
                                        Error = ("53 Pedido com layout incorreto");
                                        break;
                                    }
                                }
                            }


                        }
                        catch (IOException ioExcept)
                        {
                            throw ioExcept;
                        }

                    }
                }
                return Error;
            }

        //1 - Ler retornos do  FTP
        private void ler_Retorno_Server()
            {
                ler_ArquivosServidor(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\", "Retorno", lRetornos_Server);
            }

        //2 - Ler retornos do  Server
        private void ler_Retorno_Server_Bkp()
            {
                ler_ArquivosServidor(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\Retorno\", "BKP_Retorno", lRetornos_Server_Bkp);
            }

        //3 - Comparar retornos
        private void comparar_Retornos()
            {
                foreach (var item in lRetornos_Server)
                {
                if (!lRetornos_Server_Bkp.Contains(item))
                    {
                        if (analisar_Retorno(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\", item).Contains("13"))
                        {
                        using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
                        {
                            try
                            {
                                connectDMD.Open();
                                //Comando sql
                                command2 = new SqlCommand(" UPDATE[UNIDB].dbo.[ROBOT_LogOL] " 
                                                          +" SET Dat_Retorno_PedidoOL = GETDATE() "
                                                          +" WHERE Num_PedidoOL like '" + Convert.ToInt32(item.Substring(37).Replace(".txt", "")) + "'"
                                                          , connectDMD); ;
                                command2.ExecuteNonQuery();
                            }
                            finally
                            {
                                connectDMD.Close();

                            }
                        }
                        qRetornos_Exportar.Enqueue(item);
                        }
                        else
                        {
                            if (!lRetornos_Ajustes.Contains(item))
                            {
                            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
                            {
                                try
                                {
                                    connectDMD.Open();
                                    //Comando sql
                                    command2 = new SqlCommand("  UPDATE [UNIDB].dbo.[ROBOT_LogOL] " 
                                                              +" SET  Dat_Retorno_PedidoOL = GETDATE() " 
                                                              +"     ,Observacao_Retorno_PedidoOL = '" + analisar_Retorno(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\", item) + "'" 
                                                              +" WHERE Num_PedidoOL like '"+Convert.ToInt32(item.Substring(37).Replace(".txt", "")) + "'"
                                                              , connectDMD);
                                    command2.ExecuteNonQuery();
                                }
                                finally
                                {
                                    connectDMD.Close();

                                }
                            }
                            lRetornos_Ajustes.Add(item);
                            lsbErrorList.Invoke(new MethodInvoker(delegate { lsbErrorList.Items.Add("Aché - RETORNO - " + (item.Substring(37).Replace(".txt", "")) + " " + analisar_Retorno(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\", item)); }));
                            }
                        }
                    }
                }
            }

        //4 - Enviar retornos
        private void exportar_Retorno()
            {

                lRetornos_Server_Bkp.Clear();
                lRetornos_Server.Clear();
                while (qPedidos_Importar.Count == 0 && qNotas_Exportar.Count == 0 && qRetornos_Exportar.Count > 0)
                {
                    lsbLogList.Invoke(new MethodInvoker(delegate { lsbLogList.Items.Add("Aché - RETORNO - " + (qRetornos_Exportar.Peek().Substring(37).Replace(".txt", ""))); }));                
                
                //Transporte entre as pastas


                Upload_FTP_Ache("RETORNO", qRetornos_Exportar.Peek(), "RETORNO");
                File.Move(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\" + qRetornos_Exportar.Peek()
                             , @"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\Bkp_RETORNO\" + qRetornos_Exportar.Peek());
                
                    qRetornos_Exportar.Dequeue();
                }
            }

        //Rotina retorno
        private void rotina_Retorno()
            {
                ler_Retorno_Server();
                ler_Retorno_Server_Bkp();
                comparar_Retornos();
                exportar_Retorno();
            }



        //Notas
        //1 - Ler Nota do  FTP
        private void ler_Notas_Server()
        {
            ler_ArquivosServidor(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\", "Nota", lNotas_Server);
        }

        //2 - Ler Nota do  Server
        private void ler_Notas_Server_Bkp()
        {
            ler_ArquivosServidor(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\Nota\", "BKP_Nota", lNotas_Server_Bkp);
        }

        //3 - Comparar Nota
        private void comparar_Nota()
        {
            foreach (var item in lNotas_Server)
            {
                if (!lNotas_Server_Bkp.Contains(item))
                {                    
                        qNotas_Exportar.Enqueue(item);                                        
                }
            }
        }

        //4 - Enviar Nota
        private void exportar_Notas()
        {

            lNotas_Server_Bkp.Clear();
            lNotas_Server.Clear();
            while (qPedidos_Importar.Count == 0 && qRetornos_Exportar.Count ==0 && qNotas_Exportar.Count > 0)
            {
                lsbLogList.Invoke(new MethodInvoker(delegate { lsbLogList.Items.Add("Aché - NOTA - " + identificar_Pedido_Nota(qNotas_Exportar.Peek()).ToString()); }));
                using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
                {
                    try
                    {
                        connectDMD.Open();
                        //Comando sql
                        command2 = new SqlCommand( " UPDATE [UNIDB].dbo.[ROBOT_LogOL]                                                                                  "
                                                  +" SET NF_Pedido_OL = (SELECT Num_Nota FROM[DMD].dbo.[PDECB] ECB                                                     "
                                                  +"                    JOIN[DMD].dbo.[PDVCB] PCB ON PCB.Numero = ECB.Num_PedVen                                       "
                                                  +"                    JOIN[DMD].dbo.[NFSCB] NFS ON NFS.Cod_Pedido = PCB.Numero                                       "
                                                  +"                    WHERE ECB.Cod_PedCli LIKE '"+ identificar_Pedido_Nota(qNotas_Exportar.Peek()).ToString() + "') "
                                                  +" ,Dat_Emissao_NF = (SELECT Dat_Emissao FROM[DMD].dbo.[PDECB] ECB                                                   "
                                                  +"                   JOIN[DMD].dbo.[PDVCB] PCB ON PCB.Numero = ECB.Num_PedVen                                        "
                                                  +"                    JOIN[DMD].dbo.[NFSCB] NFS ON NFS.Cod_Pedido = PCB.Numero                                       " 
                                                  +"                    WHERE ECB.Cod_PedCli LIKE '"+ identificar_Pedido_Nota(qNotas_Exportar.Peek()).ToString() + "') "
                                                  + " WHERE Num_PedidoOL LIKE ('" +identificar_Pedido_Nota(qNotas_Exportar.Peek()).ToString() + "')"
                                                  , connectDMD); ;
                        command2.ExecuteNonQuery();
                    }
                    finally
                    {
                        connectDMD.Close();

                    }
                }
                //Transporte entre pastas
                Upload_FTP_Ache("NOTA", qNotas_Exportar.Peek(), "NOTA");
                File.Move(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\NOTA\" + qNotas_Exportar.Peek()
                         , @"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\NOTA\Bkp_NOTA\" + qNotas_Exportar.Peek());                
                qNotas_Exportar.Dequeue();
            }
        }

        //Rotina Nota
        private void rotina_Nota()
        {
            ler_Notas_Server();
            ler_Notas_Server_Bkp();
            comparar_Nota();
            exportar_Notas();
        }


        //Baixar arquivos do FTP
        void baixar_ArquivoFTP(string url, string local, string usuario, string senha)
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(usuario, senha);
                    request.UseBinary = true;

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        using (Stream rs = response.GetResponseStream())
                        {
                            using (FileStream ws = new FileStream(local, FileMode.Create))
                            {
                                byte[] buffer = new byte[2048];
                                int bytesRead = rs.Read(buffer, 0, buffer.Length);

                                while (bytesRead > 0)
                                {
                                    ws.Write(buffer, 0, bytesRead);
                                    bytesRead = rs.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }
                catch
                {

                }

            }
           
        //Load do Form     
        private void frmAlmirTron_v3_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.Robot;
            configuracoes_Iniciais();
            btnWorkingStopped_Click(btnWorkingStopped, new EventArgs());
        }

        private void configuracoes_Iniciais()
        {
            //Configura o botão de start inicial
            btnWorkingStopped.Text = "Stopped";
            btnWorkingStopped.BackColor = Color.Red;
            using (SqlConnection connectDMD = ConnectionDB_PE.getInstancia().getConnection())
            {
                try
                {
                    connectDMD.Open();
                    command2 = connectDMD.CreateCommand();
                    command2.CommandText = " SELECT Adress_Servidor,Email_Notificacao FROM [UNIDB].dbo.[BOT_CONFIGURACAO] ";

                    reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["Adress_Servidor"] != null)
                        {
                            servidor_Operante = reader["Adress_Servidor"].ToString();
                            email_Global = reader["Email_Notificacao"].ToString();
                        }
                    }
                }
                finally
                {
                    connectDMD.Close();
                }
            }
        }


        //Limpar filas e listas
        private void limpar_Estruturas()
        {
            lPedidos_FTP.Clear();
            lPedidos_Server.Clear();
            qPedidos_Importar.Clear();            
            lRetornos_Server_Bkp.Clear();
            lRetornos_Server.Clear();
            qRetornos_Exportar.Clear();
            lRetornos_Ajustes.Clear();            
            lNotas_Server_Bkp.Clear();
            lNotas_Server.Clear();
            qNotas_Exportar.Clear();
        }

        //Iniciar o processo
        private void btnWorkingStopped_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (btnWorkingStopped.Text == "Stopped")
                {
                    btnWorkingStopped.BackColor = Color.Green;
                    btnWorkingStopped.Text = "Working...";
                    timerPedido.Interval = 10000;
                    timerPedido.Start();
                    timerRetorno.Interval = 20000;
                    timerRetorno.Start();
                    timerNota.Interval = 30000;
                    timerNota.Start();                        
                    timerWorking.Start();
                }
            else{
                btnWorkingStopped.BackColor = Color.Red;
                btnWorkingStopped.Text = "Stopped";
                timerPedido.Stop();
                timerRetorno.Stop();
                timerNota.Stop();
                timerWorking.Stop();
                limpar_Estruturas();
            }
        
            this.Cursor = Cursors.Default;
        }

        private void timerPedido_Tick(object sender, EventArgs e)
            {
            timerPedido.Interval = 10000;
            Thread tPedido = new Thread(rotina_Pedido);
                while (!tPedido.IsAlive)
                    tPedido.Start();
            }

        private void timerRetorno_Tick(object sender, EventArgs e)
        {
        timerRetorno.Interval = 20000;
        Thread tRetorno = new Thread(rotina_Retorno);
            while (!tRetorno.IsAlive)
                tRetorno.Start();
        }

        private void timerNota_Tick(object sender, EventArgs e)
        {
            timerNota.Interval = 30000;
            Thread tNota = new Thread(rotina_Nota);
            while (!tNota.IsAlive)
                tNota.Start();
        }
        private void lsbErrorList_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

        private void btnSolveErrors_Click(object sender, EventArgs e)
            {
            
                if (lsbErrorList.SelectedItem != null)
                {
                btnWorkingStopped_Click(btnWorkingStopped, new EventArgs());
                mTittle = "Ajustar Retorno";
                    mText = "Deseja ajustar o retorno " + Convert.ToInt32(lRetornos_Ajustes[lsbErrorList.SelectedIndex].ToString().ToString().Substring(37).Replace(".txt", "")).ToString();
                    mIcon = MessageBoxIcon.Question;
                    mButton = MessageBoxButtons.YesNo;

                    mResult = MessageBox.Show(mText, mTittle, mButton, mIcon);
                    if (mResult == DialogResult.Yes)
                    {

                        List<String> Texto = new List<String>();
                        String path = lRetornos_Ajustes[lsbErrorList.SelectedIndex];
                        //Ajustar retorno
                        using (StreamReader FluxoTexto = new StreamReader(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\" + path))
                        {

                            while (true)
                            {
                                string LinhaTexto = FluxoTexto.ReadLine();
                                if (LinhaTexto == null)
                                {
                                    break;
                                }

                                try
                                {
                                    if (!LinhaTexto.Substring(0, 1).Equals("2"))
                                    {
                                        Texto.Add(LinhaTexto);
                                    }
                                    else
                                        Texto.Add(LinhaTexto.Substring(0, 45) + "13PRODUTO FATURADO");


                                }
                                catch
                                {
                                    //throw;
                                }

                            }
                        }

                        // Set a variable to the Documents path.
                        string docPath =
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        // Write the string array to a new file named "WriteLines.txt".
                        using (StreamWriter outputFile = new StreamWriter(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\RETORNO\" + path))
                        {
                            try
                            {
                                foreach (string line in Texto)
                                    outputFile.WriteLine(line);
                            }
                            catch
                            {
                                //throw;
                            }
                            finally
                            {
                                outputFile.Close();
                                mTittle = "Ajustar Retorno";
                                mText = "O retorno do arquivo " + Convert.ToInt32(lRetornos_Ajustes[lsbErrorList.SelectedIndex].ToString().ToString().Substring(37).Replace(".txt", "")).ToString() + ", foi ajustado com sucesso!";
                                mIcon = MessageBoxIcon.Information;
                                mButton = MessageBoxButtons.OK;

                                int index_Delete = lsbErrorList.SelectedIndex;
                                lsbErrorList.Items.RemoveAt(index_Delete);
                                lRetornos_Ajustes.RemoveAt(index_Delete);
                                MessageBox.Show(mText, mTittle, mButton, mIcon);
                            btnWorkingStopped_Click(btnWorkingStopped, new EventArgs());
                        }
                        }
                    }
                }

            }

        private int identificar_Pedido_Nota(string path)
        {                            
                    List<String> Texto = new List<String>();
                    int Pedido = 0;                    
                    //Ajustar retorno
                    using (StreamReader FluxoTexto = new StreamReader(@"\\" + servidor_Operante + @"\c$\AllyPharma\PEDIDO\TUDOFARMA\NOTA\" + path))
                    {

                        while (true)
                        {
                            string LinhaTexto = FluxoTexto.ReadLine();
                            if (LinhaTexto == null)
                            {
                                break;
                            }

                            try
                            {
                                if (LinhaTexto.Substring(0, 1).Equals("2"))
                                {
                                    Pedido =  Convert.ToInt32(LinhaTexto.Substring(38,10));
                                }                                
                            }
                            catch
                            {
                                //throw;
                            }

                        }
                    }
                                                            
            return Pedido;
        }


        private void btnWorking_ExecucaoSync()
            {
                btnWorkingStopped.Invoke(new MethodInvoker(delegate
                {
                    if (btnWorkingStopped.Text == "Working...")
                        btnWorkingStopped.Text = "Working..";
                    else if (btnWorkingStopped.Text == "Working..")
                        btnWorkingStopped.Text = "Working.";
                    else if (btnWorkingStopped.Text == "Working.")
                        btnWorkingStopped.Text = "Working";
                    else if (btnWorkingStopped.Text == "Working")
                        btnWorkingStopped.Text = "Working...";
                }));

            }

        private void timerWorking_Tick(object sender, EventArgs e)
            {

                timerWorking.Interval = 2000;
                Thread tWorking = new Thread(btnWorking_ExecucaoSync);
                while (!tWorking.IsAlive)
                    tWorking.Start();






            }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            frmOpcoes form = new frmOpcoes();
            form.ShowDialog();
            configuracoes_Iniciais();

        }
    }
}


