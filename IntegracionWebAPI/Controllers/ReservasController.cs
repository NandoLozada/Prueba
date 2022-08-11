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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservasController: ControllerBase
    {private readonly IServicioReserva _reserva;
        private readonly IGeneratePdf generatePdf;

        public ReservasController(IServicioReserva reserva, IGeneratePdf generatePdf)
        {   
            _reserva = reserva;
            this.generatePdf = generatePdf;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get()
        {
            var reservas = await _reserva.ListaReservas();

            if (reservas.ok)
            {
                return Ok(reservas.reserva);
            }

            return BadRequest(reservas.mensaje);
        }        

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpGet("ReservaPorCuarto")]
        public async Task<ActionResult<List<Reserva>>> Get(int id)
        {
            if (id != 0)
            {
                var reservas = await _reserva.ListaReservasPorCuarto(id);

                if (reservas.ok)
                {
                    return Ok(reservas.reserva);
                }
                else { return BadRequest(reservas.mensaje); }
            }
            else
            {
                return BadRequest("El campo Id no puede estar vacio");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpGet("CuartosDisponibles")]
        public async Task<ActionResult<List<int>>> BuscarCuartosDisponibles(DateTime fechaini, DateTime fechafin)
        {
            if ((Convert.ToString(fechafin) != "") & (Convert.ToString(fechaini) != ""))
            {
                var cuartos = await _reserva.CuartosDisponibles(fechaini, fechafin);

                if (cuartos.ok)
                {
                    return Ok(cuartos.cuartosdisponibles);
                }
                else { return BadRequest(cuartos.mensaje); }
            }
            else
            {
                return BadRequest("Los campos de fechas no puede estar vacio");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("CrearReserva")]
        public async Task <ActionResult> Post(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            if ((idorden != 0)&(idcuarto !=0)&(Convert.ToString(fecinicio) !="")&(Convert.ToString(fecfin) != ""))
            {
                var resultado = await _reserva.TomarReserva(idorden, idcuarto, fecinicio, fecfin);

                if (resultado.ok)
                {
                    return Ok(resultado.mensaje);
                }
                else
                {
                    return BadRequest(resultado.mensaje);
                }
            }
            else
            {
                return BadRequest("No pueden haber campos vacios");
            }
            
        }

        [HttpPatch("CambiarEstadoReserva")]
        public async Task<ActionResult> CambiarEstadoReserva(int estado, int id)
        {
            if ((estado != 0)&(id != 0))
            {
                var resultado = await _reserva.CambiarEstadoReserva(estado, id);

                if(resultado.ok)
                {
                    return Ok(resultado);
                }

                return BadRequest(resultado);
            }
            else
            {
                return BadRequest("Los campos no pueden estar vacios");
            }
        }
    }
}
