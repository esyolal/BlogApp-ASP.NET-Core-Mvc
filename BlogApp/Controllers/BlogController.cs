using System.Data.Entity;
using System.Security.Claims;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BlogController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allPosts = _context.Posts.ToList();

            var model = new PostsUsersModel
            {
                User = currentUser,
                AllPosts = allPosts
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string postContent)
        {
            if (User.Identity!.IsAuthenticated)
            {

                var currentUser = await _userManager.GetUserAsync(User);

                var newPost = new Posts
                {
                    Content = postContent,
                    CreatedAt = DateTime.Now,
                    UserId = currentUser?.Id,
                    UserName = currentUser?.UserName,
                    PictureSource = currentUser?.PictureSource
                };


                _context.Posts.Add(newPost);
                _context.SaveChanges();


                return RedirectToAction("Index", "Blog");
            }


            return RedirectToAction("Index", "Blog");
        }
        public IActionResult Post(int postId)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);

            var comments = _context.Comments.Where(c => c.PostId == postId).ToList();

            if (post == null)
            {
                return RedirectToAction("Index", "Blog");
            }

            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string commentContent)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);
            var currentUser = await _userManager.GetUserAsync(User);

            if (post != null)
            {
                // Eğer post.Comments null ise, yeni bir List<Comments> örneği oluşturarak başlatın
                if (post.Comments == null)
                {
                    post.Comments = new List<Comments>();
                }

                var comment = new Comments
                {
                    Content = commentContent,
                    CreatedAt = DateTime.Now,
                    UserId = currentUser?.Id,
                    UserName = currentUser?.UserName,
                    UserPictureSource = currentUser?.PictureSource
                };

                post.Comments.Add(comment);
                _context.SaveChanges();

                return RedirectToAction("Post", new { postId = postId });
            }

            return RedirectToAction("Error");
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

            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            if (userRoles.Contains("Admin") || (post.UserId == userId))
            {

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LikePost(int postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (!post.LikedBy.Contains(userId))
            {
                // Kullanıcı daha önce beğenmediyse beğeni ekle
                post.Likes++;
                post.LikedBy.Add(userId);
            }
            else
            {
                // Kullanıcı beğeniyorsa beğeni çıkar
                post.Likes--;
                post.LikedBy.Remove(userId);
            }

            _context.SaveChanges();

            return RedirectToAction("Index"); // Veya başka bir sayfaya yönlendirme yapabilirsiniz
        }

    }


}