using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using dotnetcore_jwt_auth.Models;
using dotnetcore_jwt_auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dotnetcore_jwt_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly UserContext userContext;
        readonly ITokenService tokenService;

        public AuthController(UserContext userContext, ITokenService tokenService)
        {
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            // if (user == null)
            // {
            //     return BadRequest("Invalid client request");
            // }

            // This example would be if we were issuing our own token. Typically, companies use a 3rd party token provider such as OTKA that would return a token
            // that can be used to create this JWT.
            //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Hard-coding the check for a user
            // if (user.UserName == "johndoe" && user.Password == "def@123")
            // {
            //     var token = CreateToken(signinCredentials, "Manager", user);
            //     return Ok(token);
            // }
            // else if(user.UserName == "janedoe" && user.Password == "def@123")
            // {
            //     var token = CreateToken(signinCredentials, "Operator", user);
            //     return Ok(token);
            // }
            // else
            // {
            //     return Unauthorized();
            // }

            // Reading from the db
            var userFromDb = userContext.LoginModels
                .FirstOrDefault(u => (u.UserName == user.UserName) &&
                                        (u.Password == user.Password));
            if (userFromDb == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Manager")
            };

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            userFromDb.RefreshToken = refreshToken;
            userFromDb.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            userContext.SaveChanges();

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });

        }

        private JwtAuthToken CreateToken(SigningCredentials signinCredentials, string role, LoginModel user)
        {
            // Add claims. (Roles are just claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims, //Add in the claims to the token
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new JwtAuthToken { Token = tokenString }; //"Token" property there will be read by the front-end client when it parses the json.
        }
    }
}