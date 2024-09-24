using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.ProductService
{
    public class ProductReadDto : BaseReadDto<Product>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductCode { get; set; }


        public override void FromEntity(Product entity)
        {
            base.FromEntity(entity);
            Title = entity.Title;
            Description = entity.Description;
            Price = entity.Price;
            Stock = entity.Stock;
            CategoryId = entity.CategoryId;
            ProductCode = entity.ProductCode;
        }
    }
    public class ProductCreateDto : ICreateDto<Product>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductCode { get; set; }
        public Product CreateEntity()
        {
            return new Product
            {
                Title = Title,
                Description = Description,
                Price = Price,
                Stock = Stock,
                CategoryId = CategoryId,
                ProductCode = ProductCode,
            };
        }
    }
    public class ProductUpdateDto : IUpdateDto<Product>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public Product UpdateEntity(Product entity)
        {
            entity.Title = Title;
            entity.Description = Description;
            entity.Price = Price;
            entity.Stock = Stock;
            entity.CategoryId = CategoryId;
            return entity;
        }
    }
}