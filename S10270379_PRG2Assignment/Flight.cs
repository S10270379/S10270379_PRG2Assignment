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
    public abstract class Flight : IComparable<Flight>
    {
        private string flightNumber;
        private string origin;
        private string destination;
        private DateTime expectedTime;
        private string status;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public DateTime ExpectedTime
        {
            get { return expectedTime; }
            set { expectedTime = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public int CompareTo(Flight other)
        {
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

        public virtual double CalculateFees()
        {
            return 300;
        }

        public override string ToString()
        {
            return $"{flightNumber} {origin} {destination} {expectedTime} {status}";
        }
    }
}
