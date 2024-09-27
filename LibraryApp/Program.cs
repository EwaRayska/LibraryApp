using LibraryApp;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBookRepository, BookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/books", (IBookRepository repository) =>
{
    return Results.Ok(repository.GetAllBooks());
});

app.MapGet("/books/{id}", (int id, IBookRepository repository) =>
{
    var book = repository.GetBookById(id);
    return book != null ? Results.Ok(book) : Results.NotFound();
});

app.MapPost("/books", (Book book, IBookRepository repository) =>
{
    repository.AddBook(book);
    return Results.Created($"/books/{book.Id}", book);
 });

app.MapPut("/books/{id}", (int id, Book book, IBookRepository repository) =>
{
    var existingBook = repository.GetBookById(id);
    if(existingBook == null) return Results.NotFound();

    book.Id = id;
    repository.UpdateBook(book);
    return Results.NoContent();
});

app.MapDelete("/books/{id}", (int id, IBookRepository repository) =>
{
    var book = repository.GetBookById(id);
    if (book != null) return Results.NotFound();

    repository.DeleteBook(id);
    return Results.NoContent();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
