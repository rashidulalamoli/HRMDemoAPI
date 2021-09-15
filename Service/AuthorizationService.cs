using DataAccess.GenericRepositoryAndUnitOfWork;
using DataAccess.Models;
using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Utility.PasswordHelper;
using Utility.StaticData;

namespace Service
{
    public interface IAuthorizationService
    {
        Task<TokenInfo> GetTokenInfo(string password, string userName);
    }
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher _hasher;
        public AuthorizationService(IRepository<User> repository, IPasswordHasher hasher)
        {
            _repository = repository;
            _hasher = hasher;
        }
        public async Task<TokenInfo> GetTokenInfo(string password, string userName)
        {
            try
            {
                TokenInfo info = new TokenInfo();
                var user = await _repository.FindAsync(x => x.Email == userName);
                if (user != null)
                {
                    bool isPasswordMatched = MatchPassword(password,user.PasswordHash);
                    if (isPasswordMatched)
                    {
                        info.Message = StaticData.PASSWORD_MATCHED;
                        info.StatusCode = StatusCodes.Status200OK;
                        info.UserGuid = user.UserGuid;
                        info.UserName = user.UserName;
                    }
                    else
                    {
                        info.Message = StaticData.PASSWORD_MISMATCHED;
                        info.StatusCode = StatusCodes.Status401Unauthorized;
                       
                    }
                }
                else
                {
                    info.Message = StaticData.USER_NOT_FOUND;
                    info.StatusCode = StatusCodes.Status404NotFound;
                }
                return info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool MatchPassword(string password, string hash)
        {
            var checkPassword = _hasher.Check(hash, password);
            return checkPassword.Verified;
        }
    }
}
