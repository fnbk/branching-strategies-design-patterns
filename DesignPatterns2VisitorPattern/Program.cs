﻿
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

    public void Accept(IWeatherVisitor visitor)
    {
        switch (Type)
        {
            case WeatherType.Storm:
                visitor.VisitStorm(this);
                break;
            case WeatherType.Heatwave:
                visitor.VisitHeatwave(this);
                break;
            // ...
        }
    }
}

public interface IWeatherVisitor
{
    void VisitStorm(WeatherData weatherData);
    void VisitHeatwave(WeatherData weatherData);
}

public class WeatherAlertVisitor : IWeatherVisitor
{
    public void VisitStorm(WeatherData weatherData)
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

    public void VisitHeatwave(WeatherData weatherData)
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
    public void ProcessWeatherAlert(WeatherData weatherData)
    {
        if (!weatherData.IsValid)
        {
            Console.WriteLine("Invalid weather data.");
            return;
        }

        var visitor = new WeatherAlertVisitor();

        weatherData.Accept(visitor);
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