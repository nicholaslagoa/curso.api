using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Infrastructure.Data;
using curso.api.Infrastructure.Data.Repositories;
using curso.api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace curso.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;//desabilita o padrão da microsf
                });
            services.AddSwaggerGen(c =>//leia
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme//conf de segurança do próprio swagger
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";//de um arquivo refraction ali do curso.api.xml
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);//de onde? do diretorio base da aplicaçao
                c.IncludeXmlComments(xmlPath);
            });

            var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);//pega a chave e converte em bytes via Encoding
            services.AddAuthentication(x =>//configurações do Jwt de PROTEÇÃO
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;//não vai usar https
                x.SaveToken = true;//salva token pra ficar cached
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//passará uma chave
                    IssuerSigningKey = new SymmetricSecurityKey(secret),//a chave será simétrica trazendo os bytes da chave no appsettings.json
                    ValidateIssuer = false,//não terá outras APIs usando esse Jwt
                    ValidateAudience = false//não terá outras APIs usando esse Jwt
                };
            });

            services.AddDbContext<CursoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));//ouvés de passar no hardcode que nem ali encima, foi passado pro appsettings e simplesmente chamado aqui
            });
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IAuthenticationService, JwtService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();//para poder protegar e mapear a API TEM QUE COLOCAR ESSA LINHA
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api curso");
                c.RoutePrefix = string.Empty;//swagger
            });
        }
    }
}
