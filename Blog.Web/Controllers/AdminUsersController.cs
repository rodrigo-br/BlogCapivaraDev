using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminUsersController : Controller
	{
		private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUsersController(IUserRepository userRepository,
			UserManager<IdentityUser> userManager)
        {
			this._userRepository = userRepository;
            this._userManager = userManager;
        }

		[HttpGet]
        public async Task<IActionResult> List()
		{
			var users = await _userRepository.GetAll();

			var usersViewModel = new UserViewModel();
			usersViewModel.Users = new List<User>();

			foreach(var user in users)
			{
				usersViewModel.Users.Add(new Models.ViewModels.User
				{
					Id = Guid.Parse(user.Id),
					Username = user.UserName,
					EmailAddress = user.Email
				});
			}

			return View(usersViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> List(UserViewModel userViewModel)
		{
			var identityUser = new IdentityUser
			{
				UserName = userViewModel.Username,
				Email = userViewModel.Email,
			};

			var identityResult = await _userManager.CreateAsync(identityUser, userViewModel.Password);
			if (identityResult != null && identityResult.Succeeded)
			{
				var roles = new List<string> { "User" };
				if (userViewModel.AdminRoleCheckbox)
				{
					roles.Add("Admin");
				}

				identityResult = await _userManager.AddToRolesAsync(identityUser, roles);
				if (identityResult != null && identityResult.Succeeded) 
				{
					return RedirectToAction("List", "AdminUsers");
				}
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());

			if (user != null)
			{
				var identityResult = await _userManager.DeleteAsync(user);

				if (identityResult != null && identityResult.Succeeded)
				{
					return RedirectToAction("List", "AdminUsers");
				}
			}

			return View();
		}
	}
}
