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
    [Route("api/Notas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotasController : ControllerBase
    {
        private readonly IServicioNota _nota;
        private readonly IGeneratePdf generatePdf;

        public NotasController(IServicioNota nota, IGeneratePdf generatePdf)
        {
            _nota = nota;
            this.generatePdf = generatePdf;
        }

        [HttpGet("TodasLasNotas")]
        public async Task<ActionResult<List<Nota>>> TodasLasNotas()
        {
            var notas = await _nota.ListaNotas();

            if(notas.ok)
            {
                return Ok(notas.notas);
            }
            return BadRequest(notas.mensaje);
        }

        [HttpGet("{idcuarto}")]
        public async Task<ActionResult<List<Nota>>> NotasPorCuarto(int idcuarto)
        {
            if (idcuarto != 0)
            {
                var resultado = await _nota.NotasPorCuarto(idcuarto);
                if (resultado.ok)
                {
                    return Ok(resultado.notas);
                }
                else { return BadRequest(resultado.mensaje);}
            }
            else
            {
                return BadRequest("El campo Id no puede estar vacio");
            }
        }

        [HttpGet("PDF/{idcuarto}")]
        public async Task<IActionResult> NotasPorCuartoPDF(int idcuarto)
        {
            if (idcuarto != 0)
            {
                var resultado = await _nota.NotasPorCuarto(idcuarto);

                if (resultado.ok)
                {
                    return await generatePdf.GetPdf("View/NotasVista.cshtml", resultado.notas);
                }
                else { return BadRequest(resultado.mensaje);}
            }
            else
            {
                return BadRequest("El campo Id no puede estar vacio");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("AgregarNota")]
        public async Task<ActionResult> PAgregarNota(int idcuarto, string descripcion)
        {
            if ((idcuarto != 0) & (descripcion != ""))
            {
                var resultado = await _nota.AgregarNota(idcuarto, descripcion);
                if (resultado.ok)
                {
                    return Ok(resultado.mensaje);
                }
                else { return BadRequest(resultado.mensaje);}
            }
            else
            {
                return BadRequest("No puede haber campos vacios");
            }
        }

        [HttpPut("ActualizarNota")]
        public async Task<ActionResult> ActualizarNota(int id, string descripcion)
        {
            if ((id != 0)&(descripcion!= ""))
            {
                var res = await _nota.ActualizarNota(id, descripcion);

                if(res.ok)
                {
                    return Ok("La nota fue actualizada con exito");
                }
                else { return BadRequest(res.mensaje);}
            }
            else
            {
                return BadRequest("Los campos no pueden estar vacios");
            }
        }
    }
}
