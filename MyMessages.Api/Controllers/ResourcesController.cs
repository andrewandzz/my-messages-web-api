using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyMessages.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        [HttpGet]
        [Route("pictures/{id:int}")]
        public async Task<ActionResult> GetPictureById([FromRoute] int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var pictureDto = await resourceService.GetPictureByIdAsync(id, userId);
                return File(pictureDto.Path, pictureDto.ContentType, pictureDto.Name);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("files/{id:int}")]
        public async Task<ActionResult> GetFileById([FromRoute] int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var fileDto = await resourceService.GetFileByIdAsync(id, userId);
                return File(fileDto.Path, fileDto.ContentType, fileDto.Name);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ForbiddenException)
            {
                return Forbid();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("stickers/{id:int}")]
        public async Task<ActionResult> GetStickerById([FromRoute] int id)
        {
            try
            {
                var stickerDto = await resourceService.GetStickerByIdAsync(id);
                return File(stickerDto.Path, stickerDto.ContentType, stickerDto.Name);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
