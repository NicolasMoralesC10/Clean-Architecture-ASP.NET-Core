using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquitecturaLimpia.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArquitecturaLimpia.Application.AreasTrabajo.Queries.GetAreasTrabajo;

public record GetAreasTrabajoQueryHandler : IRequestHandler<GetAreasTrabajoQuery, AreasTrabajoVm>
{
    private readonly IApplicationDbContext _context;

    public GetAreasTrabajoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AreasTrabajoVm> Handle(GetAreasTrabajoQuery request, CancellationToken cancellationToken)
    {
        var areasTrabajo = await _context.AreasTrabajo
            .AsNoTracking()
            .Select(x => new AreaTrabajoDto
            {
                IdAreaTrabajo = x.IdAreaTrabajo,
                AreaTrabajoNombre = x.AreaTrabajoNombre,
                EsAreaProduccion = x.EsAreaProduccion,
                Color = 
                
                x.Color
            })
            .ToListAsync(cancellationToken);

        return new AreasTrabajoVm
        {
            AreasTrabajo = areasTrabajo
        };
    }
}
