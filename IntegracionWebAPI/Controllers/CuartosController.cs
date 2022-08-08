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
        public async Task<List<Cuarto>> Get()
        {
            var cuartos = await _cuarto.ListaCuartos();
            return cuartos;
        }

        [HttpGet("UnCuarto/{Id}")]
        public async Task<ActionResult<Cuarto>> CuartoPorId(int Id)
        {
            if (Id !=0)
            {
                var cuartos = await _cuarto.CuartoPorId(Id);

                if (cuartos != null)
                {
                    return Ok(cuartos);
                }
                else
                {
                    return NotFound("No hay cuarto con ese Id");
                }
            }
            else
            {
                return BadRequest("Ingrese un Id valido");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPost("AgregarunCuarto")]
        public async Task<ActionResult> Post ([FromForm] int capacidad,  IFormFile foto)
        {
            string foto64;

            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                var fileBytes = ms.ToArray();
                foto64 = Convert.ToBase64String(fileBytes);
            }
            var res = await _cuarto.AgregarCuarto(capacidad, foto64);

            if (res.ok)
            {
                return Ok("El cuarto se agregó correctamente");
            }
            else
            {
                return BadRequest(res.mensaje);
            }

        }

        [HttpPatch("CambiarEstadoCuarto")]
        public async Task<ActionResult> EstadoCuarto(int estado, int id)
        {
            var res = await _cuarto.EstadoCuarto(estado, id);

            if(res.ok)
            {
                return Ok("El estado del cuarto fue modificado con exito");
            }
            else
            {
                return BadRequest(res.mensaje);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        [HttpPut("ActualizarCuarto")]
        public async Task<ActionResult> ActualizarCuarto([FromForm]int id, int capacidad, IFormFile foto)
        {
            string foto64;

            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                var fileBytes = ms.ToArray();
                foto64 = Convert.ToBase64String(fileBytes);
            }
            var res = await _cuarto.ActualizarCuarto(id, capacidad, foto64);

            if(res.ok)
            {
                return Ok("El cuarto se actualizo con exito");
            }
            else
            {
                return BadRequest(res.mensaje);
            }
        }
    }
}
