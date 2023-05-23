using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //ıdentity kütüphanesinden gelen her özelliği kullanmamak adına appuserregister kullanılıyor 
        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        {
            if (ModelState.IsValid)
            {
                //random olarak confirmcode üretiliyor
                Random random = new Random();
                int code;
                code = random.Next(100000, 1000000);
                AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.Username,
                    Name = appUserRegisterDto.Name,
                    Surname = appUserRegisterDto.Surname,
                    Email = appUserRegisterDto.Email,
                    City = "aaaa",
                    District = "bbbb",
                    ImageUrl = "cccc",
                    ConfirmCode = code
                };
                
                var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);
                if (result.Succeeded)
                {
                    //mail gönderme işlemi yapılıyor 
                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddressFrom = new MailboxAddress("Easy Cash Admin", "turkmenmelih01@gmail.com"); //maili kim gönderiyor
                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", appUser.Email); //appuserdan gelen mail adresine kod gönderiliyor

                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.Add(mailboxAddressTo);

                    var bodyBuilder = new BodyBuilder(); //mail içeriğini oluşturuyoruz 
                    bodyBuilder.TextBody = "Kayıt işlemini gerçekleştirmek için onay kodunuz:" + code;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    mimeMessage.Subject = "Easy Cash Onay Kodu";

                    SmtpClient client = new SmtpClient();
                    client.Connect("smtp.gmail.com", 587, false); //protocol sağlanıyor 
                    client.Authenticate("turkmenmelih01@gmail.com", "liaieujafylbhahb"); //uygulama şifresi 2 adımlı doğrulamanın aktif olması gereklidir
                    client.Send(mimeMessage);
                    client.Disconnect(true);

                    TempData["Mail"] = appUserRegisterDto.Email;

                    return RedirectToAction("Index", "ConfirmMail");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
        //şifre en az 6 karakter olmalıdır,özel karakter olmalı,büyük küçük harf olmalı
    }
}
