using ArquitecturaLimpia.Application.AreasTrabajo.Queries.GetAreasTrabajo;

namespace ArquitecturaLimpia.Web.Endpoints;

public class AreasTrabajo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAreasTrabajo);
    }

    public Task<AreasTrabajoVm> GetAreasTrabajo(ISender sender)
    {
        return sender.Send(new GetAreasTrabajoQuery());
    }
}
