public class DiningPhilosophers {
    public static void main(String[] args) {
        Fork[] forks = new Fork[5];
        Philosopher[] philosophers = new Philosopher[5];

        for (int i = 0; i < 5; i++) {
            forks[i] = new Fork(i);
        }

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
    }
}

class Fork {
    private final int id;

    public Fork(int id) {
        this.id = id;
    }

    public synchronized void pickUp() {}
    public synchronized void putDown() {}
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
    }

    private void eat() {
        synchronized (left) {
            synchronized (right) {
                System.out.println("Philosopher " + id + " is eating.");
            }
        }
    }
}
