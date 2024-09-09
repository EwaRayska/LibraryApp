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

Dictionary<string, string> titles = new Dictionary<string, string>();

Book b1 = new Book("W pustyni i w puszczy", "Henryk Sienkiewicz");
Book b2 = new Book("Krzy¿acy", "Henryk Sienkiewicz");
Book b3 = new Book("Kubuœ Puchatek", "A.A. Milne");
Book b4 = new Book("Lalka", "Boleslaw Prus");
Book b5 = new Book("D¿uma", "Albert Camus");
Book b6 = new Book("Tango", "Slawomir Mrozek");
Book b7 = new Book("Akademia Pana Kleksa", "Jan Brzechwa");
Book b8 = new Book("Harry Potter", "J.K. Rowling");
Book b9 = new Book("Hobbit", "J.R.R. Tolkien");
Book b10 = new Book("Pan Tadeusz", "Adam Mickiewicz");
Book b11 = new Book("Balladyna", "Juliusz Slowacki");

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
    var books = Enumerable.Range(1, titles.Count).Select(index =>
    new Book
    (
            index,
            titles.Keys.ElementAt(index),
            titles.Values.ElementAt(index)
        ))
    .ToArray();
    return books;
});

app.MapGet("/books/{title}", (string title) =>
    titles.Keys.FirstOrDefault(t => t == title)
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
