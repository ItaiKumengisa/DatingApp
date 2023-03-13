using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        #region
            //SymmetricSecurityKeys are used when encryption and decryption on handled all on the server side
        #endregion
        private readonly SymmetricSecurityKey _key;
        #region Notes 
        //We'll soon create a key that will be used to sign our JSON web tokens. it will be stored in configuration

        #endregion
        public TokenService(IConfiguration config)
        {
            //It takes a byte array so we convert our security key to a byte array. This is how we get the key to sign out jwt
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            //A jwt contains a list of "Claims"
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //Now we need some signing credentials. There is a signing credentials object, that we pass the key byte array and then the 
            //encryption algorithm. HMACSHA512 is the best apparently

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //This is where we describe the token we want to return

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
