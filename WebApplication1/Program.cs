using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1;
//initialize
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


//example database
var animals = new List<Animal>();
var visits = new List<Visit>();

animals.Add(new Animal(1, "greg", "dog", 10, "red"));
animals.Add(new Animal(2, "bob", "cat", 11, "green"));
animals.Add(new Animal(3, "nick", "parrot", 20, "orange"));
animals.Add(new Animal(4, "demon", "lizzard", 21, "yellow"));
visits.Add(new Visit(1, 1, new DateTime(2023,1,3), "glowa", 15));
visits.Add(new Visit(2, 2, new DateTime(2023,1,3), "lapa", 16));
visits.Add(new Visit(3, 3, new DateTime(2023,1,3), "spina", 17));
visits.Add(new Visit(4, 4, new DateTime(2023,1,3), "brzuh", 18));

//logika dzialania MinimalAPI
app.MapGet("/animals", () => Results.Ok(animals));
app.MapGet("/animals/{id:int}", (int id) => FindAnimalById(animals, id));
app.MapPost("/animals", (Animal animal) => AddAnimal(animals, animal));
app.MapPut("/animals/{id:int}", (int id, Animal animal) => UpdateAnimal(animals, id, animal));
app.MapDelete("/animals/{id:int}", (int id) => DeleteAnimal(animals, id));
app.MapGet("/animals/{id:int}/visits", (int id) => GetVisitsByAnimal(visits, id));
app.MapPost("/visits", (Visit visit) => AddVisit(visits, visit));

app.Run();

//dokladna logika
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
        


