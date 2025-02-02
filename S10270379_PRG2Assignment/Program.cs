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
}

void List_all_gates(Terminal terminal)
{
    // Print a header for the list of boarding gates
    Console.WriteLine(@"====================================================
List of Boarding Gates for Changi Airport Terminal 5
====================================================");

    // Print the column headers for the boarding gate information
    Console.WriteLine($"{"Gate Name",-16} {"DDJB",-23} {"CFFT",-23} {"LWTT",-15} {"Flight number",-15}");

    // Iterate through each boarding gate in the terminal's BoardingGates collection
    foreach (var boarding in terminal.BoardingGates.Values)
    {
        // Check if the current boarding gate is associated with a flight
        if (boarding.Flight == null)
        {
            // Print the gate information without a flight number if no flight is assigned
            Console.WriteLine($"{boarding.GateName,-16} {boarding.SupportDDJB,-23} {boarding.SupportCFFT,-23} {boarding.SupportLWTT,-15}");
        }
        else
        {
            // Print the gate information along with the associated flight number
            Console.WriteLine($"{boarding.GateName,-16} {boarding.SupportDDJB,-23} {boarding.SupportCFFT,-23} {boarding.SupportLWTT,-15} {boarding.Flight.FlightNumber,-23}");
        }
    }
}

void Assign_boardinggate(Terminal terminal)
{
    string? Input_flightNumber = null;
    string? Input_boardingGate = null;
    Flight? flight = null;
    bool flightnumberFound = false;
    bool Validate_InputBoardingGate = false;

    Console.WriteLine(@"=============================================
Assign a Boarding Gate to a Flight
=============================================");

    while (true)
    {
        try
        {
            Console.WriteLine("Enter Flight Number:");
            Input_flightNumber = Console.ReadLine().ToUpper();

            if (terminal.Flights.ContainsKey(Input_flightNumber))
            {
                flightnumberFound = true;
                try
                {
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Enter Boarding Gate:");
                            Input_boardingGate = Console.ReadLine().ToUpper();
                            if (terminal.BoardingGates.ContainsKey(Input_boardingGate))
                                Validate_InputBoardingGate = true;

                            // Check if boarding gate exists
                            if (!terminal.BoardingGates.ContainsKey(Input_boardingGate))
                            {
                                Console.WriteLine("Boarding Gate does not exist. Please try again.");
                                continue;
                            }
                            else
                            {
                                BoardingGate boardingGate = terminal.BoardingGates[Input_boardingGate];

                                // Check if the gate is already assigned
                                if (boardingGate.Flight != null)
                                {
                                    Console.WriteLine($"Error: Boarding Gate {Input_boardingGate} is already assigned to Flight {boardingGate.Flight.FlightNumber}.");
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch { }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing the boarding gate input: {ex.Message}");
                }



                if (Validate_InputBoardingGate == true && flightnumberFound == true)
                {
                    flight = terminal.Flights[Input_flightNumber];
                    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                    Console.WriteLine($"Origin: {flight.Origin}");
                    Console.WriteLine($"Destination: {flight.Destination}");
                    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");

                    string specialcode = "";
                    if (flight is DDJBFlight)
                        specialcode = "DDJB";
                    else if (flight is CFFTFlight)
                        specialcode = "CFFT";
                    else if (flight is LWTTFlight)
                        specialcode = "LWTT";
                    else
                        specialcode = "None";

                    Console.WriteLine($"Special Request Code: {specialcode}");
                    BoardingGate boardinggate = terminal.BoardingGates[Input_boardingGate];
                    Console.WriteLine($"Boarding Gate Name: {Input_boardingGate}");
                    Console.WriteLine($"Support DDJB: {boardinggate.SupportDDJB}");
                    Console.WriteLine($"Support CFFT: {boardinggate.SupportCFFT}");
                    Console.WriteLine($"Support LWTT: {boardinggate.SupportLWTT}");


                    boardinggate.Flight = flight;
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Would you like to update the flight status? (Y/N)");
                            string updateStatus = Console.ReadLine().ToUpper();

                            if (updateStatus == "Y")
                            {
                                Console.WriteLine("1. Delayed");
                                Console.WriteLine("2. Boarding");
                                Console.WriteLine("3. On Time");
                                Console.WriteLine("Select the new status:");
                                string update_option = Console.ReadLine();

                                if (update_option == "1")
                                {
                                    flight.Status = "Delayed";
                                }
                                else if (update_option == "2")
                                {
                                    flight.Status = "Boarding";
                                }
                                else if (update_option == "3")
                                {
                                    flight.Status = "On Time";
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option. Status remains unchanged.");
                                    continue;
                                }
                                Console.WriteLine($"Flight {Input_flightNumber} has been assigned to Boarding Gate {Input_boardingGate}!");
                                return;
                            }
                            else if (updateStatus == "N")
                            {
                                Console.WriteLine($"Flight {Input_flightNumber} has been assigned to Boarding Gate {Input_boardingGate}!");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Option! Please try again!");
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    Console.WriteLine("Boarding Gate does not exist. Please try again.");
                    continue;
                }
            }
            else
            {
                Console.WriteLine("Flight number not found. Please try again.");
                continue;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

    }
}
