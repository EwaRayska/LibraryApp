using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace LibraryApp.API.Tests
{
    public class BooksAPITests : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _application;
        private readonly HttpClient _client;

        public BooksAPITests(WebApplicationFactory<Program> application)
        {
            _application = application;
            _client = _application.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var desriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBookRepository));
                    if (desriptor != null)
                    {
                        services.Remove(desriptor);
                    }

                    var mockBookRepository = new Mock<IBookRepository>();

                    var books = new List<Book>()
                    {
                        new Book { Id = 1, Title="Book One", Author = "Author A" },
                        new Book { Id = 2, Title = "Book Two", Author = "Author B" },
                        new Book { Id = 3, Title = "Book Three", Author = "Author C" }
                    };

                    mockBookRepository.Setup(repo => repo.GetAllBooks()).Returns(books);
                    mockBookRepository.Setup(repo => repo.GetBookById(It.IsAny<int>()))
                        .Returns((int id) => books.FirstOrDefault(b => b.Id == id));

                    services.AddScoped(_ => mockBookRepository.Object);

                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAllBooksReturnTheSameNumberOfBooksAsExisting()
        {
            //arrange  

            //act
            var response = await _client.GetAsync("/books");

            //assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<Book[]>();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public async Task Get_ReturnBookByItsId()
        {
            //arrange

            //act
            var response = await _client.GetAsync("/books/1");

            //assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<Book>();

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Post_AddNewBook()
        {
            //arrange
            var newBook = new Book
            {
                Title = "Test",
                Author = "Jan Kowalski"
            };

            //act
            var response = await _client.PostAsJsonAsync("/books", newBook);

            //assert
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();
            var createdBook = await response.Content.ReadFromJsonAsync<Book>();
            Assert.NotNull(createdBook);
            Assert.Equal("Test", createdBook.Title);
            Assert.Equal("Jan Kowalski", createdBook.Author);
        }

        [Fact]
        public async Task Delete_ShouldRemoveBookSuccessfully()
        {
            //arrange
            var bookToRemove = new Book
            {
                Title = "To be deleted",
                Author = "Anna Nowak"
            };

            var createResponse = await _client.PostAsJsonAsync("/books", bookToRemove);
            createResponse.EnsureSuccessStatusCode();
            var createdBook = await createResponse.Content.ReadFromJsonAsync<Book>();

            //act
            if(createdBook == null)
            {
                throw new Exception("Failed to create the book. The response returned null.");
            }
            var response = await _client.DeleteAsync($"/books/{createdBook.Id}");


            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


            var getResponse = await _client.GetAsync($"/books/{createdBook.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            _client.Dispose();
            await _application.DisposeAsync();
        }
    }
}