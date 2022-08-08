using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController: ControllerBase
    {
        private readonly IServicioCliente _cliente;

        public ClientesController(IServicioCliente cliente)
        {
            _cliente = cliente;
        }
        [HttpGet("ListaClientes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<List<Cliente>> Get()
        {
            var clientes = await _cliente.ListarClientes();
            return clientes;
        }

        [HttpGet("Cliente/{DNI}")]
        public async Task<ActionResult<Cliente>> GetPorDNI(int DNI)
        {
            if (DNI != 0)
            {
                var clientes = await _cliente.ClientePorDNI(DNI);
                if (clientes != null)
                {
                    return Ok(clientes);
                }
                else { return NotFound("No se encuentra ningun cliente con ese DNI"); }
            }
            else
            {
                return BadRequest("El campo DNI no puede estar vacio");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("AgregarCliente")]
        public async Task<ActionResult> Post(int DNI, string nombre)
        {            
            if ((nombre != null) & (DNI != 0))
            {
                var res = await _cliente.AgregarCliente(DNI, nombre);

                if (res.ok)
                {
                    return Ok("El cliente se agregó con exito");
                }
                else
                {
                    return BadRequest(res.mensaje);
                }
            }
            else
            {
                return BadRequest("No puede haber campos vacios");
            }
            
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPatch("CambiarEstadoCliente")]
        public async Task<ActionResult> CambiarEstadoCliente(int estado, int id)
        {
            var res = await _cliente.CambiarEstadoCliente(estado, id);

            if ((estado != 0)&(id != 0))
            {
                if (res.ok)
                {
                    return Ok("El cliente ha cambiado su estado a " + estado + " con exito");
                }
                else
                {
                    return BadRequest(res.mensaje);
                }
            }
            else
            {
                return BadRequest("No puede haber campos vacios");
            }
        }
    }
}
