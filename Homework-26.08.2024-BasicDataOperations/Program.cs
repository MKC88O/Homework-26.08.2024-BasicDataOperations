using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
namespace Homework_26._08._2024_BasicDataOperations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            string? connectionString = config.GetConnectionString("DefaultConnection");
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            //var options = optionsBuilder.UseSqlServer("Server=DESKTOP-M496S5I;Database=testdb;Trusted_Connection=True; TrustServerCertificate=True;").Options;
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            using (ApplicationContext db = new ApplicationContext(options))
            {
                //    db.Database.EnsureDeleted();
                //    db.Database.EnsureCreated();

                Train train = new Train
                {
                    TrainNumber = "123A",
                    DepartureStation = "Odessa",
                    ArrivalStation = "Kiev",
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now.AddHours(5)
                };

                train = db.GetTrain(1);
                if (train != null)
                {
                    Console.WriteLine("Train: " + train.TrainNumber + " " + train.DepartureStation + " - " + train.ArrivalStation);
                }
                else
                {
                    Console.WriteLine("Train not found");
                }

                //db.AddTrain(train);


                //db.DeleteTrain(4);

                //train = db.Trains.FirstOrDefault(t => t.Id == 3);
                //if (train != null)
                //{
                //    train.TrainNumber = "1234A";
                //    db.UpdateTrain(train);
                //}
            }
        }
    }

    public class Train
    {
        public int Id { get; set; }
        public string? TrainNumber { get; set; }
        public string? DepartureStation { get; set; }
        public string? ArrivalStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

    class ApplicationContext : DbContext
    {
        public ApplicationContext() 
        {
        }
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Train> Trains { get; set; }

        public Train GetTrain(int id)
        {
            return Trains.FirstOrDefault(t => t.Id == id);
        }

        public void AddTrain(Train train)
        {
            Trains.Add(train);
            SaveChanges(); 
        }

        public void DeleteTrain(int id)
        {
            var train = Trains.FirstOrDefault(t => t.Id == id);
            if (train != null)
            {
                Trains.Remove(train);
                SaveChanges();
            }
        }

        public void UpdateTrain(Train newTrain)
        {
            Train? train = Trains.FirstOrDefault(t => t.Id == newTrain.Id);
            if (train != null)
            {
                train.TrainNumber = newTrain.TrainNumber;
                train.DepartureStation = newTrain.DepartureStation;
                train.ArrivalStation = newTrain.ArrivalStation;
                train.DepartureTime = newTrain.DepartureTime;
                train.ArrivalTime = newTrain.ArrivalTime;
                SaveChanges();
            }
        }

    }
}
