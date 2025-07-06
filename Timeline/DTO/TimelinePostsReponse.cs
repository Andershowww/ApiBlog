namespace ApiBlog.Timeline.DTO
{
    public class TimelinePostsReponse
    {
        public int PostId { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public DateTime DataCriacao { get; set; }

        public int AutorId { get; set; }
        public string AutorUsername { get; set; }

        public List<string> Tags { get; set; } = new();

        public int TotalCurtidas { get; set; }

        public List<ComentarioResponse> Comentarios { get; set; } = new();
    }
}
