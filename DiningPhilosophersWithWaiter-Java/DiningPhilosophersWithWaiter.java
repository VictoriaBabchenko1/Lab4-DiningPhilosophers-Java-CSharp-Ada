import java.util.concurrent.Semaphore;

public class DiningPhilosophersWithWaiter {
    public static void main(String[] args) {
        Fork[] forks = new Fork[5];
        Philosopher[] philosophers = new Philosopher[5];

        // Створення виделок
        for (int i = 0; i < 5; i++) {
            forks[i] = new Fork(i);
        }

        // Семaфор-офіціант: максимум 4 філософи можуть одночасно намагатися їсти
        Semaphore waiter = new Semaphore(4);

        // Створення філософів
        for (int i = 0; i < 5; i++) {
            Fork left = forks[i];
            Fork right = forks[(i + 1) % 5];
            philosophers[i] = new Philosopher(i, left, right, waiter);
            philosophers[i].start();
        }
    }
}

class Fork {
    private final int id;

    public Fork(int id) {
        this.id = id;
    }

    public int getId() {
        return id;
    }
}

class Philosopher extends Thread {
    private final int id;
    private final Fork leftFork;
    private final Fork rightFork;
    private final Semaphore waiter;

    public Philosopher(int id, Fork left, Fork right, Semaphore waiter) {
        this.id = id;
        this.leftFork = left;
        this.rightFork = right;
        this.waiter = waiter;
    }

    public void run() {
        try {
            for (int i = 0; i < 10; i++) {
                think();
                waiter.acquire(); // запит дозволу у "офіціанта"
                synchronized (leftFork) {
                    synchronized (rightFork) {
                        eat();
                    }
                }
                waiter.release(); // звільнення дозволу після їжі
            }
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    private void think() {
        System.out.println("Philosopher " + id + " is thinking.");
        sleepRandom();
    }

    private void eat() {
        System.out.println("Philosopher " + id + " is eating using forks " +
            leftFork.getId() + " and " + rightFork.getId() + ".");
        sleepRandom();
    }

    private void sleepRandom() {
        try {
            Thread.sleep((long) (Math.random() * 100));
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
