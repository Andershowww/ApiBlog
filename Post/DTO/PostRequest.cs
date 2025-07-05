namespace ApiBlog.Post.DTO
{
    public class PostRequest
    {
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
