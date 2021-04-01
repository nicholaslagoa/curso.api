using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.Infrastructure.Data;
using curso.api.Infrastructure.Repositories;
using curso.api.Models;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;
        public UsuarioController(IUsuarioRepository usuarioRepository, 
            IAuthenticationService authenticationService)
        {
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
        }
        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo.
        /// </summary>
        /// <param name="loginViewModelInput">View model do login</param>
        /// <returns>Retorna status ok, dados do usuário e o token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]//aonde o codigo la no swagger UI for 200...
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost] //verbo http do tipo POST
        [Route("logar")]//.netcore p
        [ValidacaoModelStateCustomizado]//usa filters ouves das linhas cruas abaixo de Logar()
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
            Usuario usuario = _usuarioRepository.ObterUsuario(loginViewModelInput.Login);
            if(usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar.");
            }
            /* if(usuario.Senha != loginViewModel.Senha.GerarSenhaCriptografada())
            {
                return BadRequest("Houve um erro ao tentar acessar.");
            }*/
            var usuarioViewModelOutput = new UsuarioViewModelOutput()//cria um novo usuario local pra testar
            {
                Codigo = usuario.Codigo,
                Login = loginViewModelInput.Login,
                Email = usuario.Email
            };

            var token = _authenticationService.GerarToken(usuarioViewModelOutput);//devolve o token escrito de fato para essa var que vai ser chamada abaixo

            return Ok(new
            {
                Token = token,
                Usuario = usuarioViewModelOutput
            });//funfou, retorna o token e o usuarioviewoutput
        }

        /// <summary>
        /// Este serviço permite cadastrar um usuário cadastrado não existente
        /// </summary>
        /// <param name="registro">View model do registro de login</param>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]//aonde o codigo la no swagger UI for 200...
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]//mesma coisa aqui
        public IActionResult Registrar(RegistroViewModelInput loginViewModelInput)
        {
            //var migracoesPendentes = contexto.Database.GetPendingMigrations();
            //if(migracoesPendentes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}

            var usuario = new Usuario();
            usuario.Login = loginViewModelInput.Login;
            usuario.Senha = loginViewModelInput.Senha;
            usuario.Email = loginViewModelInput.Email;
            _usuarioRepository.Adicionar(usuario);
            _usuarioRepository.Commit();

            return Created("", loginViewModelInput);//funfou, retorna
        }
    }
}
