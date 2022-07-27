using Dapper;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.Servicios
{
    public interface INotas
    {
    }

    public class Notas : INotas
    {
        public class ServNotas : INotas
        {
            public List<Nota> ListaNotas(NotasDAO DAO)
            {
                return DAO.ListaNotasDAO();
            }

            public List<Nota> NotasPorCuarto(NotasDAO DAO,int idcuarto)
            {
                return DAO.NotasPorCuarto(idcuarto);
            }

            public void AgregarNota(NotasDAO DAO, int idcuarto, string descripcion)
            {
                DAO.AgregarNota(idcuarto, descripcion);
            }

            public void ActualizarNota(NotasDAO DAO, int id, string descripcion)
            {
                DAO.ActualizarNota(id, descripcion);
            }

        }


    }
}
