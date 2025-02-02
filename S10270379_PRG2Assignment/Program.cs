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
    Console.WriteLine("Loading Flights..."); // Display message indicating flight data is being loaded.

    string[] Flights = File.ReadAllLines("flights.csv"); // Read all lines from the CSV file into an array.

    Console.WriteLine(Flights.Skip(1).Count() + " Flights Loaded!"); // Display the number of flights loaded, excluding the header.

    for (int i = 1; i < Flights.Length; i++) // Loop through each flight record, starting from index 1 (skipping the header).
    {
        int Fee = 0; // Initialize fee variable.
        string[] split_flights = Flights[i].Split(','); // Split the CSV line into individual flight details.

        string FN = split_flights[0]; // Flight number.
        string parts = FN.Split(" ")[0]; // Extract first part of flight number (though this variable is unused).
        string Origin = split_flights[1]; // Flight origin.
        string Destination = split_flights[2]; // Flight destination.
        DateTime EDA = Convert.ToDateTime(split_flights[3]); // Expected departure date, converted from string to DateTime.
        string SRC = split_flights.Length > 4 ? split_flights[4].Trim() : ""; // Extract source type if available, otherwise set to an empty string.
        string Status = "Scheduled"; // Default flight status.

        Flight Flightz; // Declare flight object.

        // Determine flight type based on SRC value
        if (string.IsNullOrEmpty(SRC))
        {
            Flightz = new NORMFlight(FN, Origin, Destination, EDA); // Create a normal flight if no special category is provided.
        }
        else if (SRC == "DDJB")
        {
            Fee = 300; // Set fee for DDJB flights.
            Flightz = new DDJBFlight(FN, Origin, Destination, EDA, Fee); // Create a DDJB flight.
        }
        else if (SRC == "CFFT")
        {
            Fee = 150; // Set fee for CFFT flights.
            Flightz = new CFFTFlight(FN, Origin, Destination, EDA, Fee); // Create a CFFT flight.
        }
        else if (SRC == "LWTT")
        {
            Fee = 500; // Set fee for LWTT flights.
            Flightz = new LWTTFlight(FN, Origin, Destination, EDA, Fee); // Create a LWTT flight.
        }
        else
        {
            Console.WriteLine($"Unknown SRC: {SRC}, skipping flight {FN}"); // Handle unknown flight types by skipping them.
            continue; // Skip to the next iteration of the loop.
        }

        terminal.Flights[FN] = Flightz; // Add the flight to the terminal's flight dictionary.

        Airline airline = terminal.GetAirlineFromFlight(Flightz); // Retrieve the airline associated with this flight.

        if (airline != null)
        {
            // If an airline is found, add the flight to that airline's flight list.
            airline.AddFlight(Flightz);
        }
    }

}

