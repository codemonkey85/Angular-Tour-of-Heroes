var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          // builder.WithOrigins("http://localhost:4200");
                          builder.AllowAnyOrigin();
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

Hero GetHero(int id)
{
    return heroes.FirstOrDefault(hero => hero.id == id);
}

void CreateHero(Hero hero)
{
}

void UpdateHero(Hero hero)
{
}

void DeleteHero(int id)
{
}

app.MapGet("/heroes", GetHeroes)
    .WithName(nameof(GetHeroes));
app.MapGet("/heroes/{id}", GetHero)
    .WithName(nameof(GetHero));
app.MapPost("/heroes", CreateHero)
    .WithName(nameof(CreateHero));
app.MapPut("/heroes", UpdateHero)
    .WithName(nameof(UpdateHero));
app.MapDelete("/heroes", DeleteHero)
    .WithName(nameof(DeleteHero));

app.Run();

record Hero(int id, string name);
