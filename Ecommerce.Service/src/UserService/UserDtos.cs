using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.UserAggregate;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.UserService
{
    public class UserReadDto : BaseReadDto<User>
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }

        public override void FromEntity(User entity)
        {
            base.FromEntity(entity);
            UserName = entity.UserName;
            Email = entity.Email;
            PhoneNumber = entity.PhoneNumber;
            Role = entity.Role;
        }
    }
    public class UserCreateDto : ICreateDto<User>
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }

        public User CreateEntity()
        {
            return new User
            {
                UserName = UserName,
                Email = Email,
                Password = Password,
                PhoneNumber = PhoneNumber,
                Role = Role
            };
        }
    }

    public class UserUpdateDto : IUpdateDto<User>
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }

        public User UpdateEntity(User? entity)
        {
            entity.UserName = UserName;
            entity.Email = Email;
            entity.PhoneNumber = PhoneNumber;
            entity.Role = Role;
            return entity;
        }
    }
}