using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PETSHOP.Models
{
    public partial class PETSHOPContext : DbContext
    {
        public PETSHOPContext()
        {
        }

        public PETSHOPContext(DbContextOptions<PETSHOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountManage> AccountManage { get; set; }
        public virtual DbSet<AccountRole> AccountRole { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<BillDetail> BillDetail { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CostumeProduct> CostumeProduct { get; set; }
        public virtual DbSet<CustomerType> CustomerType { get; set; }
        public virtual DbSet<DeliveryProduct> DeliveryProduct { get; set; }
        public virtual DbSet<DeliveryProductState> DeliveryProductState { get; set; }
        public virtual DbSet<DeliveryProductType> DeliveryProductType { get; set; }
        public virtual DbSet<Distributor> Distributor { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<FoodProduct> FoodProduct { get; set; }
        public virtual DbSet<InventoryReceivingNote> InventoryReceivingNote { get; set; }
        public virtual DbSet<InventoryReceivingNoteDetailForCostume> InventoryReceivingNoteDetailForCostume { get; set; }
        public virtual DbSet<InventoryReceivingNoteDetailForFood> InventoryReceivingNoteDetailForFood { get; set; }
        public virtual DbSet<InventoryReceivingNoteDetailForToy> InventoryReceivingNoteDetailForToy { get; set; }
        public virtual DbSet<PaymentMethodType> PaymentMethodType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ToyProduct> ToyProduct { get; set; }
        public virtual DbSet<UserComment> UserComment { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserScore> UserScore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["PETSHOP"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasColumnName("Account_Password")
                    .IsUnicode(false);

                entity.Property(e => e.AccountRoleId).HasColumnName("AccountRole_ID");

                entity.Property(e => e.AccountUserName)
                    .IsRequired()
                    .HasColumnName("Account_UserName")
                    .HasMaxLength(30);

                entity.Property(e => e.IsLoginExternal).HasDefaultValueSql("((0))");

                entity.Property(e => e.Jwtoken).HasColumnName("JWToken");

                entity.HasOne(d => d.AccountRole)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccountRoleId)
                    .HasConstraintName("FK_Account_AccountRole");
            });

            modelBuilder.Entity<AccountManage>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AccountRoleId).HasColumnName("AccountRole_ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.IsActivated).HasDefaultValueSql("((0))");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.HasOne(d => d.AccountRole)
                    .WithMany(p => p.AccountManage)
                    .HasForeignKey(d => d.AccountRoleId)
                    .HasConstraintName("FK_AccountManage_AccountRole");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.Property(e => e.AccountRoleId)
                    .HasColumnName("AccountRole_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountRoleDelete).HasColumnName("AccountRole_Delete");

                entity.Property(e => e.AccountRoleInsert).HasColumnName("AccountRole_Insert");

                entity.Property(e => e.AccountRoleName)
                    .HasColumnName("AccountRole_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.AccountRoleReadApi).HasColumnName("AccountRole_ReadAPI");

                entity.Property(e => e.AccountRoleUpdate).HasColumnName("AccountRole_Update");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(e => e.BillId).HasColumnName("Bill_ID");

                entity.Property(e => e.DateOfDelivered).HasColumnType("datetime");

                entity.Property(e => e.DateOfPurchase).HasColumnType("datetime");

                entity.Property(e => e.GenerateCodeCheck).IsUnicode(false);

                entity.Property(e => e.IsApprove).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCancel).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCompleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDelivery).HasDefaultValueSql("((0))");

                entity.Property(e => e.PaymentMethodTypeId).HasColumnName("PaymentMethodType_ID");

                entity.Property(e => e.UserProfileId).HasColumnName("UserProfile_ID");

                entity.HasOne(d => d.PaymentMethodType)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.PaymentMethodTypeId)
                    .HasConstraintName("FK_Bill_PaymentMethodType");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.Property(e => e.BillDetailId).HasColumnName("BillDetail_ID");

                entity.Property(e => e.BillId).HasColumnName("Bill_ID");

                entity.Property(e => e.NoteSize)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductAmount).HasColumnName("Product_Amount");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.ProductPriceCurrent).HasColumnName("Product_PriceCurrent");

                entity.Property(e => e.ProductTotalPrice).HasColumnName("Product_TotalPrice");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillDetail)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK_BillDetail_Bill");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BillDetail)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_BillDetail_Product");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<CostumeProduct>(entity =>
            {
                entity.HasKey(e => e.CostumeId);

                entity.Property(e => e.CostumeId).HasColumnName("CostumeID");

                entity.Property(e => e.CostumeSize)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CostumeProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CostumeProduct_Product");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerType_ID");

                entity.Property(e => e.CustomerTypeName)
                    .HasColumnName("CustomerType_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CustomerTypeScore).HasColumnName("CustomerType_Score");
            });

            modelBuilder.Entity<DeliveryProduct>(entity =>
            {
                entity.Property(e => e.DeliveryProductId).HasColumnName("DeliveryProduct_ID");

                entity.Property(e => e.DeliveryProductAddress).HasColumnName("DeliveryProduct_Address");

                entity.Property(e => e.DeliveryProductBillId).HasColumnName("DeliveryProduct_Bill_ID");

                entity.Property(e => e.DeliveryProductNote).HasColumnName("DeliveryProduct_Note");

                entity.Property(e => e.DeliveryProductPhoneNumber)
                    .HasColumnName("DeliveryProduct_PhoneNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryProductStateId).HasColumnName("DeliveryProductState_ID");

                entity.Property(e => e.DeliveryProductTypeId).HasColumnName("DeliveryProductType_ID");

                entity.HasOne(d => d.DeliveryProductBill)
                    .WithMany(p => p.DeliveryProduct)
                    .HasForeignKey(d => d.DeliveryProductBillId)
                    .HasConstraintName("FK_DeliveryProduct_Bill");

                entity.HasOne(d => d.DeliveryProductState)
                    .WithMany(p => p.DeliveryProduct)
                    .HasForeignKey(d => d.DeliveryProductStateId)
                    .HasConstraintName("FK_DeliveryProduct_DeliveryProduct_State");

                entity.HasOne(d => d.DeliveryProductType)
                    .WithMany(p => p.DeliveryProduct)
                    .HasForeignKey(d => d.DeliveryProductTypeId)
                    .HasConstraintName("FK_DeliveryProduct_DeliveryProductType");
            });

            modelBuilder.Entity<DeliveryProductState>(entity =>
            {
                entity.ToTable("DeliveryProduct_State");

                entity.Property(e => e.DeliveryProductStateId).HasColumnName("DeliveryProductState_ID");

                entity.Property(e => e.DeliveryProductStateName)
                    .HasColumnName("DeliveryProductState_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DeliveryProductType>(entity =>
            {
                entity.Property(e => e.DeliveryProductTypeId).HasColumnName("DeliveryProductType_ID");

                entity.Property(e => e.DeliveryProductTypeName)
                    .HasColumnName("DeliveryProductType_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.DeliveryProductTypePrice).HasColumnName("DeliveryProductType_Price");
            });

            modelBuilder.Entity<Distributor>(entity =>
            {
                entity.Property(e => e.DistributorId).HasColumnName("Distributor_ID");

                entity.Property(e => e.DistributorAddress).HasColumnName("Distributor_Address");

                entity.Property(e => e.DistributorName)
                    .IsRequired()
                    .HasColumnName("Distributor_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.DistributorPhoneNumber)
                    .HasColumnName("Distributor_PhoneNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("Created_At")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.Subject).HasMaxLength(100);
            });

            modelBuilder.Entity<FoodProduct>(entity =>
            {
                entity.HasKey(e => e.FoodId);

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.FoodExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FoodProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_FoodProduct_Product");
            });

            modelBuilder.Entity<InventoryReceivingNote>(entity =>
            {
                entity.HasKey(e => e.InventoryReceivingId)
                    .HasName("PK__Inventor__1A7F13692EEEF348");

                entity.Property(e => e.InventoryReceivingId).HasColumnName("InventoryReceiving_ID");

                entity.Property(e => e.InventoryReceivingDateReceiving)
                    .HasColumnName("InventoryReceiving_DateReceiving")
                    .HasColumnType("datetime");

                entity.Property(e => e.InventoryReceivingTotalPrice).HasColumnName("InventoryReceiving_TotalPrice");
            });

            modelBuilder.Entity<InventoryReceivingNoteDetailForCostume>(entity =>
            {
                entity.HasKey(e => new { e.InventoryReceivingId, e.CostumeProductId });

                entity.ToTable("InventoryReceivingNoteDetail_ForCostume");

                entity.Property(e => e.InventoryReceivingId).HasColumnName("InventoryReceiving_ID");

                entity.Property(e => e.CostumeProductId).HasColumnName("CostumeProduct_ID");

                entity.Property(e => e.CostumeProductAmount).HasColumnName("CostumeProduct_Amount");

                entity.Property(e => e.CostumeProductPrice).HasColumnName("CostumeProduct_Price");

                entity.Property(e => e.CostumeProductSize)
                    .HasColumnName("CostumeProduct_Size")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.HasOne(d => d.InventoryReceiving)
                    .WithMany(p => p.InventoryReceivingNoteDetailForCostume)
                    .HasForeignKey(d => d.InventoryReceivingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryReceivingNoteDetail_ForCostume_InventoryReceivingNote");
            });

            modelBuilder.Entity<InventoryReceivingNoteDetailForFood>(entity =>
            {
                entity.HasKey(e => new { e.InventoryReceivingId, e.FoodProductId });

                entity.ToTable("InventoryReceivingNoteDetail_ForFood");

                entity.Property(e => e.InventoryReceivingId).HasColumnName("InventoryReceiving_ID");

                entity.Property(e => e.FoodProductId).HasColumnName("FoodProduct_ID");

                entity.Property(e => e.FoodProductAmount).HasColumnName("FoodProduct_Amount");

                entity.Property(e => e.FoodProductPrice).HasColumnName("FoodProduct_Price");

                entity.HasOne(d => d.InventoryReceiving)
                    .WithMany(p => p.InventoryReceivingNoteDetailForFood)
                    .HasForeignKey(d => d.InventoryReceivingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryReceivingNoteDetail_ForFood_InventoryReceivingNote");
            });

            modelBuilder.Entity<InventoryReceivingNoteDetailForToy>(entity =>
            {
                entity.HasKey(e => new { e.InventoryReceivingId, e.ToyProductId });

                entity.ToTable("InventoryReceivingNoteDetail_ForToy");

                entity.Property(e => e.InventoryReceivingId).HasColumnName("InventoryReceiving_ID");

                entity.Property(e => e.ToyProductId).HasColumnName("ToyProduct_ID");

                entity.Property(e => e.ToyProductAmount).HasColumnName("ToyProduct_Amount");

                entity.Property(e => e.ToyProductPrice).HasColumnName("ToyProduct_Price");

                entity.HasOne(d => d.InventoryReceiving)
                    .WithMany(p => p.InventoryReceivingNoteDetailForToy)
                    .HasForeignKey(d => d.InventoryReceivingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryReceivingNoteDetail_ForToy_InventoryReceivingNote");
            });

            modelBuilder.Entity<PaymentMethodType>(entity =>
            {
                entity.Property(e => e.PaymentMethodTypeId)
                    .HasColumnName("PaymentMethodType_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PaymentMethodTypeName)
                    .HasColumnName("PaymentMethodType_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.DistributorId).HasColumnName("Distributor_ID");

                entity.Property(e => e.InitAt)
                    .HasColumnName("Init_At")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsActivated).HasDefaultValueSql("((0))");

                entity.Property(e => e.NumberOfPurchases).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductImage)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.SlugName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

                entity.HasOne(d => d.Distributor)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.DistributorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Distributor");
            });

            modelBuilder.Entity<ToyProduct>(entity =>
            {
                entity.HasKey(e => e.ToyId);

                entity.Property(e => e.ToyId).HasColumnName("ToyID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ToyProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ToyProduct_Product");
            });

            modelBuilder.Entity<UserComment>(entity =>
            {
                entity.Property(e => e.UserCommentId).HasColumnName("UserComment_ID");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.UserCommentApproved).HasColumnName("UserComment_Approved");

                entity.Property(e => e.UserCommentContent).HasColumnName("UserComment_Content");

                entity.Property(e => e.UserCommentPostedDate)
                    .HasColumnName("UserComment_PostedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserProfileId).HasColumnName("UserProfile_ID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.UserComment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserComment_Product");

                entity.HasOne(d => d.UserProfile)
                    .WithMany(p => p.UserComment)
                    .HasForeignKey(d => d.UserProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserComment_UserProfile");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.Property(e => e.UserProfileId).HasColumnName("UserProfile_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerType_ID");

                entity.Property(e => e.UserProfileAddress).HasColumnName("UserProfile_Address");

                entity.Property(e => e.UserProfileAvatar)
                    .HasColumnName("UserProfile_Avatar")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileDob)
                    .HasColumnName("UserProfile_DOB")
                    .HasColumnType("date");

                entity.Property(e => e.UserProfileEmail)
                    .HasColumnName("UserProfile_Email")
                    .HasMaxLength(50);

                entity.Property(e => e.UserProfileFirstName)
                    .HasColumnName("UserProfile_FirstName")
                    .HasMaxLength(30);

                entity.Property(e => e.UserProfileIdentityCard)
                    .HasColumnName("UserProfile_IdentityCard")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileLastName)
                    .HasColumnName("UserProfile_LastName")
                    .HasMaxLength(30);

                entity.Property(e => e.UserProfileMiddleName)
                    .HasColumnName("UserProfile_MiddleName")
                    .HasMaxLength(30);

                entity.Property(e => e.UserProfilePhoneNumber)
                    .HasColumnName("UserProfile_PhoneNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.UserProfile)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_UserProfile_Account");

                entity.HasOne(d => d.CustomerType)
                    .WithMany(p => p.UserProfile)
                    .HasForeignKey(d => d.CustomerTypeId)
                    .HasConstraintName("FK_UserProfile_CustomerType");
            });

            modelBuilder.Entity<UserScore>(entity =>
            {
                entity.Property(e => e.UserScoreId).HasColumnName("UserScore_ID");

                entity.Property(e => e.UserCurrentScore).HasColumnName("User_CurrentScore");

                entity.Property(e => e.UserProfileId).HasColumnName("UserProfile_ID");

                entity.HasOne(d => d.UserProfile)
                    .WithMany(p => p.UserScore)
                    .HasForeignKey(d => d.UserProfileId)
                    .HasConstraintName("FK_UserScore_UserProfile");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
