using Dapper;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.Servicios
{
    public interface IUsuarios
    {
    }

    public class Usuarios : IUsuarios
    {
        public class ServUsuarios : IUsuarios
        {
            //public List<Cliente> ListaClientes(ClientesDAO DAO)
            //{
            //    return DAO.ListaClientesDAO();
            //}

            //public List<Cliente> ClientePorDNI(ClientesDAO DAO,int DNI)
            //{
            //    return DAO.ClientePorDNI(DNI);
            //}

            //public void AgregarCliente(ClientesDAO DAO, int DNI, string nombre)
            //{
            //    DAO.AgregarCliente(DNI, nombre);
            //}

            //public void EstadoCliente(ClientesDAO DAO, int estado, int id)
            //{
            //    DAO.EstadoCliente(estado, id);
            //}

        }


    }
}
