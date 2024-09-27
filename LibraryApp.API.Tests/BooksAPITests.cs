using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace LibraryApp.API.Tests
{
    public class BooksAPITests : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> application;
        private readonly HttpClient client;

        public BooksAPITests()
        {
            application = new WebApplicationFactory<Program>();
            client = application.CreateClient();
        }

        [Fact]
        public async Task GetAllBooksReturnTheSameNumberOfBooksAsExisting()
        {
            //arrange
            await using var application = new WebApplicationFactory<Program>(); 
            using var client = application.CreateClient();

            //act
            var response = await client.GetAsync("/books");

            //assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<Book[]>();

            Assert.NotNull(result);
            Assert.Equal(11, result.Length);
        }

        [Fact]
        public async Task Get_ReturnBookByItsTitle()
        {
            //arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            //act
            var response = await client.GetAsync("/books/Balladyna");

            //assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<Book>();

            Assert.NotNull(result);
            Assert.Equal("Balladyna", result.Title);
        }

        [Fact]
        public async Task Post_AddNewBook()
        {
            //arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var newBook = new Book
            {
                Title = "Test",
                Author = "Jan Kowalski"
            };

            //act
            var response = await client.PostAsJsonAsync("/books", newBook);

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

            var createResponse = await client.PostAsJsonAsync("/books", bookToRemove);
            createResponse.EnsureSuccessStatusCode();
            var createdBook = await createResponse.Content.ReadFromJsonAsync<Book>();

            //act
            if(createdBook == null)
            {
                throw new Exception("Failed to create the book. The response returned null.");
            }
            var response = await client.DeleteAsync($"/books/{createdBook.Id}");


            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


            var getResponse = await client.GetAsync($"/books/{createdBook.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            client.Dispose();
            await application.DisposeAsync();
        }
    }
}