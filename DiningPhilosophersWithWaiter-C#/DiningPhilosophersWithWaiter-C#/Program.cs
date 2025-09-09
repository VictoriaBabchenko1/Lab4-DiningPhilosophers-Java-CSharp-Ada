using System;
using System.Threading;

class Program
{
    const int PhilosopherCount = 5;
    const int Iterations = 10;

    static void Main()
    {
        Fork[] forks = new Fork[PhilosopherCount];
        Philosopher[] philosophers = new Philosopher[PhilosopherCount];
        Thread[] threads = new Thread[PhilosopherCount];

        // Створення виделок
        for (int i = 0; i < PhilosopherCount; i++)
            forks[i] = new Fork(i);

        // Семафор-офіціант: максимум 4 філософи можуть їсти одночасно
        SemaphoreSlim waiter = new SemaphoreSlim(PhilosopherCount - 1);

        // Створення і запуск філософів
        for (int i = 0; i < PhilosopherCount; i++)
        {
            Fork left = forks[i];
            Fork right = forks[(i + 1) % PhilosopherCount];
            philosophers[i] = new Philosopher(i, left, right, waiter, Iterations);
            threads[i] = new Thread(philosophers[i].Dine);
            threads[i].Start();
        }

        // Чекаємо завершення всіх потоків
        foreach (var t in threads)
            t.Join();

        Console.WriteLine("All philosophers have finished.");
    }
}

class Fork
{
    public int Id { get; private set; }
    public Fork(int id) => Id = id;
}

class Philosopher
{
    private static readonly Random random = new Random();
    private readonly int id;
    private readonly Fork leftFork;
    private readonly Fork rightFork;
    private readonly SemaphoreSlim waiter;
    private readonly int iterations;

    public Philosopher(int id, Fork left, Fork right, SemaphoreSlim waiter, int iterations)
    {
        this.id = id;
        this.leftFork = left;
        this.rightFork = right;
        this.waiter = waiter;
        this.iterations = iterations;
    }

    public void Dine()
    {
        for (int i = 0; i < iterations; i++)
        {
            Think();

            waiter.Wait(); // Запит дозволу у "офіціанта"

            lock (leftFork)
            {
                Console.WriteLine($"Philosopher {id} picked up left fork {leftFork.Id}.");

                lock (rightFork)
                {
                    Console.WriteLine($"Philosopher {id} picked up right fork {rightFork.Id}.");
                    Eat();
                    Console.WriteLine($"Philosopher {id} put down right fork {rightFork.Id}.");
                }

                Console.WriteLine($"Philosopher {id} put down left fork {leftFork.Id}.");
            }

            waiter.Release(); // Звільнення дозволу після їжі
        }
    }

    private void Think()
    {
        Console.WriteLine($"Philosopher {id} is thinking.");
        Thread.Sleep(random.Next(100, 500));
    }

    private void Eat()
    {
        Console.WriteLine($"Philosopher {id} is eating using forks {leftFork.Id} and {rightFork.Id}.");
        Thread.Sleep(random.Next(100, 500));
    }
}
