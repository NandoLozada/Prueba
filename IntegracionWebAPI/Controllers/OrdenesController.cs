using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/ordenes")]
    [Authorize]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenesDAO DAO;
        private readonly Ordenes.ServOrdenes servLista;

        public OrdenesController(OrdenesDAO DAO, Ordenes.ServOrdenes servLista)
        {
            this.DAO = DAO;
            this.servLista = servLista;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("17-CrearOrden")]
        public int Post(int idcliente)
        {
            var idorden = servLista.AgregarOrden(DAO, idcliente);
            return idorden;

        }
    }
}
