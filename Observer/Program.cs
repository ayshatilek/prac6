using System;
using System.Collections.Generic;

namespace StockTrading
{
    public interface IObserver
    {
        void Update(string stockSymbol, double newPrice);
    }

    public interface ISubject
    {
        void Attach(IObserver observer, string stockSymbol);
        void Detach(IObserver observer, string stockSymbol);
        void Notify(string stockSymbol);
    }

    public class StockExchange : ISubject
    {
        private Dictionary<string, double> _stocks = new Dictionary<string, double>();
        private Dictionary<string, List<IObserver>> _observers = new Dictionary<string, List<IObserver>>();

        public void SetPrice(string stockSymbol, double price)
        {
            _stocks[stockSymbol] = price;
            Console.WriteLine($"\nЦена акции {stockSymbol} установлена: {price}");
            Notify(stockSymbol);
        }

        public double GetPrice(string stockSymbol) => _stocks.ContainsKey(stockSymbol) ? _stocks[stockSymbol] : 0;

        public void Attach(IObserver observer, string stockSymbol)
        {
            if (!_observers.ContainsKey(stockSymbol))
                _observers[stockSymbol] = new List<IObserver>();
            _observers[stockSymbol].Add(observer);
            Console.WriteLine($"{observer.GetType().Name} подписан на {stockSymbol}");
        }

        public void Detach(IObserver observer, string stockSymbol)
        {
            if (_observers.ContainsKey(stockSymbol))
                _observers[stockSymbol].Remove(observer);
        }

        public void Notify(string stockSymbol)
        {
            if (_observers.ContainsKey(stockSymbol))
            {
                foreach (var observer in _observers[stockSymbol])
                    observer.Update(stockSymbol, _stocks[stockSymbol]);
            }
        }
    }

    public class Trader : IObserver
    {
        public void Update(string stockSymbol, double newPrice)
        {
            Console.WriteLine($"Трейдер: Акция {stockSymbol} изменилась. Новая цена: {newPrice}");
        }
    }

    public class TradingRobot : IObserver
    {
        public void Update(string stockSymbol, double newPrice)
        {
            if (newPrice > 100)
                Console.WriteLine($"Робот: Продать {stockSymbol} по цене {newPrice}");
            else
                Console.WriteLine($"Робот: Купить {stockSymbol} по цене {newPrice}");
        }
    }

    class Program
    {
        static void Main()
        {
            var exchange = new StockExchange();

            var trader = new Trader();
            var robot = new TradingRobot();

            exchange.Attach(trader, "AAPL");
            exchange.Attach(robot, "AAPL");
            exchange.Attach(robot, "MSFT");

            exchange.SetPrice("AAPL", 120);
            exchange.SetPrice("MSFT", 90);

            exchange.Detach(robot, "AAPL");
            exchange.SetPrice("AAPL", 130);

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
