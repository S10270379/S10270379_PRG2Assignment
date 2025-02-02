//==========================================================
// Student Number : S10270379
// Student Name : Ng Lee Meng
// Partner Name : How Shao Yang Louis
//==========================================================

using S10270379_PRG2Assignment;

void loading_Airlines(Terminal terminal)
{
    // Inform the user that the loading process is starting
    Console.WriteLine("Loading Airlines...");

    // Read all lines from the "airlines.csv" file into an array of strings
    string[] Airlines = File.ReadAllLines("airlines.csv");

    // Output the number of airlines loaded, skipping the header line
    Console.WriteLine(Airlines.Skip(1).Count() + " Airlines Loaded!");

    // Loop through each line in the Airlines array, starting from index 1 to skip the header
    for (int i = 1; i < Airlines.Length; i++)
    {
        // Split the current line by commas to separate the airline name and code
        string[] split_air = Airlines[i].Split(',');

        // Assign the first element as the airline name
        string airlineName = split_air[0];

        // Assign the second element as the airline code
        string airlineCode = split_air[1];

        // Create a new Airline object using the extracted name and code
        Airline airline = new Airline(airlineName, airlineCode);

        // Add the newly created Airline object to the terminal's list of airlines
        terminal.AddAirline(airline);
    }
}

void loading_BoardingGates(Terminal terminal)
{
    // Inform the user that the loading process for boarding gates is starting
    Console.WriteLine("Loading Boarding Gates...");
    
    // Read all lines from the "boardinggates.csv" file into an array of strings
    string[] BoardingGates = File.ReadAllLines("boardinggates.csv");
    
    // Output the number of boarding gates loaded, skipping the header line
    Console.WriteLine(BoardingGates.Skip(1).Count() + " Boarding Gates Loaded!");
    
    // Loop through each line in the BoardingGates array, starting from index 1 to skip the header
    for (int i = 1; i < BoardingGates.Length; i++)
    {
        // Initialize nullable boolean variables for the boarding gate attributes
        bool? DDJB = null;
        bool? CFFT = null;
        bool? LWTT = null;
        
        // Split the current line by commas to separate the boarding gate information
        string[] split_board = BoardingGates[i].Split(',');
        
        // Assign the first element as the boarding gate identifier
        string BG = split_board[0];
        
        // Convert the subsequent elements to boolean values and assign them to the respective variables
        DDJB = Convert.ToBoolean(split_board[1]);
        CFFT = Convert.ToBoolean(split_board[2]);
        LWTT = Convert.ToBoolean(split_board[3]);
        
        // Create a new BoardingGate object using the extracted identifier and boolean values
        BoardingGate Boarding = new BoardingGate(BG, Convert.ToBoolean(DDJB), Convert.ToBoolean(CFFT), Convert.ToBoolean(LWTT));
        
        // Add the newly created BoardingGate object to the terminal's list of boarding gates
        terminal.AddBoardingGate(Boarding);
    }
}

void loading_flights(Terminal terminal)
{
    Console.WriteLine("Loading Flights...");
    string[] Flights = File.ReadAllLines("flights.csv");
    Console.WriteLine(Flights.Skip(1).Count() + " Flights Loaded!");

    for (int i = 1; i < Flights.Length; i++)
    {
        int Fee = 0;
        string[] split_flights = Flights[i].Split(',');

        string FN = split_flights[0];
        string parts = FN.Split(" ")[0];
        string Origin = split_flights[1];
        string Destination = split_flights[2];
        DateTime EDA = Convert.ToDateTime(split_flights[3]);
        string SRC = split_flights.Length > 4 ? split_flights[4].Trim() : "";
        string Status = "Scheduled";

        Flight Flightz;

        if (string.IsNullOrEmpty(SRC))
        {
            Flightz = new NORMFlight(FN, Origin, Destination, EDA);
        }
        else if (SRC == "DDJB")
        {
            Fee = 300;
            Flightz = new DDJBFlight(FN, Origin, Destination, EDA, Fee);
        }
        else if (SRC == "CFFT")
        {
            Fee = 150;
            Flightz = new CFFTFlight(FN, Origin, Destination, EDA, Fee);
        }
        else if (SRC == "LWTT")
        {
            Fee = 500;
            Flightz = new LWTTFlight(FN, Origin, Destination, EDA, Fee);
        }
        else
        {
            Console.WriteLine($"Unknown SRC: {SRC}, skipping flight {FN}");
            continue;
        }
        terminal.Flights[FN] = Flightz;
        Airline airline = terminal.GetAirlineFromFlight(Flightz);
        if (airline != null)
        {
            // If an airline is found, add the flight to that airline's flights list
            airline.AddFlight(Flightz);
        }
    }
<<<<<<< HEAD
}

void List_all_flights(Terminal terminal)
{
    Console.WriteLine(@"=============================================
List of Flights for Changi Airport Terminal 5
=============================================");
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21} {"Expected Departure/Arrival Time",-31}");


    foreach (var flights in terminal.Flights.Values)
    {
        Airline airline = terminal.GetAirlineFromFlight(flights);
        string airlineName;

        if (airline != null)
        {
            airlineName = airline.Name;
        }
        else
        {
            airlineName = "Unknown Airline";
        }
        Console.WriteLine($"{flights.FlightNumber,-16} {airlineName,-21} {flights.Origin,-21} {flights.Destination,-21} {flights.ExpectedTime,-31}");
    }
=======
>>>>>>> 32accc73d57e8dc0eeb7dd9b091edfade40ec352
}