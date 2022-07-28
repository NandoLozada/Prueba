using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    [Authorize]
    public class ReservasController: ControllerBase
    {
        private readonly ReservasDAO DAO;
        private readonly Reservas.ServReservas servLista;
        private readonly IGeneratePdf generatePdf;

        public ReservasController(ReservasDAO DAO, Reservas.ServReservas servLista, IGeneratePdf generatePdf)
        {
            this.DAO = DAO;
            this.servLista = servLista;
            this.generatePdf = generatePdf;
        }
        [HttpGet]
        public List<Reserva> Get()
        {
            var clientes = servLista.ListaReservas(DAO);
            return clientes;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet("PDF/{idcuarto}")]
        public Task<IActionResult> GetPorCuartoPDF(int idcuarto)
        {
            var reservas = servLista.ListaReservasPorCuarto(DAO, idcuarto);
            Response.ContentType="application/pdf";
            return generatePdf.GetPdf("View/ReservasVista.cshtml", reservas);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet("ReservaPorCuarto")]
        public List<Reserva> Get(int id)
        {
            var reservas = servLista.ListaReservasPorCuarto(DAO, id);
            //generatePdf.GetPdf("View/ReservasVista.cshtml", clientes);
            return reservas;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpGet("CuartosDisponibles")]
        public List<int> BuscarCuartosDisponibles(DateTime fechaini, DateTime fechafin)
        {
            var cuartos = servLista.CuartosDisponibles(DAO, fechaini, fechafin);
            return cuartos;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("CrearReserva")]
        public ActionResult Post(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            if (servLista.TomarReserva(DAO, idorden, idcuarto, fecinicio, fecfin))
            {
                return Ok();
            }

            return BadRequest("No se puede hacer la reserva");
            
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPatch("CambiarEstadoReserva")]
        public void EstadoReserva(int estado, int id)
        {
            servLista.EstadoReserva(DAO, estado, id);
        }
    }
}
