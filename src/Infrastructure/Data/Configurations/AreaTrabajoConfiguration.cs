using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquitecturaLimpia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArquitecturaLimpia.Infrastructure.Data.Configurations;
public class AreaTrabajoConfiguration : IEntityTypeConfiguration<AreaTrabajo>
{
    public void Configure(EntityTypeBuilder<AreaTrabajo> builder)
    {
        builder.ToTable("area_trabajo");

        builder.HasKey(x => x.IdAreaTrabajo);

        builder.Property(x => x.IdAreaTrabajo)
            .HasColumnName("idArea_Trabajo");

        builder.Property(x => x.AreaTrabajoNombre)
            .HasColumnName("Area_Trabajo")
            .HasMaxLength(255)
            .IsRequired();


        builder.Property(x => x.EsAreaProduccion)
            .HasColumnName("esAreaProduccion")
            .IsRequired(false);

        builder.Property(x => x.Color)
            .HasColumnName("color")
            .HasMaxLength(50)
            .IsRequired(false);
    }
}
