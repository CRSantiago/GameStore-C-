namespace GameStore.Endpoints;
using GameStore.Dtos;

public static class GamesEndpoints
{
const string GetGameEndpointName = "GetGame";   // Define the endpoint name

private static readonly List<GameDto> games = [
    new GameDto(1, "The Witcher 3: Wild Hunt", "RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new GameDto(2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new GameDto(3, "Doom Eternal", "FPS", 39.99m, new DateOnly(2020, 3, 20)),
    new GameDto(4, "Hades", "Roguelike", 24.99m, new DateOnly(2020, 9, 17)),
    new GameDto(5, "Celeste", "Platformer", 19.99m, new DateOnly(2018, 1, 25))
];

public static RouteGroupBuilder MapGameEndpoints(this WebApplication app) {

    var group = app.MapGroup("/games").WithParameterValidation();

    // GET /games
    group.MapGet("/", () => games);

    // GET /games/{id}
    group.MapGet("/{id}", (int id) => {
        GameDto? game = games.Find(game => game.Id == id);

        return game is null ? Results.NotFound() : Results.Ok(game);
        
        })
    .WithName(GetGameEndpointName);

    // POST /games
    group.MapPost("/", (CreateGameDto newGame) => {
        GameDto game = new (games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
        
        games.Add(game);

        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
    });

    // PUT /games/{id}
    group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
        var index = games.FindIndex(game => game.Id == id);

        if (index == -1) {
            return Results.NotFound();
        }

        games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);

        return Results.NoContent();
    });

    // DELETE /games/{id}
    group.MapDelete("/{id}", (int id) => {
        var index = games.FindIndex(game => game.Id == id);

        if (index == -1) {
            return Results.NotFound();
        }

        games.RemoveAt(index);

        return Results.NoContent();
    });
    return group;
}
}
