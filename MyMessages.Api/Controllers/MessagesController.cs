using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMessages.Api.Models;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyMessages.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IMapper mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            this.messageService = messageService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<MessagesDataModel>> Get(
            [FromQuery(Name = "from-id")] int? fromId,
            [FromQuery] int? count
        )
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var messagesDataDto = await messageService.GetAllAsync(userId, fromId, count);
                var messagesDataModel = mapper.Map<MessagesDataModel>(messagesDataDto);

                return Ok(messagesDataModel);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<MessageModel>> Add([FromForm] NewMessageModel model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var newMessageDto = mapper.Map<NewMessageDto>(model);
                var createdMessageDto = await messageService.AddAsync(newMessageDto, userId);
                var createdMessageModel = mapper.Map<MessageModel>(createdMessageDto);

                return Ok(createdMessageModel);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<MessageModel>> Edit([FromRoute] int id, [FromForm] EditMessageModel model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var editMessageDto = mapper.Map<EditMessageDto>(model);
                var editedMessageDto = await messageService.EditAsync(id, editMessageDto, userId);
                var messageModel = mapper.Map<MessageModel>(editedMessageDto);

                return Ok(messageModel);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Remove([FromRoute] int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                await messageService.RemoveAsync(id, userId);

                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
