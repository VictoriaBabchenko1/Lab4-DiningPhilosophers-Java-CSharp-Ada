using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Fork[] forks = new Fork[5];
        Philosopher[] philosophers = new Philosopher[5];

        // Створення виделок
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new Fork(i);
        }

        // Семафор-офіціант: максимум 4 філософи можуть їсти одночасно
        SemaphoreSlim waiter = new SemaphoreSlim(4);

        // Створення філософів
        for (int i = 0; i < 5; i++)
        {
            Fork left = forks[i];
            Fork right = forks[(i + 1) % 5];
            philosophers[i] = new Philosopher(i, left, right, waiter);
            new Thread(philosophers[i].Dine).Start();
        }
    }
}

class Fork
{
    public int Id { get; private set; }

    public Fork(int id)
    {
        Id = id;
    }
}

class Philosopher
{
    private int id;
    private Fork leftFork;
    private Fork rightFork;
    private SemaphoreSlim waiter;

    private static readonly object _lock = new object();

    public Philosopher(int id, Fork left, Fork right, SemaphoreSlim waiter)
    {
        this.id = id;
        this.leftFork = left;
        this.rightFork = right;
        this.waiter = waiter;
    }

    public void Dine()
    {
        for (int i = 0; i < 10; i++)
        {
            Think();

            waiter.Wait(); // Запит дозволу у "офіціанта"

            lock (leftFork)
            {
                lock (rightFork)
                {
                    Eat();
                }
            }

            waiter.Release(); // Звільнення дозволу після їжі
        }
    }

    private void Think()
    {
        Console.WriteLine($"Philosopher {id} is thinking.");
        Thread.Sleep(new Random().Next(100));
    }

    private void Eat()
    {
        Console.WriteLine($"Philosopher {id} is eating using forks {leftFork.Id} and {rightFork.Id}.");
        Thread.Sleep(new Random().Next(100));
    }
}
