using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class AddressService : IAddressRepository
    {
        private readonly SalesDbContext salesDbContext;

        public AddressService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }
        public async Task<Address> CreateAddress(Address address)
        {
            salesDbContext.Addresses.Add(address); // Không cần await ở đây
            await salesDbContext.SaveChangesAsync(); // Lưu vào DB
            return address; // Trả về đối tượng đã thêm

        }

        public async Task<Address> DeleteAddress(Address address)
        {
            var addressDomain = await salesDbContext.Addresses
                .FirstOrDefaultAsync(x => x.UserId == address.UserId && x.Id == address.Id);


            if (addressDomain == null)
            {
                return null;
            }

            salesDbContext.Addresses .Remove(addressDomain);
            await salesDbContext.SaveChangesAsync();

            return addressDomain;




        }

        public async Task<Address?> DeleteByAdmin(Guid AddressId)
        {
            var addressDomain = await salesDbContext.Addresses
                  .FirstOrDefaultAsync(x => x.Id == AddressId);


            if (addressDomain == null)
            {
                return null;
            }

            salesDbContext.Addresses.Remove(addressDomain);
            await salesDbContext.SaveChangesAsync();

            return addressDomain;
        }

        public async Task<List<Address>?> GetAddressByUserId(Guid UserId)
        {
             return await salesDbContext.Addresses.Where(o => o.UserId == UserId).ToListAsync();
        }

        public async Task<Address> UpdateAddress(Address address)
        {
            var addressDomain = await salesDbContext.Addresses
                .FirstOrDefaultAsync(x => x.UserId == address.UserId && x.Id == address.Id);

            if (addressDomain == null)
            {
                return null;
            }

            // Cập nhật thông tin
            addressDomain.FullName = address.FullName;
            addressDomain.Phone = address.Phone;
            addressDomain.Province = address.Province;
            addressDomain.District = address.District;
            addressDomain.Ward = address.Ward;
            addressDomain.StreetAddress = address.StreetAddress;
            addressDomain.Email = address.Email;
            addressDomain.IsDefault = address.IsDefault;

            await salesDbContext.SaveChangesAsync();

            return addressDomain;
        }

    }
}
