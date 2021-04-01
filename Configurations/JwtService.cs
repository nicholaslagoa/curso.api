using curso.api.Models.Usuarios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Configurations
{
    public class JwtService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput)
        {
            var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);//configura chave simetrica de segurança
            var securityTokenDescriptor = new SecurityTokenDescriptor//configura descriçao do token, o que vai vir de dados do usuario e quanto tempo vai expirar
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
                    new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
                    new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),//tempo que vai expirar
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)//qual algoritmo de encriptação será usado pra proteção
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();// atribui uma var com a classe de tokenHandler
            var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);//passa a descrição ali encima pra criar token
            var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

            return token;
        }
    }
}
