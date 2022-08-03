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
        public async Task<Cliente> GetPorDNI(int DNI)
        {
            var clientes = await _cliente.ClientePorDNI(DNI);
            return clientes;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("AgregarCliente")]
        public void Post(int DNI, string nombre)
        {
            _cliente.AgregarCliente(DNI, nombre);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPatch("CambiarEstadoCliente")]
        public void CambiarEstadoCliente(int estado, int id)
        {
            _cliente.CambiarEstadoCliente(estado, id);
        }
    }
}
