using Demo_ASP_MVC_Modele.BLL.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Game.Infrastructure
{
    public class TokenManager
    {
        
        private readonly string _issuer, _audience, _secret; 

        // On rappatrie les infos de AppSettings
        public TokenManager(IConfiguration config)
        {
            _issuer = config.GetSection("TokenInfo").GetSection("issuer").Value;
            _audience = config.GetSection("TokenInfo").GetSection("audience").Value;
            _secret = config.GetSection("TokenInfo").GetSection("secret").Value;
        }

        public string GenerateToken(MemberModel m)
        {
            if (m is null) throw new ArgumentNullException();

            // Generation du token

            //  1 - Créer la signin key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
                //HMAC hash la clé de sécurité
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);


            // 2 - Creation du payload / info utilisateur / on met le nombres d'infos qu'on veut
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Surname, m.Pseudo),
                new Claim(ClaimTypes.Sid, m.Id.ToString()),
                new Claim(ClaimTypes.Role, m.IsAdmin ? "Admin" : "User")
            };

            // 3 - Configuration du jwtToken

            JwtSecurityToken token = new JwtSecurityToken(
                claims : claims,
                signingCredentials : credentials,
                issuer : _issuer,
                audience : _audience,
                expires: DateTime.Now.AddDays(1)
                
                );

            // 4 - Génération du token

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);

        }
    }

}
