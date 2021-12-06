using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Commands.CreateTweet;
using Application.Commands.DeleteTweet;
using Application.Commands.UpdateTweet;
using Application.Features.Tweets.GetAllTweets;
using Application.Features.Tweets.GetOneTweet;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApi.Controllers
{ 
    
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TweetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all/{pageNr}", Name = "AllTweets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Tweet>>> GetAll(int pageNr)
        {
            var list = await _mediator.Send(new GetTweetsQuery(pageNr));
            
            return Ok(list);
        }
        
        [HttpPost("Create", Name = "CreateTweet")]
        public async Task<ActionResult<Tweet>> Create([FromBody] CreateTweetCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response.ToJson());
        }

        [HttpGet("one/{id}", Name = "GetOne")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<Tweet> GetOne([FromRoute]string id)
        {
            var res = _mediator.Send(new GetOneTweetQuery(id));
            return res;
        }

        [HttpDelete("delete/{id}", Name = "DeleteOne")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public DeleteResult DeleteOne([FromRoute] string id)
        {
            var res = _mediator.Send(new DeleteTweetCommand(id));
            return res.Result;
        }

        [HttpPut("update/{id}", Name = "UpdateOne")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public UpdateResult UpdateOne([FromRoute] string id, [FromBody]Dictionary<string, float> feels)
        {
            var res = _mediator.Send(new UpdateTweetCommand(id, feels));
            return res.Result;
        }
    }
}