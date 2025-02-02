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

void Create_flight(Terminal terminal)
{
    while (true)
    {
        bool Validate_FN = false;
        bool Validate_Origin = false;
        bool Validate_Destin = false;
        bool Validate_EDA = false;
        bool Validate_Code = false;
        bool flight_exist = false;

        string Inputflightnumber = "";
        string InputOrigin = "";
        string InputDestination = "";
        DateTime EDA = DateTime.MinValue;
        string? specialcode = null;


        // Validate Flight Number
        while (!Validate_FN)
        {
            try
            {
                Console.Write("Enter Flight Number: ");
                Inputflightnumber = Console.ReadLine().ToUpper();

                string[] parts = Inputflightnumber.Split(' ');

                if (terminal.Flights.ContainsKey(Inputflightnumber))
                {
                    Console.WriteLine("Flight number exist already! Please try again.");

                }
                else
                {
                    foreach (var Airline in terminal.Airlines.Values)
                    {
                        if (parts[0].Contains(Airline.Code) && Inputflightnumber.Count(char.IsDigit) == 3)
                        {
                            Validate_FN = true;
                            break;
                        }
                    }

                    if (!Validate_FN)
                    {
                        Console.WriteLine("Invalid Flight Number! Please try again.");
                    }
                }

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex);
            }
        }


        // Validate Origin
        while (!Validate_Origin)
        {
            try
            {
                Console.Write("Enter Origin: ");
                InputOrigin = Console.ReadLine();

                foreach (var flight in terminal.Flights.Values)
                {
                    if (InputOrigin == flight.Origin || InputOrigin == flight.Destination)
                    {
                        Validate_Origin = true;
                        break;
                    }
                }

                if (!Validate_Origin)
                {
                    Console.WriteLine("Invalid Origin! Please try again.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Validate Destination
        while (!Validate_Destin)
        {
            try
            {
                Console.Write("Enter Destination: ");
                InputDestination = Console.ReadLine();

                foreach (var flight in terminal.Flights.Values)
                {
                    if (InputDestination == flight.Destination || InputDestination == flight.Origin)
                    {
                        Validate_Destin = true;
                        break;
                    }
                }

                if (!Validate_Destin)
                {
                    Console.WriteLine("Invalid Destination! Please try again.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Validate Expected Departure/Arrival Time
        while (!Validate_EDA)
        {
            try
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string edaInput = Console.ReadLine();
                if (DateTime.TryParse(edaInput, out EDA))
                {
                    Validate_EDA = true;
                }
                else
                {
                    Console.WriteLine("Invalid Date/Time format! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // Validate Special Request Code
        while (!Validate_Code)
        {
            try
            {
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialcode = Console.ReadLine().ToUpper();

                if (specialcode == "CFFT" || specialcode == "DDJB" || specialcode == "LWTT" || specialcode == "NONE")
                {
                    Validate_Code = true;
                }
                else
                {
                    Console.WriteLine("Invalid Special Request Code! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        Flight? newFlight = null;
        string outputspecialcode = "";
        if (specialcode == "NONE")
        {
            newFlight = new NORMFlight(Inputflightnumber, InputOrigin, InputDestination, EDA);
        }
        else if (specialcode == "DDJB")
        {
            newFlight = new DDJBFlight(Inputflightnumber, InputOrigin, InputDestination, EDA, 300);
            outputspecialcode = specialcode;
        }
        else if (specialcode == "CFFT")
        {
            newFlight = new CFFTFlight(Inputflightnumber, InputOrigin, InputDestination, EDA, 150);
            outputspecialcode = specialcode;
        }
        else if (specialcode == "LWTT")
        {
            newFlight = new LWTTFlight(Inputflightnumber, InputOrigin, InputDestination, EDA, 500);
            outputspecialcode = specialcode;
        }
        if (Validate_FN && Validate_EDA && Validate_Origin && Validate_Destin && Validate_Code)
        {
            terminal.Flights.Add(newFlight.FlightNumber, newFlight);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "flights.csv");
            string formattedTime = EDA.ToString("h:mm tt"); // Ensure this matches the CSV format
            string csvLine = $"{Inputflightnumber},{InputOrigin},{InputDestination},{formattedTime},{outputspecialcode}{Environment.NewLine}";
            File.AppendAllText(filePath, csvLine);
        }

        Console.WriteLine($"Flight {Inputflightnumber} has been added!");
        while (true)
        {
            try
            {
                Console.WriteLine("Would you like to add another flight? (Y/N)");
                string input_continue = Console.ReadLine().ToUpper();
                if (input_continue == "Y")
                    Create_flight(terminal);
                else if (input_continue == "N")
                    return;
                else
                {
                    Console.WriteLine("Invalid Input! Please enter 'Y' or 'N' only.");
                    continue;
                }
            }
            catch { }
        }


    }
}

void Display_Airline_Flights(Terminal terminal)
{
    string? InputAirlineCode = null;
    string? Inputflightnumber = null;
    bool AirCode_exist = false;
    bool flight_exist = false;
    Console.WriteLine(@"=============================================
List of Airlines for Changi Airport Terminal 5
=============================================
");

    Console.WriteLine($"{"Airline Code",-16} {"Airline Name",-17}");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-16} {airline.Name,-17}");
    }
    while (!AirCode_exist)
    {
        try
        {
            Console.WriteLine("Enter Airline Codea: ");
            InputAirlineCode = Console.ReadLine().ToUpper();
            foreach (var airline in terminal.Airlines.Values)
            {
                if (airline.Code.Contains(InputAirlineCode) && InputAirlineCode != "")
                {
                    AirCode_exist = true;
                    Console.WriteLine(@$"=============================================
List of Flights for {airline.Name}
=============================================");
                    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21}");
                    foreach (var flight in terminal.Flights.Values)
                    {
                        if (flight.FlightNumber.Contains(InputAirlineCode))
                            Console.WriteLine($"{flight.FlightNumber,-16} {airline.Name,-21} {flight.Origin,-21} {flight.Destination,-21}");
                    }
                }
            }
            if (AirCode_exist == false)
            {
                Console.WriteLine("Airline Code does not exist! Please try again.");
                continue;
            }
            else
            {
                while (!flight_exist)
                {
                    try
                    {
                        Console.Write("Enter flight number: ");
                        Inputflightnumber = Console.ReadLine().ToUpper();

                        Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21} {"Expected Departure/Arrival Time",-35} {"Status",-16} {"Boarding Gate",-13}");
                        foreach (var flight in terminal.Flights.Values)
                        {
                            string airlineCode = flight.FlightNumber.Split(' ')[0];
                            string flightStatus;
                            if (flight.Status != "status")
                                flightStatus = flight.Status;
                            else
                                flightStatus = "------";

                            foreach (var airline in terminal.Airlines.Values)
                            {
                                if (airline.Code == airlineCode && flight.FlightNumber == Inputflightnumber)
                                {
                                    string? boardingGate = "Unassigned";  // Default to unassigned
                                    bool gateFound = false;

                                    foreach (var gate in terminal.BoardingGates.Values)
                                    {
                                        if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                                        {
                                            boardingGate = gate.GateName;
                                            gateFound = true;  // Mark that we found the gate
                                            break;  // Exit the loop once we find the matching gate
                                        }
                                    }

                                    // Display only the matching flight with its details
                                    Console.WriteLine($"{flight.FlightNumber,-16} {airline.Name,-21} {flight.Origin,-21} {flight.Destination,-21} {flight.ExpectedTime,-35} {flightStatus,-16} {boardingGate,-13}");
                                    flight_exist = true;  // Set flag to exit the outer loop
                                    break;  // Exit the airline loop since we found our match
                                }
                            }

                        }
                    }
                    catch { }
                }

            }
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
        }


    }

}

void Modify_Flight_Details(Terminal terminal)
{
    string? InputAirlineCode = null;
    bool AirCode_exist = false;
    bool FlightNumber_exist = false;
    Console.WriteLine(@"=============================================
List of Airlines for Changi Airport Terminal 5
=============================================
");

    Console.WriteLine($"{"Airline Code",-16} {"Airline Name",-17}");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-16} {airline.Name,-17}");
    }
    while (!AirCode_exist)
    {
        try
        {
            Console.WriteLine("Enter Airline Code: ");
            InputAirlineCode = Console.ReadLine().ToUpper().Trim();
            foreach (var airline in terminal.Airlines.Values)
            {
                if (airline.Code.Contains(InputAirlineCode) && InputAirlineCode != "")
                {
                    AirCode_exist = true;
                    Console.WriteLine($"List of Flights for {airline.Name}");
                    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21} {"Expected Departure/Arrival Time",-21}");
                    foreach (var flight in terminal.Flights.Values)
                    {
                        if (flight.FlightNumber.Contains(InputAirlineCode))
                            Console.WriteLine($"{flight.FlightNumber,-16} {airline.Name,-21} {flight.Origin,-21} {flight.Destination,-21} {flight.ExpectedTime,-21}");
                    }
                }
            }
            if (AirCode_exist == false)
            {
                Console.WriteLine("Airline Code does not exist! Please try again.");
                continue;
            }
            else
            {
                while (true)
                {
                    string? Inputflightnumber = null;
                    int? ModifyDelete_option = null;
                    int? modify_option = null;
                    string update_status = "Scheduled";
                    string? update_special = null;
                    string InputBoardingGate = "Unassigned";
                    Flight? existingFlight = null;
                    BoardingGate? boardingGate = null;
                    try
                    {

                        Console.WriteLine("Choose an existing Flight to modify or delete:");
                        Inputflightnumber = Console.ReadLine().ToUpper();

                        foreach (var flight in terminal.Flights.Values)
                        {
                            if (flight.FlightNumber.Equals(Inputflightnumber))
                            {
                                FlightNumber_exist = true;
                                while (true)
                                {
                                    try
                                    {

                                        Console.WriteLine("1. Modify Flight");
                                        Console.WriteLine("2. Delete Flight");
                                        Console.WriteLine("Choose an option:");
                                        ModifyDelete_option = Convert.ToInt32(Console.ReadLine());
                                        if (ModifyDelete_option == 1)
                                        {
                                            Console.WriteLine("1. Modify Basic Information");
                                            Console.WriteLine("2. Modify Status");
                                            Console.WriteLine("3. Modify Special Request Code");
                                            Console.WriteLine("4. Modify Boarding Gate");
                                            Console.WriteLine("Choose an option:");
                                            modify_option = Convert.ToInt32(Console.ReadLine());
                                            if (modify_option == 1)
                                            {
                                                while (true)
                                                {
                                                    bool Validate_Origin = false;
                                                    bool Validate_Destin = false;
                                                    bool Validate_EDA = false;
                                                    bool Validate_Code = false;
                                                    bool flight_exist = false;

                                                    string? InputOrigin = null;
                                                    string? InputDestination = null;
                                                    DateTime EDA = DateTime.MinValue;
                                                    string? specialcode = null;

                                                    // Validate Origin
                                                    while (!Validate_Origin)
                                                    {
                                                        try
                                                        {
                                                            Console.Write("Enter new Origin: ");
                                                            InputOrigin = Console.ReadLine();

                                                            foreach (var f in terminal.Flights.Values)
                                                            {
                                                                if (InputOrigin == f.Origin || InputOrigin == f.Destination)
                                                                {
                                                                    Validate_Origin = true;
                                                                    break;
                                                                }
                                                            }

                                                            if (!Validate_Origin)
                                                            {
                                                                Console.WriteLine("Invalid Origin! Please try again.");
                                                            }
                                                        }
                                                        catch (ArgumentNullException ex)
                                                        {
                                                            Console.WriteLine(ex);
                                                        }
                                                    }

                                                    // Validate Destination
                                                    while (!Validate_Destin)
                                                    {
                                                        try
                                                        {
                                                            Console.Write("Enter new Destination: ");
                                                            InputDestination = Console.ReadLine();


                                                            // Check if the input matches the flight's destination or origin exactly
                                                            foreach (var f in terminal.Flights.Values)
                                                            {
                                                                if (f.Destination.Contains(InputDestination) || f.Origin.Contains(InputDestination))
                                                                {
                                                                    Validate_Destin = true;
                                                                    break;
                                                                }
                                                            }

                                                            if (!Validate_Destin)
                                                            {
                                                                Console.WriteLine("Invalid Destination! Please try again.");
                                                            }
                                                        }
                                                        catch (ArgumentNullException ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                    }

                                                    // Validate Expected Departure/Arrival Time
                                                    while (!Validate_EDA)
                                                    {
                                                        try
                                                        {
                                                            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                                                            string edaInput = Console.ReadLine();
                                                            if (DateTime.TryParse(edaInput, out EDA))
                                                            {
                                                                Validate_EDA = true;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Invalid Date/Time format! Please try again.");
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex);
                                                        }
                                                    }
                                                    Flight update = terminal.Flights[Inputflightnumber];

                                                    update.Origin = InputOrigin;
                                                    update.Destination = InputDestination;
                                                    update.ExpectedTime = EDA;

                                                    Console.WriteLine($"Flight {Inputflightnumber} has been Updated!");
                                                    break;

                                                }
                                            }
                                            else if (modify_option == 2)
                                            {
                                                while (true)
                                                {

                                                    try
                                                    {
                                                        Console.Write("Enter new Status: ");
                                                        update_status = Console.ReadLine();
                                                        if (update_status == "On Time" || update_status == "Delayed" || update_status == "Boarding")
                                                        {
                                                            Flight update = terminal.Flights[Inputflightnumber];
                                                            update.Status = update_status;
                                                            Console.WriteLine($"Flight {Inputflightnumber} status has been Updated!");
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Invalid Input! Please try again.");
                                                            continue;
                                                        }

                                                    }
                                                    catch
                                                    { }
                                                }
                                            }
                                            else if (modify_option == 3)
                                            {
                                                while (true)
                                                {

                                                    try
                                                    {
                                                        Console.Write("Enter new Special Request Code: ");
                                                        update_special = Console.ReadLine().ToUpper();
                                                        if (update_special == "DDJB" || update_special == "CFFT" || update_special == "LWTT" || update_special == "NONE")
                                                        {
                                                            existingFlight = terminal.Flights[Inputflightnumber];
                                                            Flight updatedFlight;

                                                            // Create new flight object based on special request code
                                                            if (update_special == "LWTT")
                                                            {
                                                                updatedFlight = new LWTTFlight(
                                                                    existingFlight.FlightNumber,
                                                                    existingFlight.Origin,
                                                                    existingFlight.Destination,
                                                                    existingFlight.ExpectedTime,
                                                                    500); // LWTT fee
                                                            }
                                                            else if (update_special == "DDJB")
                                                            {
                                                                updatedFlight = new DDJBFlight(
                                                                    existingFlight.FlightNumber,
                                                                    existingFlight.Origin,
                                                                    existingFlight.Destination,
                                                                    existingFlight.ExpectedTime,
                                                                    300); // DDJB fee
                                                            }
                                                            else if (update_special == "CFFT")
                                                            {
                                                                updatedFlight = new CFFTFlight(
                                                                    existingFlight.FlightNumber,
                                                                    existingFlight.Origin,
                                                                    existingFlight.Destination,
                                                                    existingFlight.ExpectedTime,
                                                                    150); // CFFT fee
                                                            }
                                                            else // NONE
                                                            {
                                                                updatedFlight = new NORMFlight(
                                                                    existingFlight.FlightNumber,
                                                                    existingFlight.Origin,
                                                                    existingFlight.Destination,
                                                                    existingFlight.ExpectedTime);
                                                            }

                                                            // Preserve the existing status
                                                            updatedFlight.Status = existingFlight.Status;

                                                            // Replace the flight in the dictionary
                                                            terminal.Flights[Inputflightnumber] = updatedFlight;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Invalid Input! Please try again.");
                                                            continue;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                            }
                                            else if (modify_option == 4)
                                            {
                                                while (true)
                                                {
                                                    try
                                                    {
                                                        Console.WriteLine("Enter new Boarding gate: ");
                                                        InputBoardingGate = Console.ReadLine().ToUpper();
                                                        //BoardingGate is key, Fliight number is value
                                                        if (terminal.BoardingGates.ContainsKey(InputBoardingGate))
                                                        {
                                                            // Get the boarding gate object
                                                            boardingGate = terminal.BoardingGates[InputBoardingGate];

                                                            // Check if the boarding gate is already assigned to another flight
                                                            if (boardingGate.Flight != null)
                                                            {
                                                                Console.WriteLine($"Boarding Gate {InputBoardingGate} is already assigned to Flight {boardingGate.Flight.FlightNumber}.");
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                // Assign the boarding gate to the flight
                                                                boardingGate.Flight = terminal.Flights[Inputflightnumber];
                                                                Console.WriteLine($"Boarding Gate {InputBoardingGate} has been assigned to Flight {Inputflightnumber}.");
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Invalid Boarding Gate! Please try again.");
                                                            continue;
                                                        }

                                                    }
                                                    catch { }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid Option! Please try again.");
                                                continue;
                                            }
                                            Console.WriteLine("Flight updated!");
                                            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                                            Console.WriteLine($"Origin: {flight.Origin}");
                                            Console.WriteLine($"Destination: {flight.Destination}");
                                            Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime}");
                                            Console.WriteLine($"Status: {update_status}");
                                            Console.WriteLine($"Special Request Code: {terminal.Flights[Inputflightnumber].GetType().Name}");
                                            Console.WriteLine($"Boarding Gate: {InputBoardingGate}");
                                            return;
                                        }
                                        else if (ModifyDelete_option == 2)
                                        {
                                            terminal.Flights.Remove(Inputflightnumber);
                                            Console.WriteLine($"Flight {Inputflightnumber} has been deleted!");
                                            return;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid Input! Please try again.");
                                            continue;
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                        if (FlightNumber_exist == false)
                        {
                            Console.WriteLine("Invalid Input. Please try again!");
                            continue;
                        }
                    }
                    catch { }
                }
            }

        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }



}

void Display_Flight_Schedule(Terminal terminal)
{
    Console.WriteLine(@"=============================================
List of Flights for Changi Airport Terminal 5
=============================================");
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21} {"Expected Departure/Arrival Time",-35} {"Status",-16} {"Boarding Gate",-13}");

    // Convert the dictionary to a list and sort it based on ExpectedTime
    List<Flight> sortedFlights = terminal.Flights.Values.ToList();
    sortedFlights.Sort();

    foreach (var flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Split(' ')[0];
        string flightStatus;
        if (flight.Status != "status")
            flightStatus = flight.Status;
        else
            flightStatus = "------";
        Airline airline = terminal.GetAirlineFromFlight(flight);
        string airlineName = airline != null ? airline.Name : "Unknown Airline";

        string boardingGate = "Unassigned";
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            {
                boardingGate = gate.GateName;
                break;
            }
        }

        Console.WriteLine($"{flight.FlightNumber,-16} {airlineName,-21} {flight.Origin,-21} {flight.Destination,-21} {flight.ExpectedTime,-35} {flightStatus,-16} {boardingGate,-13}");
    }
}
