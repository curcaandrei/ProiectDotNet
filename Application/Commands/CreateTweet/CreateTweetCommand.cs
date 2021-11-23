using System.Collections.Generic;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.CreateTweet
{
    public class CreateTweetCommand : IRequest<ObjectId>
    {
        public string Text { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public Dictionary<string, float> feels { get; set; }
    }
}