using MediatR;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoLists.Commands.SoftDeleteTodoList;
public class SoftDeleteTodoListCommand : IRequest
{
    public int Id { get; init; }
}

public class SoftDeleteTodoListCommandHandler : IRequestHandler<SoftDeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public SoftDeleteTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SoftDeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        entity.Deleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}