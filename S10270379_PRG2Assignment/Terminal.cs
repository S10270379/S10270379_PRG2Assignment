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

namespace S10270379_PRG2Assignment
{
    class Terminal
    {
        private string terminalName;

        public string TerminalName
        {
            get { return terminalName; }
            set { terminalName = value; }
        }

        Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        Dictionary<string, double> gateFees = new Dictionary<string, double>();

        public Dictionary<string, Airline> Airlines
        { get { return airlines; } set { airlines = value; } }
        public Dictionary<string, Flight> Flights
        { get { return flights; } set { flights = value; } }
        public Dictionary<string, BoardingGate> BoardingGates
        { get { return boardingGates; } set { boardingGates = value; } }
        public Terminal(string name)
        {
            TerminalName = name;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            gateFees = new Dictionary<string, double>();
        }
        public bool AddAirline(Airline airline)
        {
            if (!airlines.ContainsKey(airline.Code))
            {
                airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!boardingGates.ContainsKey(gate.GateName))
            {
                boardingGates[gate.GateName] = gate;
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {

            string airlineName = flight.FlightNumber.Split(" ")[0];
            return Airlines[airlineName]; ;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in airlines.Values)
            {
                var (subtotalFees, subtotalDiscounts, finalFees) = airline.CalculateFees();
                Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");
                Console.WriteLine($"Subtotal Fees: ${subtotalFees:F2}");
                Console.WriteLine($"Subtotal Discounts: ${subtotalDiscounts:F2}");
                Console.WriteLine($"Final Fees: ${finalFees:F2}");
                Console.WriteLine("----------------------------------------");
            }
        }

        public override string ToString()
        {
            return terminalName;
        }

    }
}
