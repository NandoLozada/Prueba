using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Utiles
{
    public class ResultadoCuarto:Resultado
    {
        public List<Cuarto> cuartos { get; set; }
        public Cuarto cuarto { get; set; }
    }
}
