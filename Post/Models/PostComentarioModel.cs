using ApiBlog.Auth.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Post.Models
{
    [Table("PostComentario")]
    public class PostComentario
    {
        [Key]
        public int IdComentario { get; set; }

        [ForeignKey("Post")]
        public int IdPost { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public string Comentario { get; set; }

        public DateTime DataComentario { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;
    }
}
