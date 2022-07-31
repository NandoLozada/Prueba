using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using IntegracionWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/cuartos")]
    [Authorize]
    public class CuartosController : ControllerBase
    {
        private readonly IServicioCuarto _cuarto;
        private readonly IGeneratePdf generatePdf;

        public CuartosController(IServicioCuarto cuarto, IGeneratePdf generatePdf)
        {
            _cuarto = cuarto;
            this.generatePdf = generatePdf;
        }


        [HttpGet("ListadeCuartos")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        public Task<List<Cuarto>> Get()
        {
            var cuartos = _cuarto.ListaCuartos();
            return cuartos;
        }

        [HttpGet("UnCuarto/{Id}")]
        public Task<Cuarto> CuartoPorId(int Id)
        {
            var cuartos = _cuarto.CuartoPorId(Id);
            return cuartos;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("AgregarunCuarto")]
        public void Post([FromForm] int capacidad,  IFormFile foto)
        {
            string foto64;

            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                var fileBytes = ms.ToArray();
                foto64 = Convert.ToBase64String(fileBytes);
            }
            _cuarto.AgregarCuarto(capacidad, foto64);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPatch("CambiarEstadoCuarto")]
        public void EstadoCuarto(int estado, int id)
        {
            _cuarto.EstadoCuarto(estado, id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPut("ActualizarCuarto")]
        public void ActualizarCuarto([FromForm]int id, int capacidad, IFormFile foto)
        {
            string foto64;

            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                var fileBytes = ms.ToArray();
                foto64 = Convert.ToBase64String(fileBytes);
            }
            _cuarto.ActualizarCuarto(id, capacidad, foto);
        }
    }
}
