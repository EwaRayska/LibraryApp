using LibraryApp;
using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var books = new List<Book>
{
    new (1, "W pustyni i w puszczy", "Henryk Sienkiewicz"),
    new (2, "Krzy¿acy", "Henryk Sienkiewicz"),
    new (3, "Kubuœ Puchatek", "A.A. Milne"),
    new (4, "Lalka", "Boleslaw Prus"),
    new (5, "D¿uma", "Albert Camus"),
    new (6, "Tango", "Slawomir Mrozek"),
    new (7, "Akademia Pana Kleksa", "Jan Brzechwa"),
    new (8, "Harry Potter", "J.K. Rowling"),
    new (9, "Hobbit", "J.R.R. Tolkien"),
    new (10, "Pan Tadeusz", "Adam Mickiewicz"),
    new (11, "Balladyna", "Juliusz Slowacki")
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/books", () => books);

app.MapGet("/books/{title}", (string title) =>
{
    var book = books.FirstOrDefault(b => b.Title == title);
    if (book == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(book);
});

/*app.MapGet("/books/{author}", (string author) =>
    titles.Values.FirstOrDefault(a => a == author)
);*/

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
