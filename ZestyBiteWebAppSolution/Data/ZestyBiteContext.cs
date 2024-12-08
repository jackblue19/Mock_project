using Microsoft.EntityFrameworkCore;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Data;

public partial class ZestyBiteContext : DbContext
{
    public ZestyBiteContext()
    {
    }

    public ZestyBiteContext(DbContextOptions<ZestyBiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Profit> Profits { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }

    public virtual DbSet<SupplyItem> SupplyItems { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<TableDetail> TableDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("account");

            entity.HasIndex(e => e.RoleId, "Role_ID");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.AccountStatus).HasColumnName("Account_Status");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.ProfileImage)
                .HasColumnType("text")
                .HasColumnName("Profile_Image");
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(50)
                .HasColumnName("Verification_Code");

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("account_ibfk_1");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PRIMARY");

            entity.ToTable("bill");

            entity.HasIndex(e => e.PaymentId, "Payment_ID");

            entity.HasIndex(e => e.TableId, "Table_ID");

            entity.Property(e => e.BillId).HasColumnName("Bill_ID");
            entity.Property(e => e.BillDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Bill_Datetime");
            entity.Property(e => e.BillStatus).HasColumnName("Bill_Status");
            entity.Property(e => e.BillType).HasColumnName("Bill_Type");
            entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");
            entity.Property(e => e.TableId).HasColumnName("Table_ID");
            entity.Property(e => e.TotalCost)
                .HasPrecision(14)
                .HasColumnName("Total_Cost");

            entity.HasOne(d => d.Payment).WithMany(p => p.Bills)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_ibfk_1");

            entity.HasOne(d => d.Table).WithMany(p => p.Bills)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bill_ibfk_2");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FbId).HasName("PRIMARY");

            entity.ToTable("feedback");

            entity.HasIndex(e => e.ItemId, "Item_ID");

            entity.HasIndex(e => e.ParentFbFlag, "ParentFb_Flag");

            entity.HasIndex(e => e.Username, "Username");

            entity.Property(e => e.FbId).HasColumnName("Fb_ID");
            entity.Property(e => e.FbContent)
                .HasMaxLength(255)
                .HasColumnName("Fb_Content");
            entity.Property(e => e.FbDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Fb_Datetime");
            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.ParentFbFlag).HasColumnName("ParentFb_Flag");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedback_ibfk_2");

            entity.HasOne(d => d.ParentFbFlagNavigation).WithMany(p => p.InverseParentFbFlagNavigation)
                .HasForeignKey(d => d.ParentFbFlag)
                .OnDelete(DeleteBehavior.Cascade) 
                .HasConstraintName("feedback_ibfk_3");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Feedbacks)
                .HasPrincipalKey(p => p.Username)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("feedback_ibfk_1");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("item");

            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.IsServed).HasColumnName("Is_Served");
            entity.Property(e => e.ItemCategory)
                .HasColumnType("enum('Pizza','Pasta','Dessert','Drink','Salad','Fruit','Burgers')")
                .HasColumnName("Item_Category");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(255)
                .HasColumnName("Item_Description");
            entity.Property(e => e.ItemImage)
                .HasColumnType("text")
                .HasColumnName("Item_Image");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("Item_Name");
            entity.Property(e => e.ItemStatus).HasColumnName("Item_Status");
            entity.Property(e => e.OriginalPrice)
                .HasPrecision(14)
                .HasColumnName("Original_Price");
            entity.Property(e => e.SuggestedPrice)
                .HasPrecision(14)
                .HasColumnName("Suggested_Price");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("payment");

            entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");
            entity.Property(e => e.PaymentMethod).HasColumnName("Payment_Method");
        });

        modelBuilder.Entity<Profit>(entity =>
        {
            entity.HasKey(e => e.Date).HasName("PRIMARY");

            entity.ToTable("profit");

            entity.HasIndex(e => e.BillId, "Bill_ID");

            entity.HasIndex(e => e.SupplyId, "Supply_ID");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.BillId).HasColumnName("Bill_ID");
            entity.Property(e => e.ProfitAmmount)
                .HasPrecision(10)
                .HasColumnName("Profit_Ammount");
            entity.Property(e => e.SupplyId).HasColumnName("Supply_ID");

            entity.HasOne(d => d.Bill).WithMany(p => p.Profits)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profit_ibfk_2");

            entity.HasOne(d => d.Supply).WithMany(p => p.Profits)
                .HasForeignKey(d => d.SupplyId)
                .HasConstraintName("profit_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.RoleDescription)
                .HasColumnType("enum('Manager','Order Taker','Procurement Manager','Server Staff','Customer Service Staff','Food Runner','Customer')")
                .HasColumnName("Role_Description");
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => e.SupplyId).HasName("PRIMARY");

            entity.ToTable("supply");

            entity.HasIndex(e => e.TableId, "Table_ID");

            entity.Property(e => e.SupplyId).HasColumnName("Supply_ID");
            entity.Property(e => e.DateExpiration)
                .HasColumnType("datetime")
                .HasColumnName("Date_Expiration");
            entity.Property(e => e.DateImport)
                .HasColumnType("datetime")
                .HasColumnName("Date_Import");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("Product_Name");
            entity.Property(e => e.SupplyCategory)
                .HasColumnType("enum('Food','Drink','Facility')")
                .HasColumnName("Supply_Category");
            entity.Property(e => e.SupplyPrice)
                .HasPrecision(14)
                .HasColumnName("Supply_Price");
            entity.Property(e => e.SupplyQuantity)
                .HasPrecision(10)
                .HasColumnName("Supply_Quantity");
            entity.Property(e => e.SupplyStatus).HasColumnName("Supply_Status");
            entity.Property(e => e.TableId).HasColumnName("Table_ID");
            entity.Property(e => e.VendorAddress)
                .HasMaxLength(255)
                .HasColumnName("Vendor_Address");
            entity.Property(e => e.VendorName)
                .HasMaxLength(255)
                .HasColumnName("Vendor_Name");
            entity.Property(e => e.VendorPhone)
                .HasMaxLength(255)
                .HasColumnName("Vendor_Phone");

            entity.HasOne(d => d.Table).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supply_ibfk_1");
        });

        modelBuilder.Entity<SupplyItem>(entity =>
        {
            entity.HasKey(e => new { e.SupplyId, e.ItemId }).HasName("PRIMARY");

            entity.ToTable("supply_item");

            entity.HasIndex(e => e.ItemId, "Item_ID");

            entity.Property(e => e.SupplyId).HasColumnName("Supply_ID");
            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.SupplyItemProfit)
                .HasPrecision(14)
                .HasColumnName("Supply_Item_Profit");

            entity.HasOne(d => d.Item).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supply_item_ibfk_2");

            entity.HasOne(d => d.Supply).WithMany(p => p.SupplyItems)
                .HasForeignKey(d => d.SupplyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supply_item_ibfk_1");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PRIMARY");

            entity.ToTable("table");

            entity.HasIndex(e => e.AccountId, "Account_ID");

            entity.Property(e => e.TableId).HasColumnName("Table_ID");
            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.TableCapacity).HasColumnName("Table_Capacity");
            entity.Property(e => e.TableMaintenance).HasColumnName("Table_Maintenance");
            entity.Property(e => e.TableNote)
                .HasMaxLength(255)
                .HasColumnName("Table_Note");
            entity.Property(e => e.TableStatus)
                .HasColumnType("enum('Served','Empty','Waiting')")
                .HasColumnName("Table_Status");
            entity.Property(e => e.TableType).HasColumnName("Table_Type");

            entity.HasOne(d => d.Account).WithMany(p => p.Tables)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_ibfk_1");
        });

        modelBuilder.Entity<TableDetail>(entity =>
        {
            entity.HasKey(e => new { e.TableId, e.ItemId, e.BillId }).HasName("PRIMARY");

            entity.ToTable("table_details");

            entity.HasIndex(e => e.BillId, "Bill_ID");

            entity.HasIndex(e => e.ItemId, "Item_ID");

            entity.Property(e => e.TableId).HasColumnName("Table_ID");
            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.BillId).HasColumnName("Bill_ID");

            entity.HasOne(d => d.Bill).WithMany(p => p.TableDetails)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_details_ibfk_3");

            entity.HasOne(d => d.Item).WithMany(p => p.TableDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_details_ibfk_2");

            entity.HasOne(d => d.Table).WithMany(p => p.TableDetails)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("table_details_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
