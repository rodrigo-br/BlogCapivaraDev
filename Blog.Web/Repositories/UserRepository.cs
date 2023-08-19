using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AuthDbContext _authDbContext;

		public UserRepository(AuthDbContext authDbContext)
        {
			this._authDbContext = authDbContext;
		}

        public async Task<IEnumerable<IdentityUser>> GetAll()
		{
			var users = await _authDbContext.Users.ToListAsync();

			var superAdmninUser = await _authDbContext.Users
				.FirstOrDefaultAsync(x => x.Email == "superadmin@blog.com");

			if (superAdmninUser != null)
			{
				users.Remove(superAdmninUser);
			}

			return users;
		}
	}
}
