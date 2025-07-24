using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaLimpia.Domain.Entities;
public class AreaTrabajo : BaseAuditableEntity
{
    public int IdAreaTrabajo { get; set; }
    public string AreaTrabajoNombre { get; set; } = string.Empty;
    public bool? EsAreaProduccion { get; set; }
    public string? Color { get; set; } = string.Empty;
}
