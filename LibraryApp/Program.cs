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

List<Book> books = new List<Book>();

books.Add(new Book("W pustyni i w puszczy", "Henryk Sienkiewicz"));
books.Add(new Book("Krzy¿acy", "Henryk Sienkiewicz"));
books.Add(new Book("Kubuœ Puchatek", "A.A. Milne"));
books.Add(new Book("Lalka", "Boleslaw Prus"));
books.Add(new Book("D¿uma", "Albert Camus"));
books.Add(new Book("Tango", "Slawomir Mrozek"));
books.Add(new Book("Akademia Pana Kleksa", "Jan Brzechwa"));
books.Add(new Book("Harry Potter", "J.K. Rowling"));
books.Add(new Book("Hobbit", "J.R.R. Tolkien"));
books.Add(new Book("Pan Tadeusz", "Adam Mickiewicz"));
books.Add(new Book("Balladyna", "Juliusz Slowacki"));

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

app.MapGet("/books", () =>
{
    app.Logger.LogInformation("I'm running on development");
    Enumerable.Range(1, books.Count).ToArray();
    return books;
});

app.MapGet("/books/{title}", (string title) =>
    books.FirstOrDefault(b => b.Title == title)
);

/*app.MapGet("/books/{author}", (string author) =>
    titles.Values.FirstOrDefault(a => a == author)
);*/

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
