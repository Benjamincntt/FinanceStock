using Microsoft.AspNetCore.Mvc;

namespace FinanceStock.Controllers;
[ApiController]
[Route("api/stocks")]
public class StocksController : ControllerBase
{
    private readonly StockDbContext _dbContext;

    public StocksController(StockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("good-stocks")]
    public IActionResult GetGoodStocks(DateTime date)
    {
        var stocks = _dbContext.GoodStocks
            .Where(s => s.Date == date.Date)
            .Select(s => new
            {
                s.Symbol,
                s.Reason,
                s.ClosePrice
            })
            .ToList();
        return Ok(stocks);
    }
}