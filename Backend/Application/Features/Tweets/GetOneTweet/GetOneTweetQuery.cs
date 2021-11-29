﻿using Application.Persistence;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Features.Tweets.GetOneTweet
{
    public class GetOneTweetQuery : IRequest<Tweet>
    {
        public ObjectId Id { get; set; }
        
        public GetOneTweetQuery(string id)
        {
            this.Id = ObjectId.Parse(id);
        }
    }
}