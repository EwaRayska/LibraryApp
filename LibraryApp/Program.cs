using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var titles = new[]
{
    "W pustyni i w puszczy",
    "Krzy¿acy",
    "Kubuœ Puchatek",
    "Lalka",
    "D¿uma",
    "Tango",
    "Akademia Pana Kleksa",
    "Harry Potter",
    "Hobbit",
    "Pan Tadeusz",
    "Balladyna"
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

app.MapGet("/books", () =>
{
    var books = Enumerable.Range(1, titles.Length).Select(index =>
    new Book
    (
            index,
            titles[(index - 1)]
        ))
    .ToArray();
    return books;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal record Book(int Id, string? Title)
{
}
