using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

// Модель даних
public class Flight
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public FlightStatus Status { get; set; }
    public TimeSpan Duration { get; set; }
    public string AircraftType { get; set; }
    public string Terminal { get; set; }
}

// Перерахування статусів рейсу
public enum FlightStatus
{
    OnTime,
    Delayed,
    Cancelled,
    Boarding,
    InFlight
}

// Основний клас системи, який обробляє вхідні дані
public class FlightInformationSystem
{
    private List<Flight> flights; // Список рейсів
    // Проміжний клас для десеріалізації даних з JSON файлу
    private class FlightsData
    {
        public List<Flight> Flights { get; set; }
    }
    // Конструктор
    public FlightInformationSystem()
    {
        flights = new List<Flight>();
    }
    // Завантаження рейсів з JSON файлу
    public void LoadFlightsFromJson(string jsonFile)
    {
        try
        {
            string jsonContent = File.ReadAllText(jsonFile); // Зчитування тексту з JSON файлу
            var flightsData = JsonConvert.DeserializeObject<FlightsData>(jsonContent); // Десериалізація даних файлу

            if (flightsData != null && flightsData.Flights != null)
            {
                flights.AddRange(flightsData.Flights); // Завантаження десериалізованих даних у вигляді списку об'єктів класу Flight
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights data: {ex.Message}"); // Повідомлення про помилку при зчитуванні файлу
        }
    }
    // Збереження даних в JSON файл
    public void SaveFlightsToJson(string jsonFile)
    {
        try
        {
            var flightsData = new FlightsData { Flights = flights };
            string jsonContent = JsonConvert.SerializeObject(flightsData, Formatting.Indented);
            File.WriteAllText(jsonFile, jsonContent);
            Console.WriteLine("Flights data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving flights data: {ex.Message}"); // Повідомлення про помилку при збереженні даних
        }
    }
    // Додавання нового рейсу
    public void AddFlight(Flight flight)
    {
        flights.Add(flight);
    }
    // Видалення рейсу за номером
    public void RemoveFlight(string flightNumber)
    {
        flights.RemoveAll(flight => flight.FlightNumber == flightNumber);
    }
    // Отримання списку рейсів
    public List<Flight> GetFlights()
    {
        return flights;
    }
    // Відображення списку рейсів
    public void DisplayFlights(List<Flight> flights)
    {
        if (flights == null || flights.Count == 0)
        {
            Console.WriteLine("No flights available.");
            return;
        }

        Console.WriteLine("Flight number |   Departure time    |     Arrival time    | Terminal | Status");
        Console.WriteLine("-----------------------------------------------------------------------------");
        foreach (var flight in flights)
        {
            Console.WriteLine($"    {flight.FlightNumber}     | {flight.DepartureTime} | {flight.ArrivalTime} |    {flight.Terminal}     | {flight.Status}");
        }
    }
    // Отримання рейсів за авіакомпанією
    public List<Flight> GetFlightsByAirline(string airline)
    {
        return flights.FindAll(flight => flight.Airline == airline).OrderBy(flight => flight.DepartureTime).ToList();
    }
    // Отримання рейсів з затримкою
    public List<Flight> GetDelayedFlights()
    {
        return flights.FindAll(flight => flight.Status == FlightStatus.Delayed).OrderBy(flight => flight.DepartureTime).ToList();
    }
    // Отримання рейсів за датою відбуття
    public List<Flight> GetFlightsByDepartureDate(DateTime departureDate)
    {
        return flights.FindAll(flight => flight.DepartureTime.Date == departureDate.Date).OrderBy(flight => flight.DepartureTime).ToList();
    }
    // Отримання рейсів за призначенням
    public List<Flight> GetFlightsByDestination(string destination)
    {
        return flights.FindAll(flight => flight.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).OrderBy(flight => flight.DepartureTime).ToList();
    }
    // Отримання рейсів за певний період часу
    public List<Flight> GetFlightsArrivedInTimeRange(DateTime startTime, DateTime endTime)
    {
        return flights.FindAll(flight => (flight.ArrivalTime >= startTime && flight.ArrivalTime <= endTime)).OrderBy(flight => flight.ArrivalTime).ToList();
    }
}
class Program
{
    static void Main()
    {
        var flightSystem = new FlightInformationSystem();

        flightSystem.LoadFlightsFromJson("FILE_PATH");
        flightSystem.SaveFlightsToJson("SAVE_PATH");

        flightSystem.DisplayFlights(flightSystem.GetFlights());

        //var newFlight = new Flight
        //{
        //    FlightNumber = "AA123",
        //    Airline = "WizAir",
        //    Destination = "Paris",
        //    DepartureTime = DateTime.Now,
        //    ArrivalTime = DateTime.Now.AddHours(2),
        //    Status = FlightStatus.OnTime,
        //    Duration = TimeSpan.FromHours(2),
        //    AircraftType = "Airbus A320",
        //    Terminal = "2"
        //};
        //flightSystem.AddFlight(newFlight);

        //var wizAirFlights = flightSystem.GetFlightsByAirline("WizAir");
        //flightSystem.DisplayFlights(wizAirFlights);
        //Console.WriteLine();

        //var flightsByDate = flightSystem.GetFlightsByDepartureDate(new DateTime(2023, 06, 12));
        //flightSystem.DisplayFlights(flightsByDate);
        //Console.WriteLine();

        //var flightsByDestignation = flightSystem.GetFlightsByDestination("Barcelona");
        //flightSystem.DisplayFlights(flightsByDestignation);
        //Console.WriteLine();

        //var startTime = new DateTime(2023, 05, 30, 0, 0, 1);
        //var endTime = new DateTime(2023, 05, 31, 23, 59, 59);

        //var flightsArrivedInTimeRange = flightSystem.GetFlightsArrivedInTimeRange(startTime, endTime);
        //flightSystem.DisplayFlights(flightsArrivedInTimeRange);
    }
}
