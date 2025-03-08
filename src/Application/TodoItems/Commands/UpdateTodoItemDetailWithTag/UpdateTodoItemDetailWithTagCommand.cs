using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetailWithTag;
public class UpdateTodoItemDetailWithTagCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }

    public IList<TagDto>? Tags { get; init; }
}

public class UpdateTodoItemDetailWithTagCommandHandler : IRequestHandler<UpdateTodoItemDetailWithTagCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateTodoItemDetailWithTagCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateTodoItemDetailWithTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;
        var tags = _mapper.Map<List<Tag>>(request.Tags);
        entity.Tags = tags;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
