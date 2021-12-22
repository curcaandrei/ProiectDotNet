﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Features.Jwt;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IMediator _mediator;
        public JwtController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("/token", Name = "Jwt Generator")]
        [ProducesResponseType(StatusCodes.Status200OK)]  
        public Object GetToken(string tweets)
        {
            return _mediator.Send(new GetJwtQuery(tweets)).Result;
        }
    }
}