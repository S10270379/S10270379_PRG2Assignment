//==========================================================
// Student Number : S10270379
// Student Name : Ng Lee Meng
// Partner Name : How Shao Yang Louis
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace S10270379_PRG2Assignment
{
    class Airline
    {
        private string name;
        private string code;
        public string Name { get; set; }
        public string Code { get; set; }
        Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        public Dictionary<string, Flight> Flights
        { get { return flights; } set { flights = value; } }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }

        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights[flight.FlightNumber] = flight;
                return true;
            }
            return false;
        }

        public bool RemoveFlight(Flight flight)
        {
            return flights.Remove(flight.FlightNumber);
        }

        public bool HasFlight(string flightNumber)
        {
            return flights.ContainsKey(flightNumber);
        }

        public (double subtotalFees, double subtotalDiscounts, double finalFees) CalculateFees()
        {
            double totalFees = 0;
            double totalDiscounts = 0;

            foreach (var flight in flights.Values)
            {
                if (flight.Origin == "Singapore (SIN)")
                    totalFees += 800;
                else if (flight.Destination == "Singapore (SIN)")
                    totalFees += 500;

                totalFees += 300;

                if (flight.Origin == "DXB" || flight.Origin == "BKK" || flight.Origin == "NRT")
                    totalDiscounts += 25;

                if (!(flight is DDJBFlight) && !(flight is CFFTFlight) && !(flight is LWTTFlight))
                    totalDiscounts += 50;
            }

            if (flights.Count > 5)
                totalDiscounts += totalFees * 0.03;

            foreach (var flight in flights.Values)
            {
                if (flights.Count % 3 == 0)
                    totalDiscounts += 350;

                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                    totalDiscounts += 110;
            }

            double finalFees = totalFees - totalDiscounts;
            return (totalFees, totalDiscounts, finalFees);
        }

        public override string ToString()
        {
            return $"Airline: {Name} ({Code})";
        }
    }
}
