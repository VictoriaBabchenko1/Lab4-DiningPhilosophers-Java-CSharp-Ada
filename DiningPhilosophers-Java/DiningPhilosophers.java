public class DiningPhilosophers {
    public static void main(String[] args) {
        Fork[] forks = new Fork[5];
        Philosopher[] philosophers = new Philosopher[5];

        // Створення виделок
        for (int i = 0; i < 5; i++) {
            forks[i] = new Fork(i);
        }

        // Створення і запуск філософів
        for (int i = 0; i < 5; i++) {
            Fork left = forks[i];
            Fork right = forks[(i + 1) % 5];

            if (i == 4) { // Один філософ бере виделки у зворотному порядку
                philosophers[i] = new Philosopher(i, right, left);
            } else {
                philosophers[i] = new Philosopher(i, left, right);
            }

            philosophers[i].start();
        }

        // Очікування завершення роботи всіх потоків
        for (Philosopher philosopher : philosophers) {
            try {
                philosopher.join();
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }

        System.out.println("All philosophers have finished eating.");
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

    // синхронізований метод взяття виделки
    public synchronized void pickUp(int philosopherId) {
        System.out.println("Philosopher " + philosopherId + " picked up fork " + id);
    }

    // синхронізований метод покладання виделки
    public synchronized void putDown(int philosopherId) {
        System.out.println("Philosopher " + philosopherId + " put down fork " + id);
    }
}

class Philosopher extends Thread {
    private final int id;
    private final Fork left;
    private final Fork right;

    public Philosopher(int id, Fork left, Fork right) {
        this.id = id;
        this.left = left;
        this.right = right;
    }

    public void run() {
        for (int i = 0; i < 10; i++) {
            think();
            eat();
        }
    }

    private void think() {
        System.out.println("Philosopher " + id + " is thinking.");
        sleepRandom();
    }

    private void eat() {
        synchronized (left) {
            left.pickUp(id);
            synchronized (right) {
                right.pickUp(id);

                System.out.println("Philosopher " + id + " is eating using forks " +
                                   left.getId() + " and " + right.getId() + ".");
                sleepRandom();

                right.putDown(id);
            }
            left.putDown(id);
        }
    }

    private void sleepRandom() {
        try {
            Thread.sleep((long) (Math.random() * 100));
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}
