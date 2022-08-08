using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCuarto
    {
        public Task<List<Cuarto>> ListaCuartos();

        public Task<List<Cuarto>> CuartoPorId(int Id);

        public Task<Resultado> AgregarCuarto(int capacidad, string foto);

        public Task<Resultado> EstadoCuarto(int estado, int id);

        public Task<Resultado> ActualizarCuarto(int id, int capacidad, string foto);

    }
}