void List_all_flights(Terminal terminal)
{
    // Display a header for the flight list of Changi Airport Terminal 5.
    Console.WriteLine(@"=============================================
List of Flights for Changi Airport Terminal 5
=============================================");

    // Display the column headers for the flight details.
    Console.WriteLine($"{"Flight Number",-16} {"Airline Name",-21} {"Origin",-21} {"Destination",-21} {"Expected Departure/Arrival Time",-31}");

    // Loop through each flight in the terminal's flights dictionary.
    foreach (var flights in terminal.Flights.Values)
    {
        // Retrieve the airline associated with the current flight.
        Airline airline = terminal.GetAirlineFromFlight(flights);
        string airlineName;

        // Check if the airline is found, if not, assign "Unknown Airline" to the airline name.
        if (airline != null)
        {
            airlineName = airline.Name; // Get the airline name if available.
        }
        else
        {
            airlineName = "Unknown Airline"; // If no airline is found, set the name as "Unknown Airline".
        }

        // Display the flight information with the flight number, airline name, origin, destination, and expected time.
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
    string? Input_flightNumber = null; // Store the user input for flight number.
    string? Input_boardingGate = null; // Store the user input for boarding gate.
    Flight? flight = null; // Initialize flight object.
    bool flightnumberFound = false; // Flag to check if flight number is found.
    bool Validate_InputBoardingGate = false; // Flag to check if boarding gate is valid.

    Console.WriteLine(@"=============================================
Assign a Boarding Gate to a Flight
=============================================");

    // Start a loop to continuously prompt the user until valid inputs are provided.
    while (true)
    {
        try
        {
            // Ask the user to enter a flight number.
            Console.WriteLine("Enter Flight Number:");
            Input_flightNumber = Console.ReadLine().ToUpper(); // Read and convert input to uppercase.

            // Check if the entered flight number exists in the terminal's flights collection.
            if (terminal.Flights.ContainsKey(Input_flightNumber))
            {
                flightnumberFound = true; // Set the flag to true as the flight is found.

                try
                {
                    // Start another loop to prompt for boarding gate until valid input is provided.
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Enter Boarding Gate:");
                            Input_boardingGate = Console.ReadLine().ToUpper(); // Read and convert boarding gate input to uppercase.

                            // Check if the entered boarding gate exists in the terminal's boarding gates.
                            if (terminal.BoardingGates.ContainsKey(Input_boardingGate))
                                Validate_InputBoardingGate = true;

                            // If the boarding gate does not exist, prompt for input again.
                            if (!terminal.BoardingGates.ContainsKey(Input_boardingGate))
                            {
                                Console.WriteLine("Boarding Gate does not exist. Please try again.");
                                continue;
                            }
                            else
                            {
                                BoardingGate boardingGate = terminal.BoardingGates[Input_boardingGate];

                                // If the boarding gate is already assigned to a flight, inform the user and prompt again.
                                if (boardingGate.Flight != null)
                                {
                                    Console.WriteLine($"Error: Boarding Gate {Input_boardingGate} is already assigned to Flight {boardingGate.Flight.FlightNumber}.");
                                    continue;
                                }
                                else
                                {
                                    break; // Break the loop if the boarding gate is available.
                                }
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex); } // Catch and ignore any exceptions during boarding gate input.
                    }

                }
                catch (Exception ex)
                {
                    // Catch any exceptions when processing the boarding gate input.
                    Console.WriteLine($"An error occurred while processing the boarding gate input: {ex.Message}");
                }

                // Proceed if both flight number and boarding gate are valid.
                if (Validate_InputBoardingGate == true && flightnumberFound == true)
                {
                    // Retrieve the flight from the terminal's flights collection.
                    flight = terminal.Flights[Input_flightNumber];

                    // Display flight information (flight number, origin, destination, expected time).
                    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                    Console.WriteLine($"Origin: {flight.Origin}");
                    Console.WriteLine($"Destination: {flight.Destination}");
                    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");

                    // Determine the special request code based on the flight type (e.g., DDJB, CFFT, LWTT).
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

                    // Retrieve the boarding gate and display its details (gate name, supported flight types).
                    BoardingGate boardinggate = terminal.BoardingGates[Input_boardingGate];
                    Console.WriteLine($"Boarding Gate Name: {Input_boardingGate}");
                    Console.WriteLine($"Support DDJB: {boardinggate.SupportDDJB}");
                    Console.WriteLine($"Support CFFT: {boardinggate.SupportCFFT}");
                    Console.WriteLine($"Support LWTT: {boardinggate.SupportLWTT}");

                    // Assign the flight to the selected boarding gate.
                    boardinggate.Flight = flight;

                    // Ask if the user wants to update the flight status.
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Would you like to update the flight status? (Y/N)");
                            string updateStatus = Console.ReadLine().ToUpper(); // Read user input for status update.

                            // If the user selects "Y", ask for the new status and update it.
                            if (updateStatus == "Y")
                            {
                                Console.WriteLine("1. Delayed");
                                Console.WriteLine("2. Boarding");
                                Console.WriteLine("3. On Time");
                                Console.WriteLine("Select the new status:");
                                string update_option = Console.ReadLine();

                                if (update_option == "1")
                                {
                                    flight.Status = "Delayed"; // Set status to Delayed.
                                }
                                else if (update_option == "2")
                                {
                                    flight.Status = "Boarding"; // Set status to Boarding.
                                }
                                else if (update_option == "3")
                                {
                                    flight.Status = "On Time"; // Set status to On Time.
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option. Status remains unchanged.");
                                    continue;
                                }
                                Console.WriteLine($"Flight {Input_flightNumber} has been assigned to Boarding Gate {Input_boardingGate}!");
                                return; // Return after successfully assigning the boarding gate and updating the status.
                            }
                            else if (updateStatus == "N")
                            {
                                Console.WriteLine($"Flight {Input_flightNumber} has been assigned to Boarding Gate {Input_boardingGate}!");
                                return; // Return if the user does not want to update the status.
                            }
                            else
                            {
                                Console.WriteLine("Invalid Option! Please try again!"); // Handle invalid options.
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex); } // Catch any exceptions during the flight status update.
                    }
                }
                else
                {
                    Console.WriteLine("Boarding Gate does not exist. Please try again."); // Handle invalid boarding gate.
                    continue;
                }
            }
            else
            {
                Console.WriteLine("Flight number not found. Please try again."); // Handle invalid flight number.
                continue;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}"); // Handle unexpected errors.
        }
    }
}

