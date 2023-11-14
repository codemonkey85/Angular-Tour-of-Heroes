var myAllowSpecificOrigins = "myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()

    .AddCors(options => options.AddPolicy(
        name: myAllowSpecificOrigins,
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

List<Hero> heroes =
[
    new(Id: 11, Name: "Dr Nice"),
    new(Id: 12, Name: "Narco"),
    new(Id: 13, Name: "Bombasto"),
    new(Id: 14, Name: "Celeritas"),
    new(Id: 15, Name: "Magneta"),
    new(Id: 16, Name: "RubberMan"),
    new(Id: 17, Name: "Dynama"),
    new(Id: 18, Name: "Dr IQ"),
    new(Id: 19, Name: "Magma"),
    new(Id: 20, Name: "Tornado"),
];

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
