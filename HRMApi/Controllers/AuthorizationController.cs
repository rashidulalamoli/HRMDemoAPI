using Entity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility.StaticData;

namespace HRMApi.Controllers
{
    [ApiController]
    [Route(StaticData.API_CONTROLLER_ROUTE)]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authorizationService;
        public AuthorizationController(IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _configuration = configuration;
            _authorizationService = authorizationService;
        }

        //POST : /token
        [HttpPost(StaticData.TOKEN)]
        public async Task<IActionResult> Token(JwtTokenInfo request)
        {
            if (request.Grant_type == StaticData.GRANT_TYPE_PASSWORD)
            {
                var response = await BuildToken(request);
                return await Task.FromResult(Ok(response));
            }
            else if (request.Grant_type == StaticData.GRANT_TYPE_REFRESH_TOKEN)
            {
                var response = await BuildRefreshToken(request.Refreshtoken);
                return response;
            }
            throw new InvalidOperationException(StaticData.GRANT_TYPE_NOT_SUPPORTED);
        }

        private async Task<TokenResult> BuildToken(JwtTokenInfo request)
        {
            TokenResult tokenResult = new TokenResult();
            try
            {
                var info = await _authorizationService.GetTokenInfo(request.Password, request.Username);            
                if (info.StatusCode == 200)
                {                   
                    var token = await GenerateToken(request.Username, info.UserName);
                    tokenResult.Access_token = new JwtSecurityTokenHandler().WriteToken(token);
                    tokenResult.Expiration = token.ValidTo;
                    tokenResult.StatusCode = StatusCodes.Status200OK;
                    tokenResult.Message = StaticData.SUCCESS;
                    return await Task.FromResult(tokenResult);

                }
                tokenResult.StatusCode = info.StatusCode;
                tokenResult.Message = info.Message;
                return await Task.FromResult(tokenResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<ActionResult> BuildRefreshToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[StaticData.JWT_KEY]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var handler = new JwtSecurityTokenHandler();
                var refreshToken = handler.ReadToken(token) as JwtSecurityToken;
                var claims = refreshToken.Claims.ToList();
                var newToken = new JwtSecurityToken(_configuration[StaticData.SERVICE_BASE], _configuration[StaticData.ORIGINS], claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: credentials);
                return await Task.FromResult(Ok(new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(newToken),
                    expiration = newToken.ValidTo
                }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<JwtSecurityToken> GenerateToken(string email, string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[StaticData.JWT_KEY]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email, JwtSecurityTokenHandler.JsonClaimTypeProperty),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName, JwtSecurityTokenHandler.JsonClaimTypeProperty),
            };
            var token = new JwtSecurityToken(_configuration[StaticData.SERVICE_BASE], _configuration[StaticData.ORIGINS], claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: credentials);
            return await Task.FromResult(token);
        }
    }
}
