using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BlogApp.Models;

namespace BlogApp.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "asp-role-users")]
    public class RoleUsersTagHelper:TagHelper{

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleUsersTagHelper(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager){
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HtmlAttributeName("asp-role-users")]
        public String RoleId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userNames = new List<string>();
            var role = await _roleManager.FindByIdAsync(RoleId);

            if(role != null && role.Name != null){
                foreach(var user in _userManager.Users){
                if(await _userManager.IsInRoleAsync(user, role.Name)){
                    userNames.Add(user.UserName ?? "");
                }
                }
                output.Content.SetHtmlContent(userNames.Count == 0 ? "kullanıcı yok": setHtml(userNames));
            }
        }

        private string setHtml(List<string> userNames){
            var html = "<ul>";
            foreach(var item in userNames){
                html += "<li>" + item + "</li>";
            }
            html += "</>";
            return html;
        }
    }

   
}