using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IUserService
    {
        LogInResponseDTO Authenticate(LogInDTO model);
        LogInResponseDTO Register(RegisterDTO model);
        User GetById(int id);
        bool VerifyEmail(string token);
        void SendVerificationEmail(User user);
    }

    public class UserService : IUserService
    {
        private MainDbContext context;
        private IJwtService jwtService;
        private IMailService mailService;
        private string appLink;

        public UserService(MainDbContext mainDbContext, IJwtService jwtService, IMailService mailService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.context = mainDbContext;
            this.jwtService = jwtService;
            this.mailService = mailService;

            var appLink = configuration.GetValue<string>("AppLink");

            if (webHostEnvironment.IsProduction() && Environment.GetEnvironmentVariable("APP_LINK") != null)
                appLink = Environment.GetEnvironmentVariable("APP_LINK");

            if (appLink == null)
                throw new ArgumentException("Missing configuration", "AppLink");

            this.appLink = appLink;
        }

        public LogInResponseDTO Authenticate(LogInDTO model)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (user == null)
                return null;

            var passwordHash = HashPassword(model.Password, user.Salt);

            if (passwordHash != user.PasswordHash)
                return null;

            // authentication successful so generate jwt token
            var token = GenerateLoginToken(user);
            return new LogInResponseDTO() { Token = token };
        }

        public LogInResponseDTO Register(RegisterDTO model)
        {
            var userWithSameEmail = context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (userWithSameEmail != null)
                return null;

            var salt = GenerateSalt();
            var passwordHash = HashPassword(model.Password, salt);

            var user = new User()
            {
                Email = model.Email,
                Username = model.Username,
                PasswordHash = passwordHash,
                Salt = salt
            };

            context.Users.Add(user);
            context.SaveChanges();

            SendVerificationEmail(user);

            var token = GenerateLoginToken(user);
            return new LogInResponseDTO() { Token = token };
        }

        public User GetById(int id)
        {
            return context.Users.Include(o => o.Workspaces).FirstOrDefault(x => x.Id == id);
        }

        public bool VerifyEmail(string token)
        {
            if (!jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
                return false;

            if (jwtToken.Claims.First(x => x.Type == "type")?.Value != "verify")
                return false;

            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return false;

            user.VerifiedEmail = true;
            context.SaveChanges();
            return true;
        }

        public void SendVerificationEmail(User user)
        {
            var verifyLink = $"{appLink}verify?token={GenerateVerifyToken(user)}";

            this.mailService.SendEmailAsync(new MailRequest()
            {
                Subject = "WappChatAnalyzer account verification",
                ToEmail = user.Email,
                Body =
                $"Hello {user.Username},<br>" +
                $"<br>" +
                $"Welcome to WappChatAnalyzer. Thank you for choosing us. Please verify your account to unlock all features.<br>" +
                $"To verify it, open this link:<br>" +
                $"{verifyLink}"
            });
        }

        private byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private string HashPassword(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private string GenerateLoginToken(User user)
        {
            return jwtService.GenerateToken(
                new Claim("type", "login"),
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim("roles", Newtonsoft.Json.JsonConvert.SerializeObject(user.GetRoles().Select(o => o.ToString())))
            );
        }

        private string GenerateVerifyToken(User user)
        {
            return jwtService.GenerateToken(
                new Claim("type", "verify"),
                new Claim("id", user.Id.ToString())
            );
        }
    }
}
