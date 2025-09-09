using System;
using System.Threading;

class Fork
{
    public readonly object Lock = new object();
    public int Id { get; }
    public Fork(int id) => Id = id;
}

class Philosopher
{
    private readonly int id;
    private readonly Fork left;
    private readonly Fork right;
    private readonly Random rnd = new Random();

    public Philosopher(int id, Fork left, Fork right)
    {
        this.id = id;
        this.left = left;
        this.right = right;
    }

    public void Dine()
    {
        for (int i = 0; i < 10; i++)
        {
            Think();

            // один філософ бере виделки у зворотному порядку
            if (id == 4)
                Eat(right, left);
            else
                Eat(left, right);
        }
        Console.WriteLine($"Philosopher {id} has finished dining.");
    }

    private void Think()
    {
        Console.WriteLine($"Philosopher {id} is thinking.");
        Thread.Sleep(rnd.Next(50, 200)); // симуляція часу на роздуми
    }

    private void Eat(Fork first, Fork second)
    {
        lock (first.Lock)
        {
            Console.WriteLine($"Philosopher {id} picked up fork {first.Id}");
            Thread.Sleep(20);

            lock (second.Lock)
            {
                Console.WriteLine($"Philosopher {id} picked up fork {second.Id}");
                Console.WriteLine($"Philosopher {id} is eating using forks {first.Id} and {second.Id}.");
                Thread.Sleep(rnd.Next(50, 200));
                Console.WriteLine($"Philosopher {id} put down fork {second.Id}");
            }

            Console.WriteLine($"Philosopher {id} put down fork {first.Id}");
        }
    }
}

class Program
{
    static void Main()
    {
        var forks = new Fork[5];
        for (int i = 0; i < 5; i++)
            forks[i] = new Fork(i);

        var philosophers = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int id = i;
            var left = forks[i];
            var right = forks[(i + 1) % 5];
            var phil = new Philosopher(id, left, right);
            philosophers[i] = new Thread(phil.Dine);
            philosophers[i].Start();
        }

        // ✅ Очікування завершення роботи всіх потоків
        foreach (var t in philosophers)
            t.Join();

        Console.WriteLine("All philosophers have finished dining.");
    }
}
