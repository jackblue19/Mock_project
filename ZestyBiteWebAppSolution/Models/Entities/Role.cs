using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Role
{
    public sbyte RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    // [JsonIgnore] // Bỏ qua danh sách Accounts khi serialize 
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    //  => dùng nudget & tag trên giúp bỏ qua lỗi lặp vô hạn json vì khoá ngoại giữa các bảng
}
