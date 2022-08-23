using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
//using IntegracionWebAPI.DAOs;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<List<Cuarto>>> Get()
        {
            var cuartos = await _cuarto.ListaCuartos();

            if (cuartos.ok)
            {
                return Ok(cuartos.cuartos);
            }
            return BadRequest(cuartos.mensaje);
        }

        [HttpGet("UnCuarto/{Id}")]
        public async Task<ActionResult<Cuarto>> CuartoPorId(int Id)
        {
            if (Id !=0)
            {
                var resultado = await _cuarto.CuartoPorId(Id);

                if (resultado.ok)
                {
                    return Ok(resultado.cuarto);
                }
                else { return BadRequest(resultado.mensaje);}
            }
            else
            {
                return BadRequest("El campo Id no puede estar vacio");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("AgregarunCuarto")]
        public async Task<ActionResult> Post ([FromForm] int capacidad,  IFormFile foto)
        {
            if ((capacidad != 0)&(foto != null))
            {
                string foto64;

                using (var ms = new MemoryStream())
                {
                    foto.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    foto64 = Convert.ToBase64String(fileBytes);
                }
                var resultado = await _cuarto.AgregarCuarto(capacidad, foto64);

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
                return BadRequest("No puede haber campos vacios");
            }

        }

        [HttpPatch("CambiarEstadoCuarto")]
        public async Task<ActionResult> EstadoCuarto(int estado, int id)
        {
            if((estado != 0)&(id != 0))
            {
                var resultado = await _cuarto.CambiarEstadoCuarto(estado, id);

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
                return BadRequest("No puede haber campos vacios");
            }
            
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPut("ActualizarCuarto")]
        public async Task<ActionResult> ActualizarCuarto([FromForm]int id, int capacidad, IFormFile foto)
        {
            if ((id != 0)&(capacidad != 0)&(foto != null))
            {
                string foto64;

                using (var ms = new MemoryStream())
                {
                    foto.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    foto64 = Convert.ToBase64String(fileBytes);
                }
                var resultado = await _cuarto.ActualizarCuarto(id, capacidad, foto64);

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
                return BadRequest("No puede haber campos vacios");
            }
        }
    }
}
