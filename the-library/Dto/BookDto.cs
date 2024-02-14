using System.ComponentModel.DataAnnotations.Schema;

namespace the_library.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public string? Entrustee { get; set; }
        public DateTime? EndDate { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
