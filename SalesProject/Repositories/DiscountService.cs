using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.DTOs;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class DiscountService : IDiscountRepository


    {
        private readonly SalesDbContext salesDbContext;

        public DiscountService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }

        public async Task<Discounts> CreateDiscount(Discounts discounts)
        {
            


            await salesDbContext.Discounts.AddAsync(discounts);
            await salesDbContext.SaveChangesAsync();
            return discounts;

        }

        public async Task<List<Discounts>> GetDiscounts()
        {
            var discounts = await salesDbContext.Discounts.ToListAsync();
            return discounts;
        }

        public async Task<Discounts> DeleteDiscount(Guid id)
        {
            var discount = await salesDbContext.Discounts.FirstOrDefaultAsync(x => x.Id == id);

            if (discount == null)
            {
                return null;
            }

            salesDbContext.Discounts.Remove(discount);
            await salesDbContext.SaveChangesAsync();

            return discount;


        }

        


        

    }
}
