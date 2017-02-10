namespace ILoveAudi.Models
{
    public class Car
    {
        public Car(string name, int year, double price)
        {
            Name = name;
            Year = year;
            Price = price;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
      
    }
}