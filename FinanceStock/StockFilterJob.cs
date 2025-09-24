using FinanceStock.Services;

namespace FinanceStock;

public class StockFilterJob
{
    private readonly StockFilterService _filterService;

    public StockFilterJob(StockFilterService filterService)
    {
        _filterService = filterService;
    }

    public async Task RunDaily()
    {
        var symbols = new List<string> { "VN30", "HOSE" }; // Thay bằng VN30, HOSE, hoặc danh sách bạn muốn
        await _filterService.FilterStocksDaily(symbols);
    }
}