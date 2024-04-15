namespace WebApplication1;

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