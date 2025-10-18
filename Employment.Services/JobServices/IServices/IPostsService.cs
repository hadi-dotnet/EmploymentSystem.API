using Job.Services.JobServices.DTOs.PostsDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.IServices
{
    public interface IPostsService
    {
        Task<Result> AddPost(AddPostDTO PostDTO);
        Task<Result> UpdatePost(UpdatePostDTO PostDTO);
        Task<Result> DeletePost(int PostID);
        Task<Result> AddComment(int PostID, string Comment);
        Task<Result> UpdateComment(int CommentID, string NewComment);
        Task<Result> DeleteComment(int CommentID);
        Task<Result> AddLike(int PostID);
        Task<Result> UnLike(int PostID);
        Task<Result> AddLikeOnComment(int CommentID);
        Task<Result> UnLikeOnComment(int CommentID);
        Task<Result<PageResult<GetPostDTO>>> GetPosts(int PageNmber, int PageSize);
        Task<Result<PageResult<GetCommentsDTO>>> GetComments(int PageNmber, int PageSize, int PostID);
        Task<Result<PageResult<GetLikesDTO>>> GetLikes(int PageNumber, int PageSize, int PostID);
        Task<Result<PageResult<GetLikesDTO>>> GetLikesOnComment(int PageNumber, int PageSize, int CommentID);
    }
}
