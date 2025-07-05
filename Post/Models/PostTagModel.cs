using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBlog.Post.Models
{
    [Table("PostTags")]
    public class PostTag
    {
        [ForeignKey("Post")]
        public int IdPost { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Tag")]
        public int IdTag { get; set; }
        public ApiBlog.Tag.Models.Tag Tag { get; set; }
    }
}
