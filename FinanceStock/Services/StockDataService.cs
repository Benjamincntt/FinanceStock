using System.Text.Json;
using FinanceStock.Models;

namespace FinanceStock.Services;

public class StockDataService
{
    private readonly HttpClient _httpClient;
    private const string AlphaVantageKey = "YOUR_ALPHA_VANTAGE_KEY";

    public StockDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<StockData>> GetDailyData(string symbol)
    {
        var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={AlphaVantageKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return ParseStockData(jsonResponse, symbol);
    }

    private List<StockData> ParseStockData(string json, string symbol)
    {
        var stockDataList = new List<StockData>();
        var jsonDoc = JsonDocument.Parse(json);
        var timeSeries = jsonDoc.RootElement.GetProperty("Time Series (Daily)");

        foreach (var day in timeSeries.EnumerateObject())
        {
            var date = DateTime.Parse(day.Name);
            var data = day.Value;

            stockDataList.Add(new StockData
            {
                Symbol = symbol,
                Date = date,
                Open = data.GetProperty("1. open").GetDouble(),
                High = data.GetProperty("2. high").GetDouble(),
                Low = data.GetProperty("3. low").GetDouble(),
                Close = data.GetProperty("4. close").GetDouble(),
                Volume = data.GetProperty("5. volume").GetDouble()
            });
        }

        return stockDataList;
    }
}