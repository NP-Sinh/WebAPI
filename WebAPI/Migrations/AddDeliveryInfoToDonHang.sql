-- Add delivery information columns to DonHang table
ALTER TABLE DonHang
ADD
    DiaChiGiaoHang NVARCHAR(200) NULL,
    SoDienThoai VARCHAR(20) NULL,
    GhiChu NVARCHAR(500) NULL; 