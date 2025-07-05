namespace ApiBlog.Post.Models
{
    using ApiBlog.Auth.Models;
    using ApiBlog.Interacao.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Post")]
    public class Post
    {
        [Key]
        public int IDPost { get; set; }

        [Required, MaxLength(100)]
        public string Titulo { get; set; }

        [Required]
        public string Corpo { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<PostCurtido> Curtidas { get; set; }
        public ICollection<PostComentario> Comentarios { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }

}
