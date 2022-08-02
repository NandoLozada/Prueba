using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    [Authorize]
    public class ReservasController: ControllerBase
    {private readonly IServicioReserva _reserva;
        private readonly IGeneratePdf generatePdf;

        public ReservasController(IServicioReserva reserva, IGeneratePdf generatePdf)
        {   
            _reserva = reserva;
            this.generatePdf = generatePdf;
        }
        [HttpGet]
        public async Task<List<Reserva>> Get()
        {
            var clientes = await _reserva.ListaReservas();
            return clientes;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet("PDF/{idcuarto}")]
        public async Task<IActionResult> GetPorCuartoPDF(int idcuarto)
        {
            var reservas = await _reserva.ListaReservasPorCuarto(idcuarto);
            Response.ContentType="application/pdf";
            return await generatePdf.GetPdf("View/ReservasVista.cshtml", reservas);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet("ReservaPorCuarto")]
        public async Task<List<Reserva>> Get(int id)
        {
            var reservas = await _reserva.ListaReservasPorCuarto(id);
            return reservas;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpGet("CuartosDisponibles")]
        public async Task<List<int>> BuscarCuartosDisponibles(DateTime fechaini, DateTime fechafin)
        {
            var cuartos = await _reserva.CuartosDisponibles(fechaini, fechafin);
            return cuartos;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("CrearReserva")]
        public async Task <ActionResult> Post(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            if (await _reserva.TomarReserva(idorden, idcuarto, fecinicio, fecfin))
            {
                return Ok();
            }

            return BadRequest("No se puede hacer la reserva");
            
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPatch("CambiarEstadoReserva")]
        public void CambiarEstadoReserva(int estado, int id)
        {
            _reserva.CambiarEstadoReserva(estado, id);
        }
    }
}
