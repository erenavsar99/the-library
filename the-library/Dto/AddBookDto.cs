namespace the_library.Dto
{
    public class AddBookDto
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public IFormFile Media { get; set; }
    }
}
