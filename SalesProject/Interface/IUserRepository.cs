using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IUserRepository
    {
        Task<Users> GetUsersById(Guid id);

        Task<Users> UpdateUser(Guid id, Users user);
    }
}
