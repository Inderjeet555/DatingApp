using AutoMapper;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IdatingRepository _repository;
        private readonly IMapper _mapper;
        public MessagesController(IdatingRepository repository, IMapper mapper)
        {
            this._mapper = mapper;
            this._repository = repository;

        }

        [HttpGet("{id}", Name= "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                var messageFromRepo = await _repository.GetMessage(id);

                if(messageFromRepo == null)
                    return NotFound();

                return Ok(messageFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId,  UserForMessageDto userForMessageDto)
        {
             if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                userForMessageDto.SenderId = userId;

                var recipient = await _repository.GetUser(userForMessageDto.RecipientId);

                if (recipient == null)
                    BadRequest("Could not find user!!");

                var message = _mapper.Map<Message>(userForMessageDto);

                _repository.Add(message);

               // var messageToReturn = _mapper.Map<UserForMessageDto>(message);

                if (await _repository.SaveAll()) 
                {
                    var messageToReturn = _mapper.Map<UserForMessageDto>(message);
                    return CreatedAtRoute("GetMessage", new {userId, id = message.Id}, messageToReturn);
                }
                    

                throw new System.Exception("Creating the message failed on save!!");
                
        }
    }
}