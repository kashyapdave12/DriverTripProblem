using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DriverTripProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            string textFile = "../InputText.txt";
            string[] lines = File.ReadAllLines(textFile);
            List<Driver> lstDriver = new List<Driver>();
            List<Trip> lstTrip = new List<Trip>();
            Console.WriteLine("Reading the Input File: ");
            foreach (var item in lines)
            {
                if (item.Split(" ")[0].ToLower().Equals("driver"))
                {
                    if (item.Split(" ").Length == 2)
                    {
                        lstDriver.Add(new Driver(item.Split(" ")[0], item.Split(" ")[1]));
                    }
                    else
                    {
                        Console.WriteLine("Data cannot be inserted " + item);
                    }
                }
                if (item.Split(" ")[0].ToLower().Equals("trip"))
                {
                    if (item.Split(" ").Length == 5)
                    {
                        if (lstDriver.Any(x => x.driverName == item.Split(" ")[1]))
                        {
                            lstTrip.Add(new Trip(item.Split(" ")[0], item.Split(" ")[1], item.Split(" ")[2], item.Split(" ")[3], float.Parse(item.Split(" ")[4])));
                        }
                        else
                        {
                            Console.WriteLine("Date cannot be inserted as the driver does not exist " + item);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Data cannot be inserted " + item);
                    }
                }

            }
            Console.WriteLine();
            Console.WriteLine("Input Drivers: ");

            foreach (var item in lstDriver)
            {
                Console.WriteLine(item.driverId + "-->" + item.driverName);
            }
            Console.WriteLine("Trip Information: ");
            foreach (var item in lstTrip)
            {
                Console.WriteLine(item.tripId + "-->" + item.driverName + "-->" + item.tripStartTime + "-->" + item.tripEndTime + "-->" + item.milesDriven);
            }

            Console.WriteLine();
            Console.WriteLine("Output: ");

            CalculateTrip.TripCalculation(lstTrip);
        }

    }
    class Driver
    {
        public string driverId;
        public string driverName;
        public Driver(string driverId, string driverName)
        {
            this.driverId = driverId;
            this.driverName = driverName;
        }

    }
    class Trip
    {
        public string tripId;
        public string tripStartTime;
        public string tripEndTime;
        public float milesDriven;
        public string driverName;

        public Trip(string tripId, string driverName, string tripStartTime, string tripEndTime, float milesDriven)
        {
            this.tripId = tripId;
            this.tripStartTime = tripStartTime;
            this.tripEndTime = tripEndTime;
            this.milesDriven = milesDriven;
            this.driverName = driverName;
        }

    }

    class Output
    {
        public Output() { }
        public string driverName;
        public float miles;
        public double timeSpan;

    }

    class CalculateTrip
    {
        internal static void TripCalculation(List<Trip> lstTrip)
        {
            List<Output> result = lstTrip
             .GroupBy(l => l.driverName)
             .Select(cl => new Output
             {
                 driverName = cl.First().driverName,
                 miles = cl.Sum(c => c.milesDriven),
                 timeSpan = cl.Sum(d => DateTime.Parse(d.tripEndTime).Subtract(DateTime.Parse(d.tripStartTime)).TotalMinutes),

             }).ToList();

            var result1 = result.OrderBy(s => s.driverName);

            foreach (var item in result1)
            {

                if (Math.Round((item.miles * 60 / item.timeSpan), 0) > 100 || Math.Round((item.miles * 60 / item.timeSpan), 0) < 5)
                {
                    Console.WriteLine("Trip Discared as average speed is not in accepatable bucket for driver " + item.driverName);
                }

                else
                {
                    Console.WriteLine(item.driverName + " -- " + Math.Round(item.miles, 0) + " @ " + Math.Round((item.miles * 60 / item.timeSpan), 0) + "mph");
                }
            }
        }
    }
}

