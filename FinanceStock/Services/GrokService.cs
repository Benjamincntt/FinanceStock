using System.Text;
using System.Text.Json;

namespace FinanceStock.Services;

public class GrokService
{
    private readonly HttpClient _httpClient;
    private const string GrokApiUrl = "https://api.x.ai/grok"; // URL giả định, thay bằng URL thật
    private const string ApiKey = "YOUR_GROK_API_KEY";

    public GrokService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    }

    public async Task<string> AnalyzeStockData(string symbol, string prompt)
    {
        var requestBody = new { prompt = prompt }; // Prompt ví dụ: "Phân tích RSI và MACD cho AAPL dựa trên dữ liệu gần đây"
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(GrokApiUrl, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    } 
}