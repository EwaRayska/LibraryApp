using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace LibraryApp.API.Tests
{
    public class BooksAPITests
    {
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
    }
}