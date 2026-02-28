using System;

namespace TravelBooking
{
    public interface ICostCalculationStrategy
    {
        double CalculateCost(double distance, int passengers, string serviceClass, bool hasDiscount);
    }

    public class AirplaneCostStrategy : ICostCalculationStrategy
    {
        public double CalculateCost(double distance, int passengers, string serviceClass, bool hasDiscount)
        {
            double basePrice = distance * (serviceClass == "Бизнес" ? 0.5 : 0.3);
            double discount = hasDiscount ? 0.1 : 0.0;
            return basePrice * passengers * (1 - discount);
        }
    }

    public class TrainCostStrategy : ICostCalculationStrategy
    {
        public double CalculateCost(double distance, int passengers, string serviceClass, bool hasDiscount)
        {
            double basePrice = distance * (serviceClass == "Бизнес" ? 0.2 : 0.1);
            double discount = hasDiscount ? 0.05 : 0.0;
            return basePrice * passengers * (1 - discount);
        }
    }

    public class BusCostStrategy : ICostCalculationStrategy
    {
        public double CalculateCost(double distance, int passengers, string serviceClass, bool hasDiscount)
        {
            double basePrice = distance * 0.05;
            double discount = hasDiscount ? 0.02 : 0.0;
            return basePrice * passengers * (1 - discount);
        }
    }

    public class TravelBookingContext
    {
        private ICostCalculationStrategy _strategy;

        public void SetStrategy(ICostCalculationStrategy strategy)
        {
            _strategy = strategy;
        }

        public double CalculateCost(double distance, int passengers, string serviceClass, bool hasDiscount)
        {
            if (_strategy == null)
                throw new Exception("Стратегия расчета стоимости не выбрана!");
            return _strategy.CalculateCost(distance, passengers, serviceClass, hasDiscount);
        }
    }

    class Program
    {
        static void Main()
        {
            var context = new TravelBookingContext();

            context.SetStrategy(new AirplaneCostStrategy());
            double cost = context.CalculateCost(1000, 2, "Эконом", true);
            Console.WriteLine($"Стоимость поездки на самолете: {cost:F2} руб.");

            context.SetStrategy(new TrainCostStrategy());
            cost = context.CalculateCost(1000, 2, "Бизнес", false);
            Console.WriteLine($"Стоимость поездки на поезде: {cost:F2} руб.");

            context.SetStrategy(new BusCostStrategy());
            cost = context.CalculateCost(500, 3, "Эконом", true);
            Console.WriteLine($"Стоимость поездки на автобусе: {cost:F2} руб.");

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
