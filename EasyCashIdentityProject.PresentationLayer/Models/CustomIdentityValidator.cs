using Microsoft.AspNetCore.Identity;

namespace EasyCashIdentityProject.PresentationLayer.Models
{
    public class CustomIdentityValidator : IdentityErrorDescriber //hataları türkçe olmasını sağlar  register işlemleri
                                                                                                     
    {               
        //overrride methodun işleyişini bozmaz ama bizim istediğimiz formatta çalışır 
        //passwordtoshort şifrenin çok kısa olduğunu döner 
        public override IdentityError PasswordTooShort(int length)              
        {
            return new IdentityError()                                           
            {
                Code = "PasswordTooShort",
                Description = $"Parola en az {length} karakter olmalıdır" //$ işareti varsa mutlaka parametre geçilmelidir
            };
        }
        //passwordrequiresuppre şifrede en az 1 büyük harf ister 
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresUpper",
                Description = "Lütfen en az 1 büyük harf giriniz"
            };
        }
        //passwordrequireslower şifrede en az 1 küçük harf ister 
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = "Lütfen en az 1 küçük harf giriniz"
            };
        }
        //passwordrequiredigit en az 1 tane rakam ister 
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresDigit",
                Description = "Lütfen en az 1 tane rakam giriniz"
            };
        }
        //passwordrequiresnonalphanumeric en az 1 tane sembol ister 
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Lütfen en az 1 tane sembol giriniz"
            };
        }
    }
}
