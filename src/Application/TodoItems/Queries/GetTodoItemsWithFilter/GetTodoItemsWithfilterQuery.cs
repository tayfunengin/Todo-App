using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoItems.Queries.GetTodoItemsWithFilter;
public class GetTodoItemsWithFilterQuery : IRequest<List<TodoItemDto>>
{
    public int ListId { get; init; }
    public string? SearcText { get; init; } = string.Empty;
    public string? Tag { get; init; } = string.Empty;
}
public class GetTodoItemsWithfilterQueryHandler : IRequestHandler<GetTodoItemsWithFilterQuery, List<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsWithfilterQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<List<TodoItemDto>> Handle(GetTodoItemsWithFilterQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems.AsNoTracking()
           .ApplyFilter(request.ListId, request.SearcText, request.Tag)
           .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider).ToListAsync();

    }
}

public static class IQueryableExtensions
{
    public static IQueryable<TodoItem> ApplyFilter(this IQueryable<TodoItem> query, int listId, string searchText, string tag)
    {
        query = query.Where(x => x.ListId == listId);

        if (!string.IsNullOrEmpty(searchText))
            query = query.Where(x => x.Title.ToLower().Contains(searchText.ToLower()));

        if (!string.IsNullOrEmpty(tag))
            query = query.Where(x => x.Tags.Any(x => x.Name.Equals(tag)));

        return query;
    }
}