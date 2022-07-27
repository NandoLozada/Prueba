using IntegracionWebAPI.Entidades;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.DAOs
{
    public class UsuariosDAO
    {
        private readonly ConexionDB conexionDB;

        public UsuariosDAO(ConexionDB conexionDB)
        {
            this.conexionDB = conexionDB;
        }

        //public List<Cliente> ListaClientesDAO()
        //{
        //    var queryListaClientes = "SELECT * FROM Clientes";

        //    using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
                
        //    {
        //       var listaClientes =  conexion.Query<Cliente>(queryListaClientes).ToList();
        //       return listaClientes.ToList();
        //    }
        //}

        //public List<Cliente> ClientePorDNI(int DNI)
        //{
        //    var clientedni = "SELECT * FROM Clientes WHERE DNI =" + DNI ;

        //    using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

        //    {
        //        var cliente = conexion.Query<Cliente>(clientedni).ToList();

        //        return cliente;
        //    }
        //}

        //public void AgregarCliente (int DNI, string nombre)
        //{
        //    var insertcliente = "INSERT INTO Clientes (NomApe, DNI, IdEstado) VALUES (@nombreq, @dniq, @estadoq)";

        //    using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

        //    {
        //        conexion.Execute(insertcliente, new {nombreq = nombre, dniq = DNI, estadoq = 1 });
        //    }
        //}

        //public void EstadoCliente(int estado, int id)
        //{
        //    var updatecliente = "UPDATE Clientes SET IdEstado = @estadoq WHERE Id = @idq";

        //    using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

        //    {
        //        conexion.Execute(updatecliente, new { estadoq = estado, idq = id });
        //    }
        //}
    }
}
