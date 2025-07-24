using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquitecturaLimpia.Application.Common.Interfaces;
using MediatR;

namespace ArquitecturaLimpia.Application.AreasTrabajo.Queries.GetAreasTrabajo;

public record GetAreasTrabajoQuery : IRequest<AreasTrabajoVm>;
