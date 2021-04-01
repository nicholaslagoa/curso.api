using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infrastructure.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Curso>//mesma coisa de usuario, ja que vai ser um banco de dados tem que
    {//implementar o entityframework, o builder, business entities e a interface
        public void Configure(EntityTypeBuilder<Curso> builder)
        {
            builder.ToTable("TB_CURSO");//define como vai ser chamada a tabela no código
            builder.HasKey(p => p.Codigo);//define que código vai ser a chave primária
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd();//define COMO AUTO_INCREMENT???? BRABO
            builder.Property(p => p.Nome);
            builder.Property(p => p.Descricao);
            builder.HasOne(p => p.Usuario).WithMany().HasForeignKey(fk => fk.CodigoUsuario);//aqui define que a FK é codigoUsuario
        }
    }
}
