using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCliente
    {
        Task<List<Cliente>> ListarClientes();

        Task<Cliente> ClientePorDNI(int DNI);

        Task <Resultado> AgregarCliente(int DNI, string nombre);

        Task<Resultado> CambiarEstadoCliente(int estado, int id);
    }
}
