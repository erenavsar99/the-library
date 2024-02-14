using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using the_library.Data;
using the_library.Models;
using the_library.Dto;
using the_library.Repository.Abstract;
using the_library.Repository.Implementation;

namespace the_library.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class BooksController : Controller
    {
        private readonly TheLibraryDbContext _context;
        private IFileService _fileService;
        private ILogger<BooksController> _logger;

        public BooksController(TheLibraryDbContext theLibraryDbContext, IFileService fileService, ILogger<BooksController> logger)
        {
            _context = theLibraryDbContext;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation("Get All Books executing...");

            var books = await _context.Books.ToListAsync();

            foreach (var book in books)
            {
                if(!string.IsNullOrEmpty(book.Path) && System.IO.File.Exists(book.Path))
                {
                    byte[] byteA = await System.IO.File.ReadAllBytesAsync(book.Path);
                   book.ImageFile = Convert.ToBase64String(byteA);
                } else
                {
                    book.ImageFile = null;
                }
            }

            return Ok(books);
        }

        [HttpPost("AddBookAsync")]
        public async Task<IActionResult> AddBook([FromForm] AddBookDto bookRequest)
        {
            _logger.LogInformation($"{bookRequest}, add new book method is executed...");
           var imageResult =  _fileService.SaveImage(bookRequest.Media);
            var newPath = "";

            if(imageResult.Item1 == 1)
            {
                newPath = imageResult.Item2;
            }
            else
            {
                _logger.LogError("Unsupported media type!");
                return StatusCode(422, "Desteklenmeyen görsel türü");
            }
                 
            var newBook = new Book()
            {
                Id = Guid.NewGuid(),
                Name = bookRequest.Name,
                Author = bookRequest.Author,
                IsAvailable = true,
                EndDate = null,
                Entrustee = null,
                Path = newPath
            };

            await _context.Books.AddAsync(newBook);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{bookRequest.Name}, has been added and saved. ");

            return Ok("Kitap Eklendi!");
        }

        [HttpPost("GiveBookAsync")]
        public async Task<IActionResult> GiveBook([FromBody] EntrusteeDto bookRequest)
        {
            _logger.LogInformation($"A book with id, { bookRequest.Id}, is requested to be borrowed by person { bookRequest.Entrustee} until date {bookRequest.Entrustee}");

            var book = await _context.Books.Where(x=>x.Id == bookRequest.Id).FirstOrDefaultAsync();

            book.Entrustee = bookRequest.Entrustee;
            book.EndDate = bookRequest.EndDate;
            book.IsAvailable = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"{bookRequest.Id}, book has been given and saved to db...");

            return Ok("Kitap Ödünç Verildi!");
        }
    }
}
