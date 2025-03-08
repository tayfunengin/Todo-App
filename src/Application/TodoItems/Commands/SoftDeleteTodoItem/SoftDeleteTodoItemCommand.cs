using MediatR;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoItems.Commands.SoftDeleteTodoItem;
public class SoftDeleteTodoItemCommand : IRequest
{
    public int Id { get; init; }
}


public class SoftDeleteTodoItemCommandHandler : IRequestHandler<SoftDeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public SoftDeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SoftDeleteTodoItemCommand request, CancellationToken cancellationToken)
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
