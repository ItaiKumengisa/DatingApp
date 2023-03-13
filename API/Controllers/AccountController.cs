using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dbContext;

        private readonly ITokenService _tokenService;
        public AccountController(DataContext dbContext, ITokenService tokenService) 
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))  return BadRequest("username is taken");
            
            //Use dotnet hashing 
            //This using keyword makes sure that we get rid of the allocated space for this object when we're out of scope
            //any class that implements IDisplosable, we can use the using keyword

            //this has a "ComputeHash" method that makes a hash of our password

            //HMAC is used to hash a string rather than creating our own hash algorithm. Also creates a key (salt)
            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDto.Username.ToLower(),
                //Before passing the password to the hash function, we transform it to a byte array
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            #region Comments
            //var user = await _dbContext.Users.SingleOrDefaultAsync(
            //    user => user.UserName == loginDto.Username);

            //if (user is null) return Unauthorized();

            //We have the user if we've gotten this far. Now we have to consider the Hashed 
            //password and the Hashkey we have stored for this user

            //So Itai, try to remember what we did to compute the hash in the first place:
            //We created a hmac object to use its hashing algorithm instead of trying to create our own
            //using var hmac = new HMACSHA512();

            //Then we used that to compute a hash code from our given password
            //var hashedPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //We then stored the Hashkey with the user that can be used to unscramble the hashcode.
            //We have the hashed password saved with the user and we have the one passed in by the current user
            //do we somehow decode both hashes to compare them?

            //Kinda
            //So apparently if you use the "new HMACSHA512()" hashing algorithm without passing it a parameter,
            //it will create it's own hashkey. But you also have the option to pass it a hashkey. 
            //If we pass it the hashkey from the user we got back, it will have the hash key needed to unscramble the hashcode

            #endregion

            var user = await _dbContext.Users.SingleOrDefaultAsync(user => user.UserName == loginDto.Username);

            if (user is null) return BadRequest("There is no user with this username");


            //If I get a user back, take its salt key to use it to recreate our hashing algorithm
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (user.PasswordHash[i] != computedHash[i]) return Unauthorized("Invalid password");
            }

            return Ok(new UserDto { Username = user.UserName, Token = _tokenService.CreateToken(user)});

        }

        //we use this to check if the user exists
        private async Task<bool> UserExists(string username)
        {
            return await _dbContext.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}
