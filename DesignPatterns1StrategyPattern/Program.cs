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

public interface IWeatherAlertStrategy
{
    void HandleWeatherAlert(WeatherData weatherData);
}

public class StormAlertStrategy : IWeatherAlertStrategy
{
    public void HandleWeatherAlert(WeatherData weatherData)
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
}

public class HeatwaveAlertStrategy : IWeatherAlertStrategy
{
    public void HandleWeatherAlert(WeatherData weatherData)
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
    private readonly Dictionary<WeatherType, IWeatherAlertStrategy> _strategies;

    public WeatherAlertProcessor()
    {
        _strategies = new Dictionary<WeatherType, IWeatherAlertStrategy>
        {
            { WeatherType.Storm, new StormAlertStrategy() },
            { WeatherType.Heatwave, new HeatwaveAlertStrategy() },
            // ...
        };
    }

    public void ProcessWeatherAlert(WeatherData weatherData)
    {
        if (!weatherData.IsValid)
        {
            Console.WriteLine("Invalid weather data.");
            return;
        }

        if (_strategies.TryGetValue(weatherData.Type, out IWeatherAlertStrategy strategy))
        {
            strategy.HandleWeatherAlert(weatherData);
        }
        else
        {
            Console.WriteLine("No strategy found for the given weather type.");
        }
    }
}

class Program
{
    static void Main()
    {
        var processor = new WeatherAlertProcessor();

        Console.WriteLine("\n### High Severity Storm ###");
        var stormData = new WeatherData
        {
            IsValid = true, Type = WeatherType.Storm, Severity = Severity.High, IsApproaching = true, HasHail = true
        };
        processor.ProcessWeatherAlert(stormData);

        Console.WriteLine("\n### Critical Heatwave ###");
        var heatwaveData = new WeatherData
        {
            IsValid = true, Type = WeatherType.Heatwave, Temperature = 105, Humidity = 65
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