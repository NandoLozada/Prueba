using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Utiles
{
    public class ResultadoOrden:Resultado
    {
        public Orden orden { get; set; }
        public int ordenId { get; set; }
    }
}
