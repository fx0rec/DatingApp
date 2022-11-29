
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        //A Symmetric key is used for both encrypting and decrypting the token key. 
        //An Asymmetric key is when a both a private & public key is used.
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            //  SymmetricSecurityKey() takes a byte[] so we need to use the encoding.
            //  Specified within the parameters of config is the Key to the KV pair. Value not added yet
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            //  Claim has a set number of standards that it uses 
            //  Expiration Time "exp", Not Before "nbf", Issued At "iat", etc.
            var claims = new List<Claim>
            {
                //  Adding the claim NameId, this is a claim about a user.
                //  They are all information that a user claims
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

                //Signing Credentials, _key encrypted with SHA512 
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            //  Describing the token we're gonna return. 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //  The subject is gonna include the claims we want to return.
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //   Self explanatory
                SigningCredentials = creds  //  Passing in signing credentials
            };
            //  After the Descriptor we need a tokenHandler.
            var tokenHandler = new JwtSecurityTokenHandler();
            //  Token is created, and the claims are initialized.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); //Token is written out then returned
        }
    }
}