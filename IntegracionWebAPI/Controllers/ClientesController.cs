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
    [Authorize]
    public class ClientesController: ControllerBase
    {
        private readonly ClientesDAO DAO;
        private readonly Clientes.ServClientes servLista;

        private readonly IServicioCliente _cliente;

        public ClientesController(ClientesDAO DAO, Clientes.ServClientes servLista, IServicioCliente cliente)
        {
            this.DAO = DAO;
            this.servLista = servLista;
            _cliente = cliente;
        }
        [HttpGet("6-ListaClientes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<List<Cliente>> Get()
        {
            var clientes = servLista.ListaClientes(DAO);
            var clients = await _cliente.ListarClients();
            return clients;
        }
                
        [HttpGet("7-15-InfoCliente/{DNI}")]
        public List<Cliente> GetPorDNI(int DNI)
        {
            var clientes = servLista.ClientePorDNI(DAO, DNI);
            return clientes;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("8-16-AgregarCliente")]
        public void Post(int DNI, string nombre)
        {
            servLista.AgregarCliente(DAO, DNI, nombre);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPatch("8-EstadoCliente")]
        public void EstadoCliente(int estado, int id)
        {
            servLista.EstadoCliente(DAO,estado, id);
        }
    }
}
