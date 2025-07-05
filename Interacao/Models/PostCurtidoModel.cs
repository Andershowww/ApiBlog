using ApiBlog.Auth.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBlog.Interacao.Models
{
    [Table("PostCurtido")]
    public class PostCurtido
    {
        [ForeignKey("Post")]
        public int IdPost { get; set; }
        public ApiBlog.Post.Models.Post Post { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public DateTime DataCurtido { get; set; } = DateTime.UtcNow;
    }
}
