var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   );
});

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

IEnumerable<Hero> GetHeroes()
{
    return heroes;
}

Hero? GetHero(int id)
{
    return heroes.FirstOrDefault(hero => hero.id == id);
}

Hero CreateHero(Hero hero)
{
    var newHero = new Hero(
         heroes.Max(h => h.id) + 1,
         hero.name
    );
    heroes.Add(newHero);
    return newHero;
}

Hero? UpdateHero(Hero hero)
{
    var foundHero = GetHero(hero.id);
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

app.MapGet("/heroes", GetHeroes)
    .WithName(nameof(GetHeroes));
app.MapGet("/heroes/{id}", GetHero)
    .WithName(nameof(GetHero));
app.MapPost("/heroes", CreateHero)
    .WithName(nameof(CreateHero));
app.MapPut("/heroes", UpdateHero)
    .WithName(nameof(UpdateHero));
app.MapDelete("/heroes/{id}", DeleteHero)
    .WithName(nameof(DeleteHero));

app.Run();

record Hero(int id, string name);
