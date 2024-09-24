using Ecommerce.Domain.src.ProductAggregate;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.ProductImageService
{
    public class ProductImageReadDto : BaseReadDto<ProductImage>
    {
        public Guid ProductId { get; set; }
        public string ImageURL { get; set; }
        public bool IsPrimary { get; set; }
        public string? ImageText { get; set; }

        public override void FromEntity(ProductImage entity)
        {
            base.FromEntity(entity);
            ProductId = entity.ProductId;
            ImageURL = entity.ImageURL;
            IsPrimary = entity.IsPrimary;
            ImageText = entity.ImageText;
        }
    }
    public class ProductImageCreateDto : ICreateDto<ProductImage>
    {
        public Guid ProductId { get; set; }
        public string ImageURL { get; set; }
        public bool IsPrimary { get; set; }
        public string? ImageText { get; set; }
        public ProductImage CreateEntity()
        {
            return new ProductImage
            {
                ProductId = ProductId,
                ImageURL = ImageURL,
                IsPrimary = IsPrimary,
                ImageText = ImageText
            };
        }
    }
    public class ProductImageUpdateDto : IUpdateDto<ProductImage>
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageURL { get; set; }
        public bool IsPrimary { get; set; }
        public string? ImageText { get; set; }
        public ProductImage UpdateEntity(ProductImage entity)
        {
            entity.ProductId = ProductId;
            entity.ImageURL = ImageURL;
            entity.IsPrimary = IsPrimary;
            entity.ImageText = ImageText;
            return entity;
        }
    }
}