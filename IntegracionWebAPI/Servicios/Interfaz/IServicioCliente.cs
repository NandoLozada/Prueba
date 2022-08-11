using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioCliente
    {
        Task<List<Cliente>> ListarClientes();

        Task<ResultadoCliente> ClientePorDNI(int DNI);

        Task<ResultadoCliente> AgregarCliente(int DNI, string nombre);

        Task<ResultadoCliente> CambiarEstadoCliente(int estado, int id);
    }
}
