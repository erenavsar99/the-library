using System.ComponentModel.DataAnnotations.Schema;

namespace the_library.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public string? Entrustee { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Path { get; set; }

        [NotMapped]
        public string ImageFile { get; set; }
    }
}
