using Kros.Framework.Modules;

public class Product : Entity<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal PurchasePrice { get; set; }
    public string PictureFileName { get; set; }
    public string ProductType { get; set; }
    public string ProductBrand { get; set; }
    public int AvailableStock { get; set; }
}
<<<<<<< HEAD
=======

public class ProductDto : OutputDto<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PictureFileName { get; set; }
    public string ProductType { get; set; }
    public string ProductBrand { get; set; }
    public int AvailableStock { get; set; }
}
>>>>>>> 1a354376180820a7389fdb0ecd42eaf3f1003b1d
