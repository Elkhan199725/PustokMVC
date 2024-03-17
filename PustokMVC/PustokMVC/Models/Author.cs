using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Models
{
    public class Author : BaseEntity
    {
        [StringLength(50)]
        public string? FullName { get; set; }
        public List<Book>? Books { get; set; }
    }
}
