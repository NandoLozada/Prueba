using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCliente
    {
        Task<List<Cliente>> ListarClientes();

        Task<Cliente> ClientePorDNI(int DNI);

        void AgregarCliente(int DNI, string nombre);

        void CambiarEstadoCliente(int estado, int id);
    }
}
