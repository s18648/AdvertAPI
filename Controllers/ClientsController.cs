using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdvertAPI.DTOs.Requests;
using AdvertAPI.DTOs.Responses;
using AdvertAPI.Handlers;
using AdvertAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdvertAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    [Authorize(Roles = "user")]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public ClientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RegisterUser(AddUserRequest iur)
        {
            var res = _dbService.AddUser(iur);

            if (iur == null)
            {
                return BadRequest("User-client was not created");
            }


            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, iur.Login),
                    new Claim(ClaimTypes.Name, iur.FirstName),
                    new Claim(ClaimTypes.Role, "user")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(res.Hash));  //TODO HERE WAS SECRET VALUE - SOME LONG ASS VALUE PUT IN APPSETTINGS JSON INCONF
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "AdvertCompany",
                audience: "user",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid();
            string refreshTokenString = refreshToken.ToString();

            RecordTokenRequest rtr = new RecordTokenRequest
            {
                IdClient = res.IdClient,
                refreshTokenValue = res.TokenString
            };


            _dbService.RecordToken(rtr);


            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });



        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequestDto request)

        {

            LoginAttemptResponse loginAttempt = _dbService.checkLogin(request.Login);

            if (loginAttempt == null)
            {
                return NotFound("That login does not exist in the database");
            }


            if (!AuthHandler.Validate(request.Password, loginAttempt.Salt, loginAttempt.Hash))
            {
                return BadRequest("Incorrect Password");
            }

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginAttempt.Login),
                    new Claim(ClaimTypes.Name, loginAttempt.FirstName),
                    new Claim(ClaimTypes.Role, "user")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FFFFFFFFFIIIIIIIIIXXXXXX THEEEEEE VAAAAAAAALLLLLLLLLUUUEEEEEEE!!!!!!!!!!!!!!!!!!!!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "AdvertCompany",
                audience: "user",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid();
            string refreshTokenString = refreshToken.ToString();

            RecordTokenRequest rtr = new RecordTokenRequest
            {
                IdClient = loginAttempt.IdClient,
                refreshTokenValue = loginAttempt.TokenString
            };

            _dbService.RecordToken(rtr);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });


        }


        [HttpPost("refresh-token/{token}")]
        [AllowAnonymous]
        public IActionResult RefreshToken(string requestToken)
        {
            var givenToken = _dbService.ValidateTheToken(requestToken);

            if (givenToken == null)
            {
                return BadRequest();
            }

            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, givenToken.Login), 
                    new Claim(ClaimTypes.Name, givenToken.FirstName),
                    new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FIX THE VAL IN HERE")); //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "AdvertCompany",
                audience: "user",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid();

            string refreshTokenString = refreshToken.ToString();

            var tokenCreated = new RecordTokenRequest
            {
                IdClient = givenToken.IdClient,
                refreshTokenValue = givenToken.TokenString

            };

            _dbService.RecordToken(tokenCreated);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });

        }




        [HttpGet]
        public IActionResult GetCampaigns()
        {
            var res = _dbService.GetListOfAllCampaigns();
            return Ok(res);

        }












    }
}