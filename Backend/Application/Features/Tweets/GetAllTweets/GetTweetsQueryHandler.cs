using System.Threading;
using System.Threading.Tasks;
using Application.Persistence;
using Domain.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tweets.GetAllTweets
{
    public class GetTweetsQueryHandler : IRequestHandler<GetTweetsQuery, System.Collections.Generic.List<TweetDto>>
    {
        private readonly IAsyncRepository<Tweet> _repository;

        public GetTweetsQueryHandler(IAsyncRepository<Tweet> repository)
        {
            _repository = repository;
        }

        public async Task<System.Collections.Generic.List<TweetDto>> Handle(GetTweetsQuery request, CancellationToken cancellationToken)
        {
            var allTweets = await _repository.ListAllAsync(request.PageNr);
            return System.Linq.Enumerable.ToList(allTweets);
        }
    }
}