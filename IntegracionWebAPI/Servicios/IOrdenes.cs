using Dapper;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.Servicios
{
    public interface IOrdenes
    {
    }

    public class Ordenes : IOrdenes
    {
        public class ServOrdenes : IOrdenes
        {
            //public List<Cliente> ListaClientes(ClientesDAO DAO)
            //{
            //    return DAO.ListaClientesDAO();
            //}

            //public List<Cliente> ClientePorDNI(ClientesDAO DAO,int DNI)
            //{
            //    return DAO.ClientePorDNI(DNI);
            //}

            public int AgregarOrden(OrdenesDAO DAO, int idcliente)
            {
                return DAO.AgregarOrden(idcliente);
            }

            //public void EstadoCliente(ClientesDAO DAO, int estado, int id)
            //{
            //    DAO.EstadoCliente(estado, id);
            //}

        }


    }
}
