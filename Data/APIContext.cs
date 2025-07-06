using ApiBlog.Post.Models;
using ApiBlog.Usuario.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Data
{
    public class APIContext : IdentityDbContext<IdentityUser>
    {
        public APIContext(DbContextOptions<APIContext> options)
             : base(options)
        {
        }
        public APIContext()
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ApiBlog.Post.Models.Post> Posts { get; set; }
        public DbSet<PostCurtido> PostsCurtidos { get; set; }
        public DbSet<PostComentario> PostsComentarios { get; set; }
        public DbSet<ApiBlog.Tag.Models.Tag> Tags { get; set; }
        public DbSet<PostTag> PostsTags { get; set; }
        public DbSet<UsuarioSeguido> UsuariosSeguidos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioSeguido>()
               .HasKey(us => new { us.IdUsuario, us.IdUsuarioSeguido });

            modelBuilder.Entity<UsuarioSeguido>()
                .HasOne(us => us.Usuario)
                .WithMany(u => u.Seguindo)
                .HasForeignKey(us => us.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioSeguido>()
                .HasOne(us => us.UsuarioSeguidoRef)
                .WithMany(u => u.Seguidores)
                .HasForeignKey(us => us.IdUsuarioSeguido)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostCurtido>()
                .HasKey(pc => new { pc.IdPost, pc.IdUsuario });

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.IdPost, pt.IdTag });

            modelBuilder.Entity<PostComentario>()
                .HasKey(pc => new { pc.IdPost, pc.IdUsuario, pc.DataComentario });
        }
    }
}
