namespace the_library.Repository.Abstract
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
    }
}
