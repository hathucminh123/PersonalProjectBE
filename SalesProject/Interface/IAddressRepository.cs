using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IAddressRepository
    {


        Task<Address> CreateAddress (Address address);



       Task<Address> UpdateAddress (Address address);


        Task<Address> DeleteAddress (Address address);


        Task<Address?> DeleteByAdmin (Guid AddressId );
        Task<List<Address>?> GetAddressByUserId (Guid UserId);

    }
}
