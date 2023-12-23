using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Data;
namespace BlogApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            var userPosts = _context.Posts.Where(p => p.UserId == user.Id).ToList();

            return View(userPosts);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Blog");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                string? profilePictureBase64 = user.PictureSource != null ? Convert.ToBase64String(user.PictureSource) : null;
                return View(new ProfileViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    ProfilePictureBase64 = profilePictureBase64

                });
            }

            return RedirectToAction("Edit", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProfileViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index", "Blog");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.FullName = model.FullName;

                    if (model.ProfilePicture?.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await model.ProfilePicture.CopyToAsync(memoryStream);
                            if (memoryStream.Length < 2097152)
                            {
                                user.PictureSource = memoryStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("File", "The file is too large.");
                            }
                        }
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                    if (result.Succeeded)
                    {
                        TempData["successMessage"] = "Profil güncellendi.";
                        return RedirectToAction("Edit", "Profile");
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (post.UserId == userId)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return RedirectToAction(nameof(Index));
        }


    }
}