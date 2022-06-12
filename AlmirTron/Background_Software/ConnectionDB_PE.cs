using System.Configuration;
using System.Data.SqlClient;

namespace AlmirTron.Background_Software
{
    class ConnectionDB_PE
    {
        private static readonly ConnectionDB_PE iSQL = new ConnectionDB_PE();

        public static ConnectionDB_PE getInstancia()
        {
            return iSQL;
        }

        public SqlConnection getConnection()
        {
            string conn = ConfigurationManager.ConnectionStrings["SQLConnectionStringPE"].ToString();
            return new SqlConnection(conn);
        }
    }
}
