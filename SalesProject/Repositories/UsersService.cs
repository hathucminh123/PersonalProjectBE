using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;

namespace SalesProject.Repositories
{
    public class UsersService : IUserRepository
    {
        private readonly SalesDbContext salesDbContext;

        public UsersService(SalesDbContext salesDbContext)
        {
            this.salesDbContext = salesDbContext;
        }
        public async Task<Users> GetUsersById(Guid id)
        {
            var user = await salesDbContext.Users
                .Include(x => x.Orders)
                .Include(x => x.CartItems)
                .Include(x => x.Reviews)
                .Include(x => x.Addresses)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<Users> UpdateUser(Guid id, Users users)
        {
            var user = await salesDbContext.Users
                .Include(x => x.Orders)
                .Include(x => x.CartItems)
                .Include(x => x.Reviews)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            // ✅ Cập nhật các thuộc tính cơ bản
            user.FullName = users.FullName;
            user.AvatarUrl = users.AvatarUrl;
            user.Phone = users.Phone;
            user.Address = users.Address;
            user.DateOfBirth = users.DateOfBirth;
            user.IsActive = users.IsActive;

            // ✅ Nếu cho phép đổi mật khẩu (mã hóa mật khẩu trước khi lưu)
            if (!string.IsNullOrEmpty(users.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(users.PasswordHash);
            }

            // ✅ Không cho phép thay đổi Email và Role (vì lý do bảo mật)
            // user.Email = users.Email; // 🚫 Không nên cho phép thay đổi trực tiếp
            // user.Role = users.Role;   // 🚫 Không nên cho phép người dùng thay đổi

            // ✅ Cập nhật ngày chỉnh sửa cuối cùng
            user.UpdatedAt = DateTime.UtcNow;

            // ✅ Lưu thay đổi vào cơ sở dữ liệu
            await salesDbContext.SaveChangesAsync();

            return user;
        }

    }
}