void Create_flight(Terminal terminal)
{
    // Start an infinite loop for flight creation process
    while (true)
    {
        // Flags to track the validation status for flight details
        bool Validate_FN = false;
        bool Validate_Origin = false;
        bool Validate_Destin = false;
        bool Validate_EDA = false;
        bool Validate_Code = false;
        bool flight_exist = false;

        // Initialize variables to store user input for flight details
        string Inputflightnumber = "";
        string InputOrigin = "";
        string InputDestination = "";
        DateTime EDA = DateTime.MinValue;
        string? specialcode = null;

        // Validate Flight Number input
        while (!Validate_FN)
        {
            try
            {
                Console.Write("Enter Flight Number: ");
                Inputflightnumber = Console.ReadLine().ToUpper(); // Read and convert input to uppercase

                string[] parts = Inputflightnumber.Split(' '); // Split flight number for validation

                // Check if flight number already exists in the terminal's flight collection
                if (terminal.Flights.ContainsKey(Inputflightnumber))
                {
                    Console.WriteLine("Flight number exist already! Please try again.");
                }
                else
                {
                    // Check if the flight number contains a valid airline code and 3-digit number
                    foreach (var Airline in terminal.Airlines.Values)
                    {
                        if (parts[0].Contains(Airline.Code) && Inputflightnumber.Count(char.IsDigit) == 3)
                        {
                            Validate_FN = true; // Mark flight number as valid
                            break;
                        }
                    }

                    // If the flight number isn't valid, prompt user to try again
                    if (!Validate_FN)
                    {
                        Console.WriteLine("Invalid Flight Number! Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Handle null argument exception
            }
        }

        // Validate Origin input
        while (!Validate_Origin)
        {
            try
            {
                Console.Write("Enter Origin: ");
                InputOrigin = Console.ReadLine(); // Read user input for origin

                // Check if the origin is valid (it should match an existing flight's origin or destination)
                foreach (var flight in terminal.Flights.Values)
                {
                    if (InputOrigin == flight.Origin || InputOrigin == flight.Destination)
                    {
                        Validate_Origin = true;
                        break;
                    }
                }

                // If origin is invalid, prompt user to try again
                if (!Validate_Origin)
                {
                    Console.WriteLine("Invalid Origin! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Handle null argument exception
            }
        }

        // Validate Destination input
        while (!Validate_Destin)
        {
            try
            {
                Console.Write("Enter Destination: ");
                InputDestination = Console.ReadLine(); // Read user input for destination

                // Check if the destination is valid (it should match an existing flight's origin or destination)
                foreach (var flight in terminal.Flights.Values)
                {
                    if (InputDestination == flight.Destination || InputDestination == flight.Origin)
                    {
                        Validate_Destin = true;
                        break;
                    }
                }

                // If destination is invalid, prompt user to try again
                if (!Validate_Destin)
                {
                    Console.WriteLine("Invalid Destination! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Handle null argument exception
            }
        }

        // Validate Expected Departure/Arrival Time input
        while (!Validate_EDA)
        {
            try
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string edaInput = Console.ReadLine(); // Read input for date/time

                // Try to parse the input into a valid DateTime
                if (DateTime.TryParse(edaInput, out EDA))
                {
                    Validate_EDA = true; // Mark time as valid
                }
                else
                {
                    Console.WriteLine("Invalid Date/Time format! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Handle any unexpected exceptions
            }
        }

        // Validate Special Request Code input
        while (!Validate_Code)
        {
            try
            {
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                specialcode = Console.ReadLine().ToUpper(); // Read and convert input to uppercase

                // Check if the special code is valid
                if (specialcode == "CFFT" || specialcode == "DDJB" || specialcode == "LWTT" || specialcode == "NONE")
                {
                    Validate_Code = true; // Mark special code as valid
                }
                else
                {
                    Console.WriteLine("Invalid Special Request Code! Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Handle any exceptions
            }
        }

        // Create the new flight based on user input and special code
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

        // If all validations are successful, add the new flight to the terminal's flights collection
        if (Validate_FN && Validate_EDA && Validate_Origin && Validate_Destin && Validate_Code)
        {
            terminal.Flights.Add(newFlight.FlightNumber, newFlight); // Add the flight to the system

            // Save the new flight details to the "flights.csv" file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "flights.csv");
            string formattedTime = EDA.ToString("h:mm tt"); // Format time to match the CSV format
            string csvLine = $"{Inputflightnumber},{InputOrigin},{InputDestination},{formattedTime},{outputspecialcode}{Environment.NewLine}";
            File.AppendAllText(filePath, csvLine); // Append the new flight details to the CSV

            Console.WriteLine($"Flight {Inputflightnumber} has been added!");
        }

        // Ask the user if they want to add another flight
        while (true)
        {
            try
            {
                Console.WriteLine("Would you like to add another flight? (Y/N)");
                string input_continue = Console.ReadLine().ToUpper(); // Read user's decision

                // Recursively call Create_flight if 'Y' is entered
                if (input_continue == "Y")
                    Create_flight(terminal);
                else if (input_continue == "N")
                    return; // Exit the method if 'N' is entered
                else
                {
                    Console.WriteLine("Invalid Input! Please enter 'Y' or 'N' only.");
                    continue; // Prompt again for valid input
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
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }
        catch (Exception ex)
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
                                                        catch (Exception ex)
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
                                                        catch (Exception ex)
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
                                                    catch (Exception ex) { Console.WriteLine(ex); }
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
                                    catch (Exception ex) { Console.WriteLine(ex); }
                                }
                            }
                        }
                        if (FlightNumber_exist == false)
                        {
                            Console.WriteLine("Invalid Input. Please try again!");
                            continue;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                }
            }

        }
        catch (Exception ex)
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

void ProcessUnassignedFlights(Terminal terminal)
{
    // Step 1: Create a queue for unassigned flights
    Queue<Flight> unassignedFlights = new Queue<Flight>();

    // Step 2: Check for unassigned flights and boarding gates
    int unassignedFlightCount = 0;
    int unassignedBoardingGateCount = 0;

    foreach (var flight in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            {
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned)
        {
            unassignedFlights.Enqueue(flight);
            unassignedFlightCount++;
        }
    }

    foreach (var gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == null)
        {
            unassignedBoardingGateCount++;
        }
    }

    Console.WriteLine($"Total number of Flights without a Boarding Gate assigned: {unassignedFlightCount}");
    Console.WriteLine($"Total number of Boarding Gates without a Flight assigned: {unassignedBoardingGateCount}");

    // Step 3: Assign boarding gates to flights
    int processedFlights = 0;
    int processedGates = 0;

    while (unassignedFlights.Count > 0)
    {
        Flight currentFlight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;

        // Check if the flight has a special request code
        string specialCode = "";
        if (currentFlight is DDJBFlight)
            specialCode = "DDJB";
        else if (currentFlight is CFFTFlight)
            specialCode = "CFFT";
        else if (currentFlight is LWTTFlight)
            specialCode = "LWTT";

        // Find an unassigned boarding gate that matches the special request code
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == null)
            {
                if (!string.IsNullOrEmpty(specialCode))
                {
                    // Check if the gate supports the special request code
                    if ((specialCode == "DDJB" && (gate.GateName.StartsWith("A10") || gate.GateName.StartsWith("A11") || gate.GateName.StartsWith("A12") ||
                                                   gate.GateName.StartsWith("A13") || gate.GateName.StartsWith("A20") || gate.GateName.StartsWith("A21") ||
                                                   gate.GateName.StartsWith("A22") || gate.GateName.StartsWith("B10") || gate.GateName.StartsWith("B11") ||
                                                   gate.GateName.StartsWith("B12"))) ||
                        (specialCode == "CFFT" && (gate.GateName.StartsWith("B1") || gate.GateName.StartsWith("B2") || gate.GateName.StartsWith("B3") ||
                                                   gate.GateName.StartsWith("C"))) ||
                        (specialCode == "LWTT" && (gate.GateName.StartsWith("A1") || gate.GateName.StartsWith("A2") || gate.GateName.StartsWith("A20") ||
                                                   gate.GateName.StartsWith("A21") || gate.GateName.StartsWith("A22") || gate.GateName.StartsWith("C14") ||
                                                   gate.GateName.StartsWith("C15") || gate.GateName.StartsWith("C16") || gate.GateName.StartsWith("B"))))
                    {
                        assignedGate = gate;
                        break;
                    }
                }
                else
                {
                    // If no special request code, assign any unassigned gate
                    assignedGate = gate;
                    break;
                }
            }
        }

        if (assignedGate != null)
        {
            assignedGate.Flight = currentFlight;
            processedFlights++;
            processedGates++;

            // Display the flight details
            Console.WriteLine($"Flight Number: {currentFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {terminal.GetAirlineFromFlight(currentFlight)?.Name ?? "Unknown"}");
            Console.WriteLine($"Origin: {currentFlight.Origin}");
            Console.WriteLine($"Destination: {currentFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {currentFlight.ExpectedTime}");
            Console.WriteLine($"Special Request Code: {specialCode}");
            Console.WriteLine($"Boarding Gate: {assignedGate.GateName}");
            Console.WriteLine("----------------------------------------");
        }
    }

    // Step 4: Display the results
    Console.WriteLine($"Total number of Flights processed and assigned: {processedFlights}");
    Console.WriteLine($"Total number of Boarding Gates processed and assigned: {processedGates}");

    int totalFlights = terminal.Flights.Count;
    int totalGates = terminal.BoardingGates.Count;

    double flightPercentage = (double)processedFlights / totalFlights * 100;
    double gatePercentage = (double)processedGates / totalGates * 100;

    Console.WriteLine($"Percentage of Flights processed automatically: {flightPercentage:F2}%");
    Console.WriteLine($"Percentage of Boarding Gates processed automatically: {gatePercentage:F2}%");
}

void DisplayTotalFeePerAirline(Terminal terminal)
{
    // Check for unassigned flights (existing code remains)
    bool allFlightsAssigned = true;
    foreach (var flight in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (var gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            {
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned)
        {
            Console.WriteLine($"Flight {flight.FlightNumber} does not have a boarding gate assigned.");
            allFlightsAssigned = false;
        }
    }

    if (!allFlightsAssigned)
    {
        Console.WriteLine("Please ensure all flights have boarding gates assigned before running this feature.");
        return;
    }

    // Display per-airline fees using Terminal's method
    terminal.PrintAirlineFees();

    // Calculate overall totals
    double totalSubtotalFees = 0;
    double totalSubtotalDiscounts = 0;
    double totalFinalFees = 0;

    foreach (var airline in terminal.Airlines.Values)
    {
        var (subtotal, discounts, final) = airline.CalculateFees();
        totalSubtotalFees += subtotal;
        totalSubtotalDiscounts += discounts;
        totalFinalFees += final;
    }

    // Display totals
    Console.WriteLine($"Total Subtotal Fees for All Airlines: ${totalSubtotalFees:F2}");
    Console.WriteLine($"Total Subtotal Discounts for All Airlines: ${totalSubtotalDiscounts:F2}");
    Console.WriteLine($"Total Final Fees for All Airlines: ${totalFinalFees:F2}");

    if (totalFinalFees != 0)
    {
        double discountPercentage = (totalSubtotalDiscounts / totalFinalFees) * 100;
        Console.WriteLine($"Percentage of Discounts over Final Fees: {discountPercentage:F2}%");
    }
    else
    {
        Console.WriteLine("Percentage of Discounts: N/A (No fees)");
    }
}

void Display_Menu()
{
    Console.WriteLine();
    Console.WriteLine(@"=============================================
Welcome to Changi Airport Terminal 5
=============================================
1. List All Flights
2. List Boarding Gates
3. Assign a Boarding Gate to a Flight
4. Create Flight
5. Display Airline Flights
6. Modify Flight Details
7. Display Flight Schedule
8. Process All Unassign Flights
9. Display Total Fees per Airline
0. Exit

Please select your option:");
}
Terminal terminal = new Terminal("Terminal 5");
loading_Airlines(terminal);
loading_BoardingGates(terminal);
loading_flights(terminal);

while (true)
{
    int? option = null;
    try
    {
        Display_Menu();
        option = Convert.ToInt32(Console.ReadLine());
        if (option == 1)
            List_all_flights(terminal);
        else if (option == 2)
            List_all_gates(terminal);
        else if (option == 3)
            Assign_boardinggate(terminal);
        else if (option == 4)
            Create_flight(terminal);
        else if (option == 5)
            Display_Airline_Flights(terminal);
        else if (option == 6)
            Modify_Flight_Details(terminal);
        else if (option == 7)
            Display_Flight_Schedule(terminal);
        else if (option == 8)
            ProcessUnassignedFlights(terminal);
        else if (option == 9)
            DisplayTotalFeePerAirline(terminal);
        else if (option == 0)
            break;
        else
            Console.WriteLine("Invalid option! Please try again.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
