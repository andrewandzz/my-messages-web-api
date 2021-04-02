using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMessages.Api.Models;
using MyMessages.Logics.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMessages.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StickersController : ControllerBase
    {
        private readonly IStickerService stickerService;
        private readonly IMapper mapper;

        public StickersController(IStickerService stickerService, IMapper mapper)
        {
            this.stickerService = stickerService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<StickerModel>>> GetAll()
        {
            try
            {
                var stickerDtos = await stickerService.GetAllAsync();
                var stickerModels = mapper.Map<List<StickerModel>>(stickerDtos);

                return stickerModels;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
