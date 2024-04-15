namespace WebApplication1;

public class Visit {
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
    public Visit() {}
}