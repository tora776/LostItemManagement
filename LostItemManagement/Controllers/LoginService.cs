using LostItemManagement.Models;
using System;

namespace LostItemManagement.Services
{
    public class LoginService
    {
        private readonly LoginRepository _repository;

        public LoginService(LoginRepository repository)
        {
            _repository = repository;
        }

        public string? Authenticate(string userId, string password)
        {
            var user = _repository.GetUser(userId, password);
            if (user == null) return null;

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var now = DateTime.UtcNow;
            var expire = now.AddMinutes(30);

            _repository.SaveLoginToken(user.userId, token, now, expire);
            return token;
        }

        public bool IsTokenValid(string token)
        {
            var login = _repository.GetLoginByToken(token);
            return login != null && login.expireDate > DateTime.UtcNow;
        }
    }
}