using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// </summary>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            // Validate API key
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }

            // Build full request URL
            string url = $"{baseUrl}?q={city}&appid={ApiConfig.OpenWeatherMapApiKey}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                try
                {
                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                        await Task.Yield();

                    switch (request.result)
                    {
                        case UnityWebRequest.Result.Success:
                            try
                            {
                                // Deserialize JSON response
                                var weatherData = JsonConvert.DeserializeObject<WeatherData>(request.downloadHandler.text);
                                return weatherData;
                            }
                            catch (JsonException ex)
                            {
                                Debug.LogError($"JSON parsing failed: {ex.Message}");
                                return null;
                            }

                        case UnityWebRequest.Result.ConnectionError:
                            Debug.LogError($"Network connection error: {request.error}");
                            break;

                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError($"HTTP error {request.responseCode}: {request.error}");
                            break;

                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError($"Data processing error: {request.error}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request failed: {ex.Message}");
                }

                return null;
            }
        }
    }
}
