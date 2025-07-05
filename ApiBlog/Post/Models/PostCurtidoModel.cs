using ApiBlog.Auth.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBlog.Post.Models
{
    [Table("PostCurtido")]
    public class PostCurtido
    {
        [ForeignKey("Post")]
        public int IdPost { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public DateTime DataCurtido { get; set; } = DateTime.UtcNow;
    }
}
