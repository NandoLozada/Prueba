using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize]
    public class ClientesController: ControllerBase
    {
        private readonly ClientesDAO DAO;
        private readonly Clientes.ServClientes servLista;

        public ClientesController(ClientesDAO DAO, Clientes.ServClientes servLista)
        {
            this.DAO = DAO;
            this.servLista = servLista;
        }
        [HttpGet("6-ListaClientes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public List<Cliente> Get()
        {
            var clientes = servLista.ListaClientes(DAO);
            return clientes;
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
