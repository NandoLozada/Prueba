using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/ordenes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdenesController : ControllerBase
    {
        private readonly IServicioOrden _orden;

        public OrdenesController(IServicioOrden orden)
        {
            _orden = orden;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("CrearOrden")]
        public async Task<int> Post(int idcliente)
        {
            var idorden = await _orden.AgregarOrden(idcliente);
            return idorden;

        }
    }
}
