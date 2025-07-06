using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBlog.Usuarios.Models
{

    [Table("UsuarioSeguido")]
    public class UsuarioSeguido
    {
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("UsuarioSeguidoRef")]
        public int IdUsuarioSeguido { get; set; }
        public Usuario UsuarioSeguidoRef { get; set; }

        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}
