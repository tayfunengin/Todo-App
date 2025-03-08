using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoLists.Queries.GetTodos;
public class GetTagsQuery : IRequest<List<TagDto>>
{
}

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<TagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tagDtoList = new List<TagDto>();
        foreach (var todoItem in _context.TodoLists.AsNoTracking().Include(t => t.Items).ThenInclude(i => i.Tags).SelectMany(x => x.Items))
        {
            var tags = todoItem.Tags;

            tagDtoList.AddRange(_mapper.Map<List<TagDto>>(tags));
        }

        return tagDtoList.DistinctBy(t => t.Name).ToList();
    }

}
