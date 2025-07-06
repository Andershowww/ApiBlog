namespace ApiBlog.Usuarios.Models
{
    using ApiBlog.Post.Models;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;

        public ICollection<Post> Posts { get; set; }
        public ICollection<UsuarioSeguido> Seguindo { get; set; }
        public ICollection<UsuarioSeguido> Seguidores { get; set; }
        public ICollection<PostCurtido> Curtidas { get; set; }
        public ICollection<PostComentario> Comentarios { get; set; }
    }

}
