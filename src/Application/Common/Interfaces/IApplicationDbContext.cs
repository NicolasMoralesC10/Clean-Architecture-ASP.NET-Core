using ArquitecturaLimpia.Domain.Entities;

namespace ArquitecturaLimpia.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<AreaTrabajo> AreasTrabajo { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
