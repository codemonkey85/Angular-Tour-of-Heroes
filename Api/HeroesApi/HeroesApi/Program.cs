var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()

    .AddCors(options => options.AddPolicy(
        name: MyAllowSpecificOrigins,
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

var heroes = new List<Hero>
{
    new Hero(11, "Dr Nice"),
    new Hero(12, "Narco"),
    new Hero(13, "Bombasto"),
    new Hero(14, "Celeritas"),
    new Hero(15, "Magneta"),
    new Hero(16, "RubberMan"),
    new Hero(17, "Dynama"),
    new Hero(18, "Dr IQ"),
    new Hero(19, "Magma"),
    new Hero(20, "Tornado"),
};

IEnumerable<Hero> GetHeroes() => heroes;

Hero? GetHero(int id) => heroes.FirstOrDefault(hero => hero.Id == id);

Hero CreateHero(Hero hero)
{
    var newHero = new Hero(
         heroes.Max(h => h.Id) + 1,
         hero.Name
    );
    heroes.Add(newHero);
    return newHero;
}

Hero? UpdateHero(Hero hero)
{
    var foundHero = GetHero(hero.Id);
    if (foundHero is null)
    {
        return null;
    }
    var index = heroes.IndexOf(foundHero);
    heroes[index] = hero;
    return hero;
}

void DeleteHero(int id)
{
    var foundHero = GetHero(id);
    if (foundHero is not null)
    {
        heroes.Remove(foundHero);
    }
}

var heroesApiGroup = app.MapGroup("/heroes");

heroesApiGroup.MapGet("/", GetHeroes).WithName(nameof(GetHeroes));
heroesApiGroup.MapGet("/{id}", GetHero).WithName(nameof(GetHero));
heroesApiGroup.MapPost("/", CreateHero).WithName(nameof(CreateHero));
heroesApiGroup.MapPut("/", UpdateHero).WithName(nameof(UpdateHero));
heroesApiGroup.MapDelete("/{id}", DeleteHero).WithName(nameof(DeleteHero));

app.Run();

internal record Hero(int Id, string Name);
