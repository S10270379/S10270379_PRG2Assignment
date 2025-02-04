﻿//==========================================================
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
    class CFFTFlight : Flight
    {
        private double requestFee;

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee) : base(flightNumber, origin, destination, expectedTime, "status")
        {
            this.requestFee = requestFee;
        }

        public override double CalculateFees()
        {
            return requestFee + 300;
        }
    }
}
