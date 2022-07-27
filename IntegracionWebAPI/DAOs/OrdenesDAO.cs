using IntegracionWebAPI.Entidades;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.DAOs
{
    public class OrdenesDAO
    {
        private readonly ConexionDB conexionDB;

        public OrdenesDAO(ConexionDB conexionDB)
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

        public int AgregarOrden(int idcliente)
        {
            var insertorden = "INSERT INTO Ordenes (IdCliente) VALUES (@idcliente)";
            var ultid = "SELECT MAX(Id) FROM ORDENES";
            var orden = 0;

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                conexion.Execute(insertorden, new { idcliente = idcliente});
                orden = conexion.QuerySingle<int>(ultid);
            }

            return orden;
        }

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
