
using System.ComponentModel.DataAnnotations;
using Ecommerce.Domain.src.AddressAggregate;
using Ecommerce.Domain.src.Shared;
using Ecommerce.Service.src.Shared;
namespace Ecommerce.Service.src.AddressService;

public class AddressReadDto : BaseReadDto<Address>
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? City { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;

    public override void FromEntity(Address entity)
    {
        base.FromEntity(entity);
        AddressLine1 = entity.AddressLine1;
        AddressLine2 = entity.AddressLine2;
        City = entity.City;
        PostalCode = entity.PostalCode;
        Country = entity.Country;
    }
}
public class AddressCreateDto : ICreateDto<Address>
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? City { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;

    public Address CreateEntity()
    {
        return new Address
        {
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            City = City,
            PostalCode = PostalCode,
            Country = Country
        };
    }
}
public class AddressUpdateDto : IUpdateDto<Address>
{
    public Guid Id { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? City { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;

    public Address UpdateEntity(Address entity)
    {
        entity.AddressLine1 = AddressLine1;
        entity.AddressLine2 = AddressLine2;
        entity.City = City;
        entity.PostalCode = PostalCode;
        entity.Country = Country;
        return entity;
    }
}
