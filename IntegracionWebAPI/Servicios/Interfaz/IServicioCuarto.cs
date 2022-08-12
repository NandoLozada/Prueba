using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCuarto
    {
        public Task<ResultadoCuarto> ListaCuartos();

        public Task<ResultadoCuarto> CuartoPorId(int Id);

        public Task<ResultadoCuarto> AgregarCuarto(int capacidad, string foto);

        public Task<ResultadoCuarto> CambiarEstadoCuarto(int estado, int id);

        public Task<ResultadoCuarto> ActualizarCuarto(int id, int capacidad, string foto);

    }
}
