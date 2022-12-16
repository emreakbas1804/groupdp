using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webUi.Extensions;
using webUi.Identity;
using webUi.Models;
using static webUi.Program;

namespace webUi.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private UserManager<User> UserManager;
        private SignInManager<User> SignInManager;
        private readonly GroupDbContext context;
        public PanelController(GroupDbContext Context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            context = Context;
        }

        public IActionResult Index()
        {
            var LoginuserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;            
            var point = context.Points.Where(i => i.User.Id == LoginuserId).FirstOrDefault();
            return View(point);
        }
       
    }
}