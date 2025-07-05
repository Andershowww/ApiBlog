namespace ApiBlog.Tag.Models
{
    using ApiBlog.Post.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Tags")]
    public class Tag
    {
        [Key]
        public int IdTag { get; set; }

        [Required, MaxLength(50)]
        public string Nome { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<PostTag> PostTags { get; set; }
    }
}
