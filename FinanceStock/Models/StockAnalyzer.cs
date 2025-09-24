using System.Text.Json;
using FinanceStock.Services;

namespace FinanceStock.Models;

public class StockAnalyzer
{
    private readonly GrokService _grokService;
    private readonly StockDataService _stockDataService;

    public StockAnalyzer(GrokService grokService, StockDataService stockDataService)
    {
        _grokService = grokService;
        _stockDataService = stockDataService;
    }

    public async Task<StockAnalysisResult> AnalyzeStock(string symbol)
    {
        // Lấy dữ liệu 30 ngày gần nhất
        var stockData = await _stockDataService.GetDailyData(symbol);
        var dataJson = JsonSerializer.Serialize(stockData);

        // Tạo prompt cho Grok
        var prompt = $@"Analyze technical indicators for {symbol} with the following OHLC data (30 days): {dataJson}. 
Calculate:
1. RSI (14-day)
2. MACD (12,26,9)
3. Check if the latest closing price is above 50-day SMA
If RSI < 30 or a bullish MACD crossover occurs, mark the stock as 'good'. 
Return JSON: {{'isGood': boolean, 'reason': string}}";

        // Gọi Grok API
        var grokResponse = await _grokService.AnalyzeStockData(symbol, prompt);
        return JsonSerializer.Deserialize<StockAnalysisResult>(grokResponse);
    }
}
public class StockAnalysisResult
{
    public bool IsGood { get; set; }
    public string Reason { get; set; }
}