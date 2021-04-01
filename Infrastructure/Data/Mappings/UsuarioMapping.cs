using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infrastructure.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>//como é uma interface, tem que ter a implementação dela completa
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)//o usuario vai ser o "de, para" do bd
        {
            builder.ToTable("TB_USUARIO");//para chamar a tabela usuario no codigo
            builder.HasKey(p => p.Codigo);//define a chave primaria
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd();//vai ser uma identity? gere atraves do valuegeneratedonadd(ACHO QUE É AUTO_INCREMENT)
            builder.Property(p => p.Login);//resto dos campos da entity Usuario
            builder.Property(p => p.Senha);
            builder.Property(p => p.Email);

        }
    }
}
