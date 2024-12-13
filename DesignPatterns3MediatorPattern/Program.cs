public enum WeatherType { Storm, Heatwave }
public enum Severity { High, Medium, Low }

public class WeatherData
{
    public bool IsValid { get; set; }
    public WeatherType Type { get; set; }
    public Severity Severity { get; set; }
    public bool IsApproaching { get; set; }
    public bool HasHail { get; set; }
    public int Temperature { get; set; }
    public int Humidity { get; set; }
}

public interface IWeatherAlertMediator
{
    void HandleWeatherAlert(WeatherData weatherData);
}

public class WeatherAlertMediator : IWeatherAlertMediator
{
    public void HandleWeatherAlert(WeatherData weatherData)
    {
        // The complex logic is now centralized within the mediator
        if (!weatherData.IsValid)
        {
            Console.WriteLine("Invalid weather data.");
            return;
        }

        switch (weatherData.Type)
        {
            case WeatherType.Storm:
                HandleStorm(weatherData);
                break;
            case WeatherType.Heatwave:
                HandleHeatwave(weatherData);
                break;
            default:
                Console.WriteLine("Additional weather types can be handled here.");
                break;
        }
    }

    private void HandleStorm(WeatherData weatherData)
    {
        Console.WriteLine("Handling storm alert.");
        if (weatherData.Severity == Severity.High)
        {
            Console.WriteLine("High severity storm detected.");
            if (weatherData.IsApproaching)
            {
                Console.WriteLine("Storm is approaching. Taking immediate actions.");
                if (weatherData.HasHail)
                {
                    Console.WriteLine("Hail detected. Handling hail-specific actions.");
                }
            }
        }
    }

    private void HandleHeatwave(WeatherData weatherData)
    {
        Console.WriteLine("Handling heatwave alert.");
        if (weatherData.Temperature > 100)
        {
            Console.WriteLine("Critical heatwave conditions with temperature over 100.");
            if (weatherData.Humidity > 60)
            {
                Console.WriteLine("High humidity encountered during heatwave. Taking additional precautions.");
            }
        }
    }
}

public class WeatherAlertProcessor
{
    private readonly IWeatherAlertMediator _mediator;

    public WeatherAlertProcessor(IWeatherAlertMediator mediator)
    {
        _mediator = mediator;
    }

    public void ProcessWeatherAlert(WeatherData weatherData)
    {
        _mediator.HandleWeatherAlert(weatherData);
    }
}


class Program
{
    static void Main()
    {
        IWeatherAlertMediator mediator = new WeatherAlertMediator();
        var processor = new WeatherAlertProcessor(mediator);

        Console.WriteLine("\n### High Severity Storm ###");
        var stormData = new WeatherData
        {
            IsValid = true,
            Type = WeatherType.Storm,
            Severity = Severity.High,
            IsApproaching = true,
            HasHail = true
        };
        processor.ProcessWeatherAlert(stormData);

        Console.WriteLine("\n### Critical Heatwave ###");
        var heatwaveData = new WeatherData
        {
            IsValid = true,
            Type = WeatherType.Heatwave,
            Temperature = 105,
            Humidity = 65
        };
        processor.ProcessWeatherAlert(heatwaveData);

        Console.WriteLine("\n### Invalid Weather Data ###");
        var invalidData = new WeatherData
        {
            IsValid = false
        };
        processor.ProcessWeatherAlert(invalidData);
    }
}


