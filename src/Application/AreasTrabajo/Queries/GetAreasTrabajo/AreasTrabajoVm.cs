using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaLimpia.Application.AreasTrabajo.Queries.GetAreasTrabajo;

public class AreasTrabajoVm
{
    public IReadOnlyCollection<AreaTrabajoDto> AreasTrabajo { get; init; } = Array.Empty<AreaTrabajoDto>();
}

public class AreaTrabajoDto
{
    public int IdAreaTrabajo { get; init; }
    public string AreaTrabajoNombre { get; init; } = string.Empty;
    public bool? EsAreaProduccion { get; init; }
    public string? Color { get; init; } = string.Empty;
}
