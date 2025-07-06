namespace ApiBlog.Timeline.DTO
{
    public class ComentarioResponse
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Texto { get; set; }
        public DateTime DataComentario { get; set; }
    }

}
