using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCliente
    {
        Task<List<Cliente>> ListarClients();
    }
}
