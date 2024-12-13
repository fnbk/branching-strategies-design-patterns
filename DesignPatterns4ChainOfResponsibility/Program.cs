
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

public abstract class WeatherHandler
{
    protected WeatherHandler _nextHandler;

    public void SetNextHandler(WeatherHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public abstract void HandleRequest(WeatherData weatherData);
}

public class StormWeatherHandler : WeatherHandler
{
    public override void HandleRequest(WeatherData weatherData)
    {
        if (weatherData.Type == WeatherType.Storm)
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
        else
        {
            _nextHandler?.HandleRequest(weatherData);
        }
    }
}

public class HeatwaveWeatherHandler : WeatherHandler
{
    public override void HandleRequest(WeatherData weatherData)
    {
        if (weatherData.Type == WeatherType.Heatwave)
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
        else
        {
            _nextHandler?.HandleRequest(weatherData);
        }
    }
}

public class InvalidWeatherDataHandler : WeatherHandler
{
    public override void HandleRequest(WeatherData weatherData)
    {
        if (!weatherData.IsValid)
        {
            Console.WriteLine("Invalid weather data.");
        }
        else
        {
            _nextHandler?.HandleRequest(weatherData);
        }
    }
}

public class WeatherAlertProcessor
{
    private readonly WeatherHandler _handlerChain;

    public WeatherAlertProcessor()
    {
        // Initialize the chain
        _handlerChain = new InvalidWeatherDataHandler();

        var stormHandler = new StormWeatherHandler();
        var heatwaveHandler = new HeatwaveWeatherHandler();

        // Construct the chain of responsibility
        _handlerChain.SetNextHandler(stormHandler);
        stormHandler.SetNextHandler(heatwaveHandler);
    }

    public void ProcessWeatherAlert(WeatherData weatherData)
    {
        // Start processing from the beginning of the chain
        _handlerChain.HandleRequest(weatherData);
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

