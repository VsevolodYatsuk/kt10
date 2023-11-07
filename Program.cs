using System;
using System.Collections.Generic;

namespace CustomCode
{
    public interface IClonable<T> where T : IClonable<T>
    {
        T Replicate();
    }

    public class Dot : IClonable<Dot>
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Dot(int x, int y)
        {
            XPosition = x;
            YPosition = y;
        }

        public Dot(Dot other)
        {
            XPosition = other.XPosition;
            YPosition = other.YPosition;
        }

        public Dot Replicate()
        {
            return new Dot(this);
        }
    }

    public class Quadrilateral : IClonable<Quadrilateral>
    {
        public Dot UpperLeft { get; set; }
        public Dot LowerRight { get; set; }

        public Quadrilateral(Dot upperLeft, Dot lowerRight)
        {
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }

        public Quadrilateral(Quadrilateral other)
        {
            UpperLeft = new Dot(other.UpperLeft);
            LowerRight = new Dot(other.LowerRight);
        }

        public Quadrilateral Replicate()
        {
            return new Quadrilateral(this);
        }
    }

    public interface IComparer<T> where T : struct
    {
        int Assess(T x, T y);
    }

    public struct RealNumber : IComparer<RealNumber>
    {
        public double ActualPart { get; set; }
        public double ImaginedPart { get; set; }

        public RealNumber(double actual, double imagined)
        {
            ActualPart = actual;
            ImaginedPart = imagined;
        }

        public int Assess(RealNumber x, RealNumber y)
        {
            double modulusX = Math.Sqrt(x.ActualPart * x.ActualPart + x.ImaginedPart * x.ImaginedPart);
            double modulusY = Math.Sqrt(y.ActualPart * y.ActualPart + y.ImaginedPart * y.ImaginedPart);

            if (modulusX < modulusY)
                return -1;
            else if (modulusX > modulusY)
                return 1;
            else
                return 0;
        }
    }

    public struct DecimalNumber : IComparer<DecimalNumber>
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public DecimalNumber(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.");
            Numerator = numerator;
            Denominator = denominator;
        }

        public int Assess(DecimalNumber x, DecimalNumber y)
        {
            double valueX = (double)x.Numerator / x.Denominator;
            double valueY = (double)y.Numerator / y.Denominator;

            if (valueX < valueY)
                return -1;
            else if (valueX > valueY)
                return 1;
            else
                return 0;
        }
    }

    public interface IStorage<T> where T : IItem
    {
        void Include(T item);
        void Exclude(T item);
        T LocateById(int id);
        IEnumerable<T> ObtainAll();
    }

    public interface IItem
    {
        int Identifier { get; }
    }

    public class Item : IItem
    {
        public int Identifier { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Item(int id, string name, decimal price)
        {
            Identifier = id;
            Name = name;
            Price = price;
        }
    }

    public class Client : IItem
    {
        public int Identifier { get; }
        public string Name { get; set; }
        public string Address { get; set; }

        public Client(int id, string name, string address)
        {
            Identifier = id;
            Name = name;
            Address = address;
        }
    }

    public class ItemStore : IStorage<Item>
    {
        private List<Item> itemList = new List<Item>();

        public void Include(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            itemList.Add(item);
        }

        public void Exclude(Item item)
        {
            itemList.Remove(item);
        }

        public Item LocateById(int id)
        {
            return itemList.Find(i => i.Identifier == id);
        }

        public IEnumerable<Item> ObtainAll()
        {
            return itemList;
        }
    }

    public class ClientRegistry : IStorage<Client>
    {
        private List<Client> clientList = new List<Client>();

        public void Include(Client item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            clientList.Add(item);
        }

        public void Exclude(Client item)
        {
            clientList.Remove(item);
        }

        public Client LocateById(int id)
        {
            return clientList.Find(c => c.Identifier == id);
        }

        public IEnumerable<Client> ObtainAll()
        {
            return clientList;
        }
    }

    class CustomCode
    {
        public T CloneObject<T>(T original) where T : IClonable<T>
        {
            return original.Replicate();
        }

        static void Main(string[] args)
        {
            var itemStore = new ItemStore();
            var clientRegistry = new ClientRegistry();

            itemStore.Include(new Item(1, "Friend", 1));
            itemStore.Include(new Item(2, "Liver", 70000));
            itemStore.Include(new Item(3, "Buy a job on c#", 99999999));

            clientRegistry.Include(new Client(1, "Johnson", "Maple Street"));
            clientRegistry.Include(new Client(2, "Smith", "Elm Street"));
            clientRegistry.Include(new Client(3, "Williams", "Oak Street"));

            Console.WriteLine("Items:");
            foreach (var item in itemStore.ObtainAll())
            {
                Console.WriteLine($"ID: {item.Identifier}, Item Name: {item.Name}, Item Price: {item.Price}");
            }

            Console.WriteLine("\nClients:");
            foreach (var client in clientRegistry.ObtainAll())
            {
                Console.WriteLine($"ID: {client.Identifier}, Client Name: {client.Name}, Client Address: {client.Address}");
            }

            int itemIdToFind = 2;
            var foundItem = itemStore.LocateById(itemIdToFind);
            if (foundItem != null)
            {
                Console.WriteLine($"\nItem with ID {itemIdToFind}: {foundItem.Name}");
            }
            else
            {
                Console.WriteLine($"\nItem with ID {itemIdToFind} not found.");
            }

            int clientIdToFind = 3;
            var foundClient = clientRegistry.LocateById(clientIdToFind);
            if (foundClient != null)
            {
                Console.WriteLine($"\nClient with ID {clientIdToFind}: {foundClient.Name}");
            }
            else
            {
                Console.WriteLine($"\nClient with ID {clientIdToFind} not found.");
            }

            Console.WriteLine("-------------------------------------");

            var customCode = new CustomCode();

            Dot dot1 = new Dot(1, 2);
            Dot dot2 = customCode.CloneObject(dot1);

            Quadrilateral quadrilateral1 = new Quadrilateral(new Dot(1, 2), new Dot(3, 4));
            Quadrilateral quadrilateral2 = customCode.CloneObject(quadrilateral1);

            Console.WriteLine($"Copied Dot: ({dot2.XPosition}, {dot2.YPosition})");
            Console.WriteLine($"Copied Quadrilateral: Upper Left - ({quadrilateral2.UpperLeft.XPosition}, {quadrilateral2.UpperLeft.YPosition}), Lower Right - ({quadrilateral2.LowerRight.XPosition}, {quadrilateral2.LowerRight.YPosition})");

            Console.WriteLine("-------------------------------------");

            RealNumber real1 = new RealNumber(3, 4);
            RealNumber real2 = new RealNumber(1, 2);

            DecimalNumber decimal1 = new DecimalNumber(3, 5);
            DecimalNumber decimal2 = new DecimalNumber(1, 3);

            IComparer<RealNumber> realComparer = new RealNumber();
            IComparer<DecimalNumber> decimalComparer = new DecimalNumber();

            int realComparison = realComparer.Assess(real1, real2);
            int decimalComparison = decimalComparer.Assess(decimal1, decimal2);

            Console.WriteLine("Comparison of Real Numbers: " + realComparison);
            Console.WriteLine("Comparison of Decimal Numbers: " + decimalComparison);
        }
    }
}
