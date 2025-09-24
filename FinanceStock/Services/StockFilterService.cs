using FinanceStock.Models;

namespace FinanceStock.Services;

public class StockFilterService
{
    private readonly StockAnalyzer _analyzer;
    private readonly StockDbContext _dbContext;
    private readonly StockDataService _stockDataService;
    public StockFilterService(StockAnalyzer analyzer, StockDbContext dbContext, StockDataService stockDataService)
    {
        _analyzer = analyzer;
        _dbContext = dbContext;
        _stockDataService = stockDataService;
    }

    public async Task FilterStocksDaily(List<string> symbols)
    {
        foreach (var symbol in symbols)
        {
            // Kiểm tra xem đã phân tích cho ngày hiện tại chưa
            var existing = _dbContext.GoodStocks
                .Any(g => g.Symbol == symbol && g.Date == DateTime.Today);

            if (!existing)
            {
                // Phân tích bằng Grok
                var result = await _analyzer.AnalyzeStock(symbol);
                if (result.IsGood)
                {
                    // Lấy giá đóng cửa mới nhất từ StockData
                    var stockData = await _stockDataService.GetDailyData(symbol);
                    var latestClose = stockData.OrderByDescending(s => s.Date).FirstOrDefault()?.Close;

                    _dbContext.GoodStocks.Add(new GoodStock
                    {
                        Symbol = symbol,
                        Date = DateTime.Today,
                        Reason = result.Reason,
                        ClosePrice = latestClose
                    });
                }
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}