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
    class BoardingGate
    {
        private string gateName;
        private bool supportsDDJB;
        private bool supportsCFFT;

        private bool supportsLWTT;
        private Flight flight;

        public string GateName
        { get { return gateName; } set { gateName = value; } }

        public bool SupportDDJB
        { get { return supportsDDJB; } set { supportsDDJB = value; } }
        public bool SupportCFFT
        { get { return supportsCFFT; } set { supportsCFFT = value; } }
        public bool SupportLWTT
        { get { return supportsLWTT; } set { supportsLWTT = value; } }

        public Flight Flight
        {
            get { return flight; }
            set { flight = value; }
        }
        public BoardingGate(string gateName, bool supportDDJB, bool supportCFFT, bool supportLWTT)
        {
            GateName = gateName;
            SupportDDJB = supportDDJB;
            SupportCFFT = supportCFFT;
            SupportLWTT = supportLWTT;
        }

        public double CalculateFees()
        {
            return 300;
        }

        public override string ToString()
        {
            string flightInfo = Flight != null ? $"Assigned Flight: {Flight.FlightNumber}" : "No Flight Assigned";
            return $"Gate: {GateName} (CFFT: {SupportCFFT}, DDJB: {SupportDDJB}, LWTT: {SupportLWTT}) | {flightInfo}";
        }
    }
}
