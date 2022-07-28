using IntegracionWebAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using IntegracionWebAPI.DAOs;
using IntegracionWebAPI.Servicios;
using Wkhtmltopdf.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IntegracionWebAPI.Controllers
{
    [ApiController]
    [Route("api/Notas")]
    [Authorize]
    public class NotasController: ControllerBase
    {
        private readonly NotasDAO DAO;
        private readonly Notas.ServNotas servLista;
        private readonly IGeneratePdf generatePdf;

        public NotasController(NotasDAO DAO, Notas.ServNotas servLista, IGeneratePdf generatePdf)
        {
            this.DAO = DAO;
            this.servLista = servLista;
            this.generatePdf = generatePdf;
        }
        [HttpGet]
        public List<Nota> Get()
        {
            var notas = servLista.ListaNotas(DAO);
            return notas;
        }

        [HttpGet("{idcuarto}")]
        public List<Nota> GetPorCuarto(int idcuarto)
        {
            var notas = servLista.NotasPorCuarto(DAO, idcuarto);
            return notas.ToList();
        }

        [HttpGet("PDF/{idcuarto}")]
        public Task<IActionResult> GetPorCuartoPDF(int idcuarto)
        {
            var notas = servLista.NotasPorCuarto(DAO, idcuarto);
            return generatePdf.GetPdf("View/NotasVista.cshtml", notas);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAC")]
        [HttpPost("AgregarNota")]
        public void Post(int idcuarto, string descripcion)
        {
            servLista.AgregarNota(DAO, idcuarto, descripcion);
        }

        [HttpPut]
        public void ActualizarNota(int id, string descripcion)
        {
            servLista.ActualizarNota(DAO, id, descripcion);
        }
    }
}
