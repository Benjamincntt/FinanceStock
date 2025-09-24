namespace FinanceStock.Models;

public class GoodStock
{
    public int Id { get; set; } // Primary key
    public string Symbol { get; set; } // Mã cổ phiếu (VD: AAPL, VNM)
    public DateTime Date { get; set; } // Ngày phân tích
    public string Reason { get; set; } // Lý do được đánh giá là tốt (từ Grok)
    public double? ClosePrice { get; set; } // Giá đóng cửa (tùy chọn, để hiển thị thêm thông tin)
}