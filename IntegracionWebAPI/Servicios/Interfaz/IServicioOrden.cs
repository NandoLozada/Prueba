using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioOrden
    {
        public Task<ResultadoOrden> AgregarOrden(int idcliente);
    }
}
