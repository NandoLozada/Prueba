using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Utiles
{
    public class ResultadoCliente:Resultado
    {
        public List<Cliente> clientes { get; set; }
        public Cliente cliente { get; set; }    
    }
}
