using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webUi.Extensions;
using webUi.Identity;
using webUi.Models;
using static webUi.Program;

namespace webUi.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> UserManager;
        private SignInManager<User> SignInManager;
        private readonly GroupDbContext context;
        public HomeController(GroupDbContext Context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            context = Context;
        }

        public IActionResult Index()
        {            
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Giriş Başarısız",
                    Message = "Panele Giriş Yapabilmek İçin Mail Adresi ve Parola Girilmeli",
                    AlertType = "danger"
                });
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Giriş Başarısız",
                    Message = model.Email + " Kayıtlı Kullanıcı Bulunamadı",
                    AlertType = "danger"
                });
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {                
                return Redirect("~/panel");
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Giriş Başarısız",
                Message = "Kullanıcı Adı veya Parola Hatalı",
                AlertType = "danger"
            });
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            
            if (!ModelState.IsValid)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kayıt Başarısız",
                    Message = "Kayıt Formu Eksiksiz Bir Şekilde Doldurulmalı",
                    AlertType = "danger"
                });
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var RegisteredUser = await UserManager.FindByEmailAsync(model.Email);
                var Point = new Point()
                {
                    UserPoint = 100,
                    User = RegisteredUser
            };
                context.Points.Add(Point);
                context.SaveChanges();
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kayıt Başarılı",
                    Message = "Kayıt Oluşturulmuştur. Giriş Yapabilirsiniz. ",
                    AlertType = "success"
                });
                return RedirectToAction("login", "home");
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Kayıt Başarısız",
                Message = "Bu Email Adresi İle Daha Önce Kayıt Oluşturulmuştur",
                AlertType = "danger"
            });
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            var LoginuserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;            
            var point = context.Points.Where(i => i.User.Id == LoginuserId).FirstOrDefault();
            if(point.UserPoint > 0)
            {
                point.UserPoint = point.UserPoint - 20;
                context.Points.Update(point);
                context.SaveChanges();
            }            
            await SignInManager.SignOutAsync();
            return Redirect("~/");
        } 
    }
}