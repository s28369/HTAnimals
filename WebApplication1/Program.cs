    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;

    var builder = WebApplication.CreateBuilder(args);
    var animals = new List<Animal>();
    var visits = new List<Visit>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    // Endpoint do pobierania listy zwierząt
    app.MapGet("/animals", () =>
    {
        return Results.Ok(animals);
    });

    // Endpoint do pobierania konkretnego zwierzęcia po id
    app.MapGet("/animals/{id}", (int id) =>
    {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return Results.NotFound($"Zwierzę o id {id} nie zostało znalezione.");
        return Results.Ok(animal);
    });

    // Endpoint do dodawania zwierzęcia
    app.MapPost("/animals", (Animal animal) =>
    {
        animal.Id = animals.Count + 1;
        animals.Add(animal);
        return Results.Created($"/animals/{animal.Id}", animal);
    });

    // Endpoint do edycji zwierzęcia
    app.MapPut("/animals/{id}", (int id, Animal updatedAnimal) =>
    {
        var index = animals.FindIndex(a => a.Id == id);
        if (index == -1)
            return Results.NotFound($"Zwierzę o id {id} nie zostało znalezione.");
        
        updatedAnimal.Id = id;
        animals[index] = updatedAnimal;
        return Results.Ok(updatedAnimal);
    });

    // Endpoint do usuwania zwierzęcia
    app.MapDelete("/animals/{id}", (int id) =>
    {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null)
            return Results.NotFound($"Zwierzę o id {id} nie zostało znalezione.");
        
        animals.Remove(animal);
        return Results.NoContent();
    });

    // Endpoint do pobierania listy wizyt powiązanych z danym zwierzęciem
    app.MapGet("/animals/{id}/visits", (int id) =>
    {
        var animalVisits = visits.Where(v => v.AnimalId == id).ToList();
        return Results.Ok(animalVisits);
    });

    // Endpoint do dodawania nowych wizyt
    app.MapPost("/visits", (Visit visit) =>
    {
        visit.Id = visits.Count + 1;
        visits.Add(visit);
        return Results.Created($"/visits/{visit.Id}", visit);
    });

    app.Run();

    // Klasa reprezentująca zwierzę
    public class Animal
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Weight { get; set; }
        public string FurColor { get; set; }
    }

    // Klasa reprezentująca wizytę
    public class Visit
    {
        public Visit()
        {
        }

        public Visit(int id, int animalId, DateTime visitDate, string description, decimal price)
        {
            Id = id;
            AnimalId = animalId;
            VisitDate = visitDate;
            Description = description;
            Price = price;
        }

        public int Id { get; set; }
        public int AnimalId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
