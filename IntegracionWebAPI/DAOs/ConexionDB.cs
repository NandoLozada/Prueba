using System.Data.SqlClient;

namespace IntegracionWebAPI.DAOs
{
    public class ConexionDB
    {
        public string StringConexion()
        {
            return "Data Source=.;Database=IntegracionWebAPI;Integrated Security=true";
        }
    }

}
