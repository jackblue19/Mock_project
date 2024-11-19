using System;
using System.Collections.Generic;

namespace ZetstyBite.Models.DTOs;

public class ItemDTO
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = null!;

    public string ItemCategory { get; set; } = null!;

    public ulong ItemStatus { get; set; } = 1;

    public string ItemDescription { get; set; } = null!;

    public decimal SuggestedPrice { get; set; }

    public string ItemImage { get; set; } = null!;

    public ulong IsServed { get; set; } = 0;

    public int TableId{get; set;}

    public ulong TableType { get; set; } = 0;
    
    public string TableStatus{get; set; } = null!;

    // noting => cần xme lại quan hệ tam phân ??
    // chú ý mối liên quan giữa item - table - reservation - account => T11_Table_Details??

}
