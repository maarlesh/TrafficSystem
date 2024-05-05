using System;
using System.Threading;

class TrafficLight
{
    private Semaphore intersectionSemaphore;

    private const int GREEN_LIGHT_TIME = 5000;
    private const int RED_LIGHT_TIME = 3000;

    public TrafficLight(Semaphore semaphore)
    {
        intersectionSemaphore = semaphore;
    }

    public void ChangeState(string state)
    {
        Console.WriteLine($"Traffic light changed to {state}");
        Thread.Sleep(2000);
    }

    public void ControlTraffic()
    {
        while (true)
        {
            ChangeState("Green");
            intersectionSemaphore.Release(); 
            Thread.Sleep(GREEN_LIGHT_TIME);

            ChangeState("Red");
            Thread.Sleep(RED_LIGHT_TIME);
        }
    }
}

class Vehicle
{
    private int id;
    private Semaphore intersectionSemaphore;

    public Vehicle(int id, Semaphore semaphore)
    {
        this.id = id;
        intersectionSemaphore = semaphore;
    }

    public void PassThroughIntersection()
    {
        Console.WriteLine($"Vehicle {id} is approaching the intersection");
        intersectionSemaphore.WaitOne(); 
         Console.WriteLine($"Vehicle {id} is passing through the intersection");
        Thread.Sleep(2000);
        Console.WriteLine($"Vehicle {id} has passed through the intersection");
    }

}

class TrafficLigths
{
    static void Main(string[] args)
    {
        //capacity 2 vehicles
        Semaphore intersectionSemaphore = new Semaphore(2, 2); 

        TrafficLight trafficLight = new TrafficLight(intersectionSemaphore);
        Vehicle vehicle1 = new Vehicle(1, intersectionSemaphore);
        Vehicle vehicle2 = new Vehicle(2, intersectionSemaphore);

        Thread trafficLightThread = new Thread(trafficLight.ControlTraffic);
        trafficLightThread.Start();

        Thread vehicle1Thread = new Thread(vehicle1.PassThroughIntersection);
        Thread vehicle2Thread = new Thread(vehicle2.PassThroughIntersection);
        vehicle1Thread.Start();
        vehicle2Thread.Start();

        vehicle1Thread.Join();
        vehicle2Thread.Join();
        trafficLightThread.Join();

        Console.WriteLine("Simulation completed");
    }
}
