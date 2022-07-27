using Dapper;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.Servicios
{
    public interface IReservas
    {
    }

    public class Reservas : IReservas
    {
        public class ServReservas : IReservas
        {
            public bool TomarReserva(ReservasDAO DAO, int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
            {
                bool ok = false;
                if (HayDisponibilidad(DAO, idcuarto, fecinicio, fecfin))
                {
                    ok = DAO.AgregarReserva(idorden, idcuarto, fecinicio, fecfin);
                }

                if(ok)
                { return true; }
                else { return false; }
                
            }

            bool HayDisponibilidad(ReservasDAO DAO, int idcuarto, DateTime fecinicio, DateTime fecfin)
            {
                List<Reserva> reservaList = new List<Reserva>();

                reservaList = ReservaPosible(DAO, idcuarto,fecinicio, fecfin); //Lista de reservas para un cuarto

                foreach(Reserva reserva in reservaList)
                {
                    if (fecinicio > reserva.FechaInicio & fecinicio < reserva.FechaFin)// esto en la query
                    {
                        return false;
                    }
                    else
                    {
                       if (fecfin> reserva.FechaInicio & fecfin < reserva.FechaFin)
                        {
                            return false;
                        }
                    }  
                    
                    if (fecinicio < reserva.FechaInicio & fecfin > reserva.FechaFin)
                    {
                        return false;
                    }
                }
                return true;
            }

            public List<Reserva> ReservaPosible(ReservasDAO DAO, int idcuarto, DateTime fecinicio, DateTime fecfin)
            {
                var reservascuarto = DAO.ReservasPorCuarto(idcuarto, fecinicio, fecfin);

                return reservascuarto; //Todas las reservas de un cuarto
            }

           public List<int> ListaIdCuartosActivos (ReservasDAO DAO)
            {
                var listacuartos = DAO.ListaIdCuarto();
                return listacuartos;
            }

            public List<int> CuartosDisponibles(ReservasDAO DAO, DateTime fechaini, DateTime fechafin)
            {
                List<int> listacuartosdisponibles= new List<int>();

                var listaid = ListaIdCuartosActivos(DAO);

                foreach(int Id in listaid)
                {
                    if (HayDisponibilidad(DAO, Id, fechaini, fechafin))
                    {
                        listacuartosdisponibles.Add(Id);
                    }
                }

                return listacuartosdisponibles;
            }

            public List<Reserva> ListaReservas(ReservasDAO DAO)
            {
                return DAO.ListaReservasDAO();
            }

            public List<Reserva> ListaReservasPorCuarto(ReservasDAO DAO, int id)
            {
                return DAO.ListaReservasPorCuarto(id);
            }

            public void EstadoReserva(ReservasDAO DAO, int estado, int id)
            {
                DAO.EstadoReserva(estado, id);
            }

        }


    }
}
