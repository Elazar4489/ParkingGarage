namespace ParkingGarage
{
    class Program
    {
        static void Main()
        {
            ParkingManager manager = new ParkingManager();
            bool flag = true;
            while (flag)
            {
                Console.Write("1. create car\n 2. enter car\n 3 exit car\n 4. exit");
                string choice = Console.ReadLine();
                if (choice == "1") manager.CreateCar();
                else if (choice == "2") { Console.Write("enter id"); manager.CarEntry(Console.ReadLine()); }
                else if (choice == "3") { Console.Write("enter id"); manager.CarExit(Console.ReadLine()); }
                else if (choice == "4") flag = false;
            }
        }
    }
    interface ICarLogger { void WriteLogger(string text); }
    interface IPreOrder { void Order(DateTime time); }
    interface IPaymentTransaction { void Payment(); }
    interface ConsolCarLogger : ICarLogger { }
    interface CreditPayment : IPaymentTransaction { }
    interface CashPayment : IPaymentTransaction { }

    class ParkingManager
    {
        public List<Car> cars = new List<Car>();
        public void CreateCar() 
        {
            Console.Write("Enter the id: ");
            string id = Console.ReadLine();
            if (! chack(id))
            {
                Console.Write("1. regular\n 2. disabled\n 3. motorcycle");
                string typeCar = Console.ReadLine();
                if (typeCar == "1") cars.Add(new RegularCar(id));
                else if (typeCar == "2") cars.Add(new DisabledCar(id));
                else if (typeCar == "3") cars.Add(new DisabledCar(id));
            }
            
        }
        public void CarEntry(string id)
        {
            foreach (Car car in cars) { if (car.Id == id) car.Enter(); }
        }
        public void CarExit(string id)
        {
            foreach (Car car in cars) 
            { 
                if (car.Id == id) car.Exit();
                CalculatingAmount(car);
                Payment(car);
                cars.Remove(car);
            }
        }
        public void CalculatingAmount(Car car)
        {
            double amount = car.rate.Calculate(car);
            car.AmountDebt = amount;
        }
        public void Payment(Car car)
        {
            car.AmountDebt = 0;
        }
        public void PreOrder( Car car, DateTime time)
        {
            if (car is RegularCar || car is DisabledCar)
            {
                car.Order();
            }
            else Console.WriteLine("thecar is not support in this option");
        }
        public bool chack(string id)
        {
            foreach (Car car in cars)
            {
                if (car.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
    abstract class Car
    {
        public RateCalculator rate;
        public string Id;
        public DateTime EntryTime;
        public DateTime ExitTime;
        public double AmountDebt;
        public Car(string id) { Id = id; }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Pay();
        public void Order() { }
    }
    class RegularCar : Car, IPreOrder
    {
        public RateCalculator rate = new RegularRateCalculator();
        public RegularCar(string id) : base(id)
        {
            Id = id;
        }
        public override void Enter()
        {
            EntryTime = DateTime.Now;
        }
        public override void Exit()
        {
            ExitTime = DateTime.Now;
        }
        public override void Pay()
        {
            AmountDebt = 0;
        }
        public void Order(DateTime time)
        {
            EntryTime = time;
        }

    }

    class DisabledCar : Car, IPreOrder
    {
        public RateCalculator rate = new DisabledRateCalculator();
        public DisabledCar(string id) : base(id)
        {
            Id = id;
        }
        public override void Enter()
        {
            EntryTime = DateTime.Now;
        }
        public override void Exit()
        {
            ExitTime = DateTime.Now;
        }
        public override void Pay()
        {
            AmountDebt = 0;
        }
        public void Order(DateTime time)
        {
            EntryTime = time;
        }

    }

    class Motorcycle : Car
    {
        public RateCalculator rate = new MotorcycleRateCalculator();
        public Motorcycle(string id) : base(id)
        {
            Id = id;
        }
        public override void Enter()
        {
            EntryTime = DateTime.Now;
        }
        public override void Exit()
        {
            ExitTime = DateTime.Now;
        }
        public override void Pay()
        {
            AmountDebt = 0;
        }

    }

    abstract class RateCalculator
    {
        public abstract double Calculate(Car car);
    }
    class RegularRateCalculator: RateCalculator
    {
        public override double Calculate(Car car)
        {
            TimeSpan duration = car.ExitTime - car.EntryTime;
            double amount = duration.TotalHours * 15;
            return duration.TotalHours;
        }
    }
    class DisabledRateCalculator : RateCalculator
    {
        public override double Calculate(Car car)
        {
            TimeSpan duration = car.ExitTime - car.EntryTime;
            double amount = duration.TotalHours * 8;
            return duration.TotalHours;
        }
    }
    class MotorcycleRateCalculator : RateCalculator
    {
        public override double Calculate(Car car)
        {
            TimeSpan duration = car.ExitTime - car.EntryTime;
            double amount = duration.TotalHours * 5;
            return duration.TotalHours;
        }
    }

}