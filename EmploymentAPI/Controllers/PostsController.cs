using Job.Services.JobServices.DTOs.PostsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postservice;
        public PostsController(IPostsService postsService)
        {
            _postservice = postsService;
        }

        [HttpPost("Add-Post")]
        public async Task<IActionResult> AddPost([FromForm] AddPostDTO Post)
        {
            var res = await _postservice.AddPost(Post);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPut("Update-Post")]
        public async Task<IActionResult> UpdatePost ([FromForm] UpdatePostDTO Post)
        {
            var res = await _postservice.UpdatePost(Post);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("Delete-Post")]
        public async Task<IActionResult> DeletePost([FromQuery] int PostID)
        {
            var res = await _postservice.DeletePost(PostID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Add-Comment")]
        public async Task<IActionResult> AddComment([FromQuery] int PostID, [FromQuery]string Comment)
        {
            var res = await _postservice.AddComment(PostID, Comment);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPut("Update-Comment")]
        public async Task<IActionResult> UpdateComment([FromQuery] int CommentID, [FromQuery] string Comment)
        {
            var res = await _postservice.UpdateComment(CommentID, Comment);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("Delete-Comment")]
        public async Task<IActionResult> DeleteComment([FromQuery] int CommentID)
        {
            var res = await _postservice.DeleteComment(CommentID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Add-Like")]
        public async Task<IActionResult> AddLike([FromQuery] int PostID)
        {
            var res = await _postservice.AddLike(PostID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("Un-Like")]
        public async Task<IActionResult> UnLike([FromQuery] int PostID)
        {
            var res = await _postservice.UnLike(PostID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Add-Like-On-Comment")]
        public async Task<IActionResult> AddLikeOnComment([FromQuery] int CommentID)
        {
            var res = await _postservice.AddLikeOnComment(CommentID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("Un-Like-On-Comment")]
        public async Task<IActionResult> UnLikeOnComment([FromQuery] int CommentID)
        {
            var res = await _postservice.UnLikeOnComment(CommentID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpGet("Get-Posts")]
        public async Task<IActionResult> GetPosts(int PageNmber, int PageSize)
        {
            var res = await _postservice.GetPosts(PageNmber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetPostDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetPostDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("Get-Comments")]
        public async Task<IActionResult> GetComments(int PageNmber, int PageSize,int PostID)
        {
            var res = await _postservice.GetComments(PageNmber, PageSize,PostID);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetCommentsDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetCommentsDTO>>.SuccessResponse(res.Data, res.Message));
        }

        [HttpGet("Get-Likes")]
        public async Task<IActionResult> GetLikes(int PageNmber, int PageSize, int PostID)
        {
            var res = await _postservice.GetLikes(PageNmber, PageSize, PostID);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetLikesDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetLikesDTO>>.SuccessResponse(res.Data, res.Message));
        }

        [HttpGet("Get-Likes-On-Comment")]
        public async Task<IActionResult> GetLikesOnComment(int PageNmber, int PageSize, int CommentID)
        {
            var res = await _postservice.GetLikesOnComment(PageNmber, PageSize, CommentID);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetLikesDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetLikesDTO>>.SuccessResponse(res.Data, res.Message));
        }
    }
}
