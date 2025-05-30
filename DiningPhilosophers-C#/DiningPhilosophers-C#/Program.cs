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
            if (id == 4)
                Eat(right, left); // один філософ бере виделки у зворотному порядку
            else
                Eat(left, right);
        }
    }

    private void Think() => Console.WriteLine($"Philosopher {id} is thinking.");

    private void Eat(Fork first, Fork second)
    {
        lock (first.Lock)
        {
            lock (second.Lock)
            {
                Console.WriteLine($"Philosopher {id} is eating.");
            }
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
    }
}
