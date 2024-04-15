    using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var animals = new List<Animal>();
var visits = new List<Visit>();

app.MapGet("/animals", () => Results.Ok(animals));
app.MapGet("/animals/{id:int}", (int id) => FindAnimalById(animals, id));
app.MapPost("/animals", (Animal animal) => AddAnimal(animals, animal));
app.MapPut("/animals/{id:int}", (int id, Animal animal) => UpdateAnimal(animals, id, animal));
app.MapDelete("/animals/{id:int}", (int id) => DeleteAnimal(animals, id));
app.MapGet("/animals/{id:int}/visits", (int id) => GetVisitsByAnimal(visits, id));
app.MapPost("/visits", (Visit visit) => AddVisit(visits, visit));

app.Run();

IResult FindAnimalById(List<Animal> animals, int id) {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    return animal is null ? Results.NotFound($"Animal with ID {id} not found.") : Results.Ok(animal);
}

IResult AddAnimal(List<Animal> animals, Animal animal) {
    animal.Id = animals.Count + 1;
    animals.Add(animal);
    return Results.Created($"/animals/{animal.Id}", animal);
}

IResult UpdateAnimal(List<Animal> animals, int id, Animal updatedAnimal) {
    var index = animals.FindIndex(a => a.Id == id);
    if (index == -1)
        return Results.NotFound($"Animal with ID {id} not found.");
    updatedAnimal.Id = id;
    animals[index] = updatedAnimal;
    return Results.Ok(updatedAnimal);
}

IResult DeleteAnimal(List<Animal> animals, int id) {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal is null)
        return Results.NotFound($"Animal with ID {id} not found.");
    animals.Remove(animal);
    return Results.NoContent();
}

List<Visit> GetVisitsByAnimal(List<Visit> visits, int animalId) {
    return visits.Where(v => v.AnimalId == animalId).ToList();
}

IResult AddVisit(List<Visit> visits, Visit visit) {
    visit.Id = visits.Count + 1;
    visits.Add(visit);
    return Results.Created($"/visits/{visit.Id}", visit);
}
        

public class Animal {
    public Animal(int id, string name, string category, double weight, string furColor)
    {
        Id = id;
        Name = name;
        Category = category;
        Weight = weight;
        FurColor = furColor;
        
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public double Weight { get; set; }
    public string FurColor { get; set; }
}

public class Visit {
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public DateTime VisitDate { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Visit() {}
}