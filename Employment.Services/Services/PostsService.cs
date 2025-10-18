using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Core.Helper;
using Job.Services.JobServices.DTOs.PostsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Tls.Crypto.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class PostsService:IPostsService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userservice;
        private readonly IFileService _fileService;

        public PostsService(AppDBContext dbContext, IUserService userservice,IFileService fileService)
        {
            _dbContext = dbContext;
            _userservice = userservice;
            _fileService = fileService;
        }

        public async Task<Result> AddPost(AddPostDTO PostDTO)
        {
            if(!string.IsNullOrWhiteSpace(PostDTO.Content)||PostDTO.Image != null)
            {
                var UserID = _userservice.GetCurrentUserID();

                var User = await _dbContext.AppUser.Include(x=>x.Company).Include(x=>x.Employees).FirstOrDefaultAsync(x => x.Id == UserID);
                if (User == null)
                    return Result.Fail("Error Tokent");

                if (!NameHelper.IsNameExist(User))
                    return Result.Fail("You Have To Enter Your Name");

                var ImagePath = await _fileService.SaveFileAsync(null, PostDTO.Image);

                var Post = new Posts
                {
                    UserID = UserID,
                    Content = PostDTO.Content,
                    Image = ImagePath,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Posts.AddAsync(Post);
                await _dbContext.SaveChangesAsync();

                return Result.SuccessResult($"Added Success , id = {Post.Id}");

            }
            return Result.Fail("You Have To Enter Image Or Content");
        }

        public async Task<Result> UpdatePost (UpdatePostDTO PostDTO)
        {
            if (PostDTO.PostID < 0)
                return Result.Fail("Invaled Input");
            if (!string.IsNullOrWhiteSpace(PostDTO.Content) || PostDTO.Image != null)
            {

                    var UserID = _userservice.GetCurrentUserID();
                if (UserID == null)
                    return Result.Fail("Token Error");

                var Post = await _dbContext.Posts.FirstOrDefaultAsync(x=>x.Id == PostDTO.PostID);
                if (Post == null)
                    return Result.Fail("Not Found");

                if (Post.UserID != UserID)
                    return Result.Fail("You Have No Access");

                Post.Content = PostDTO.Content;
                Post.Image = await _fileService.SaveFileAsync(Post.Image,PostDTO.Image);

                await _dbContext.SaveChangesAsync();

                return Result.SuccessResult("Updated Success");
            }
            return Result.Fail("You Have To Enter Image Or Content");
        }

        public async Task<Result> DeletePost(int PostID)
        {
            if (PostID < 0)
                return Result.Fail("Invaled Input");

            var post = await _dbContext.Posts.FirstOrDefaultAsync(x=>x.Id==PostID);
            if (post == null)
                return Result.Fail("Not Found");

            var UserID = _userservice.GetCurrentUserID();
            if (post.UserID != UserID)
                return Result.Fail("You Have No Access");

            try
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();
                return Result.SuccessResult("Deleted Success");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
              
            }
        }

        public async Task<Result> AddComment(int PostID,string Comment)
        {
            if (PostID < 0 || string.IsNullOrWhiteSpace(Comment))
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");


            var Data = await _dbContext.Posts.Where(x=>x.Id == PostID).Select(d => new
            {
                post = d.Id == PostID,
                user = _dbContext.AppUser.Include(x=>x.Employees).Include(x=>x.Company)
                        .FirstOrDefault(x=>x.Id == UserID),
                Connect = _dbContext.Connections.FirstOrDefault(x=>(x.Sender==UserID||x.Reseiver==UserID)
                        &&(x.Sender == d.UserID||x.Reseiver == d.UserID)&&x.Status==EnumConnectStatus.accept)
            }).FirstOrDefaultAsync();

            if (Data == null || !Data.post)
                return Result.Fail("Not Found");

            if(Data.user.Employees.UserID !=UserID)
            {
                if (Data.Connect == null)
                return Result.Fail("You Have To be Connect");
            }

            if (!NameHelper.IsNameExist(Data.user))
                return Result.Fail("You Have To Enter Your Name");

            
         
            var comment = new Comments
            {
                UserID = UserID,
                Comment = Comment,
                PostID = PostID,
                CreatedAt = DateTime.UtcNow

            };

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult($"Comment Added Success , id = {comment.id}");

        }

        public async Task<Result> UpdateComment(int CommentID, string NewComment)
        {
            if (CommentID < 0 || string.IsNullOrWhiteSpace(NewComment))
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var currentcomment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.id == CommentID);
            if (currentcomment == null)
                return Result.Fail("Not Found");

            if (currentcomment.UserID != UserID)
                return Result.Fail("You Have No access");

            currentcomment.Comment = NewComment;

            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Comment Updated Success");

        }

        public async Task<Result> DeleteComment(int CommentID)
        {
            if (CommentID < 0)
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var Comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.id == CommentID);
            if (Comment == null)
                return Result.Fail("Not Found");

            if (Comment.UserID != UserID)
                return Result.Fail("You Have No access");

            try
            {
                _dbContext.Comments.Remove(Comment);
                await _dbContext.SaveChangesAsync();
                return Result.SuccessResult("Deleted Success");

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            
            }



        }

        public async Task<Result> AddLike(int PostID)
        {
            if (PostID < 0)
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Tokent Error");

            var Data = await _dbContext.Posts.Where(p => p.Id == PostID)
                                            .Select(p => new
                                            {
                                                Post = p,
                                                HasLike = p.Likes.Any(l => l.UserID == UserID),
                                                User = _dbContext.AppUser.Include(x => x.Employees)
                                                .Include(x => x.Company).FirstOrDefault(x => x.Id == UserID)
                                            })
                                            .FirstOrDefaultAsync();

            if (Data == null)
                return Result.Fail("Not Found");

            if (Data.HasLike)
                return Result.Fail("You Already Like On The Post");

            if (!NameHelper.IsNameExist(Data.User))
                return Result.Fail("You Have To Enter Your Name");


            var Like = new Likes
            {
                UserID = UserID,
                PostID = PostID,
            };

            await _dbContext.Likes.AddAsync(Like);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Liked");
        }

        public async Task<Result> UnLike(int PostID)
        {
            if (PostID < 0)
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Tokent Error");

            var Like = await _dbContext.Likes.FirstOrDefaultAsync(x => x.PostID == PostID);
            if (Like == null)
                return Result.Fail("Not Found");

            if (Like.UserID != UserID)
                return Result.Fail("You Have No access");

       
             _dbContext.Likes.Remove(Like);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("UnLiked");
        }

        public async Task<Result> AddLikeOnComment(int CommentID)
        {
            if (CommentID < 0)
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Tokent Error");

            var Data = await _dbContext.Comments.Where(x => x.id == CommentID)
                                            .Select(c => new
                                            {
                                                Commnet = c,
                                                HasLike = c.Likes.Any(l => l.UserID == UserID),
                                                User = _dbContext.AppUser.Include(x => x.Employees)
                                                .Include(x => x.Company).FirstOrDefault(x => x.Id == UserID)
                                            })
                                            .FirstOrDefaultAsync();

            if (Data == null)
                return Result.Fail("Not Found");

            if (Data.HasLike)
                return Result.Fail("You Already Like On The Post");

            if (!NameHelper.IsNameExist(Data.User))
                return Result.Fail("You Have To Enter Your Name");

            var Like = new LikesOnComments
            {
                UserID = UserID,
                CommentID = CommentID,
            };

            await _dbContext.LikesOnComments.AddAsync(Like);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Liked");
        }

        public async Task<Result> UnLikeOnComment(int CommentID)
        {
            if (CommentID < 0)
                return Result.Fail("Invale Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var Like = await _dbContext.LikesOnComments.FirstOrDefaultAsync(x => x.CommentID == CommentID);
            if (Like == null)
                return Result.Fail("Not Found");

            if (Like.UserID != UserID)
                return Result.Fail("You Have No Access");

            _dbContext.LikesOnComments.Remove(Like);
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Unlikeed");
        }

        public async Task<Result<PageResult<GetPostDTO>>> GetPosts(int PageNmber , int PageSize)
        {
            if (PageNmber <= 0 || PageSize <= 0)
                return Result<PageResult<GetPostDTO>>.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result<PageResult<GetPostDTO>>.Fail("Token Error");

            var Posts = new List<GetPostDTO>();

            if (_userservice.GetRole() == UserTypeEnum.Company.ToString())
            {
                Posts = await _dbContext.Posts.OrderByDescending(x => x.CreatedAt).AsNoTracking().Skip((PageNmber - 1) * PageSize)
                                              .Take(PageSize).Select(p => new GetPostDTO
                                              {
                                                  PostID = p.Id,
                                                  UserID = p.UserID,
                                                  FillName = p.User.UserType == UserTypeEnum.Company ? (p.User.Company.Name)
                                                  : ($"{p.User.Employees.FirstName} {p.User.Employees.secoundName} {p.User.Employees.LastName}"),
                                                  Content = p.Content,
                                                  Image = p.Image,
                                                  TotalLikes = p.Likes.Count(),
                                                  TotalComment = p.Comments.Count(),
                                                  DateCreated = p.CreatedAt

                                              }).ToListAsync();
            }
            else
            {

                var Connectionlist = await (from ps in _dbContext.Posts
                                            from emp in _dbContext.Employees
                                            where ps.UserID == emp.UserID
                                            && (
                                                ps.UserID == UserID||                     
                                                (
                                                    (from con in _dbContext.Connections
                                                     where (con.Sender == UserID && con.Reseiver == emp.UserID && con.Status == EnumConnectStatus.accept)
                                                     || (con.Reseiver == UserID && con.Sender == emp.UserID && con.Status == EnumConnectStatus.accept)
                                                     select con).Any()
                                                )
                                            )
                                            orderby ps.CreatedAt descending
                                            select new GetPostDTO
                                            {
                                                PostID = ps.Id,
                                                UserID = ps.UserID,
                                                FillName = $"{emp.FirstName} {emp.secoundName} {emp.LastName}",
                                                Content = ps.Content,
                                                Image = ps.Image,
                                                TotalComment = ps.Comments.Count(),
                                                TotalLikes = ps.Likes.Count(),
                                                DateCreated = ps.CreatedAt
                                            }).Distinct().Skip((PageNmber - 1) * PageSize)
                                              .Take(PageSize).ToListAsync();


                var Companylist = await (from ps in _dbContext.Posts
                                         from cm in _dbContext.Companies
                                         from fl in _dbContext.Followers
                                         where ps.UserID == cm.UserID && fl.FollowerID == UserID && cm.UserID == fl.CompanyID
                                         orderby ps.CreatedAt descending
                                         select new GetPostDTO
                                         {
                                             PostID = ps.Id,
                                             UserID = ps.UserID,
                                             FillName = $"{cm.Name}",
                                             Content = ps.Content,
                                             Image = ps.Image,
                                             TotalComment = ps.Comments.Count(),
                                             TotalLikes = ps.Likes.Count(),
                                             DateCreated = ps.CreatedAt
                                         }).Distinct().Skip((PageNmber - 1) * PageSize)
                                     .Take(PageSize).ToListAsync();

                Posts = Connectionlist.Union(Companylist).OrderByDescending(x => x.DateCreated).ToList();
               
            }

            if (!Posts.Any())
                return Result<PageResult<GetPostDTO>>.Fail("Not Found");



            return Result<PageResult<GetPostDTO>>.SuccessResult(new PageResult<GetPostDTO>
            {
                PageNumber = PageNmber,
                PageSize = PageSize,
                Items = Posts
            }, "Success");

            // NOTE: For demonstration purposes, we are calculating LikesCount and CommentsCount dynamically
            // via LINQ Count(). In a large-scale production system, we could store these as denormalized
            // columns in the Posts table (LikesCount, CommentsCount) and update them on insert/delete
            // to improve performance. For this project, since it's medium-sized and for showcase, 
            // dynamic counting is sufficient.
        }

        public async Task<Result<PageResult<GetCommentsDTO>>> GetComments(int PageNmber, int PageSize,int PostID)
        {
            if (PageNmber <= 0 || PageSize <= 0)
                return Result<PageResult<GetCommentsDTO>>.Fail("Invaled Input");

            var Comments = await _dbContext.Comments.Where(x=>x.PostID==PostID).AsNoTracking()
                                                        .Skip((PageNmber-1)*PageSize).Take(PageSize)
                                                        .OrderByDescending(x=>x.CreatedAt)
                                                        .Select(c=>new GetCommentsDTO
                                                        {
                                                            id = c.id,
                                                            UserID = c.UserID,
                                                            Comment = c.Comment,
                                                            FillName = c.User.UserType==UserTypeEnum.Company?c.User.Company.Name
                                                            :($"{c.User.Employees.FirstName} {c.User.Employees.secoundName} {c.User.Employees.LastName}"),
                                                            TotalLikes = c.Likes.Count(),
                                                            Created = c.CreatedAt
                                                        }).ToListAsync();

            if(!Comments.Any())
                return Result<PageResult<GetCommentsDTO>>.Fail("Not Found");

            return Result<PageResult<GetCommentsDTO>>.SuccessResult(new PageResult<GetCommentsDTO>
            {
                PageNumber = PageNmber,
                PageSize= PageSize,
                Items = Comments
            },"Success");

        }

        public async Task<Result<PageResult<GetLikesDTO>>> GetLikes(int PageNumber,int PageSize,int PostID)
        {
            if (PageNumber <= 0 || PageSize <= 0)
                return Result<PageResult<GetLikesDTO>>.Fail("Invaled Input");

            var Likes = await _dbContext.Likes.AsNoTracking().Where(x => x.PostID == PostID).Skip((PageNumber - 1) * PageSize)
                                            .Take(PageSize).Select(l => new GetLikesDTO
                                            {
                                                id = l.id,
                                                UserID = l.UserID,
                                                FullName = l.User.UserType == UserTypeEnum.Company ?
                                                l.User.Company.Name : ($"{l.User.Employees.FirstName} {l.User.Employees.secoundName} {l.User.Employees.LastName}")
                                            }).ToListAsync();

            if (!Likes.Any())
                return Result<PageResult<GetLikesDTO>>.Fail("Not Found");

            return Result<PageResult<GetLikesDTO>>.SuccessResult(new PageResult<GetLikesDTO>
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                Items = Likes
            }, "Success");


        }

        public async Task<Result<PageResult<GetLikesDTO>>> GetLikesOnComment(int PageNumber, int PageSize, int CommentID)
        {
            if (PageNumber <= 0 || PageSize <= 0|| CommentID<0)
                return Result<PageResult<GetLikesDTO>>.Fail("Invaled Input");

            var Likes = await _dbContext.LikesOnComments.AsNoTracking().Where(x => x.CommentID == CommentID).Skip((PageNumber - 1) * PageSize)
                                            .Take(PageSize).Select(l => new GetLikesDTO
                                            {
                                                id = l.id,
                                                UserID = l.UserID,
                                                FullName = l.User.UserType == UserTypeEnum.Company ?
                                                l.User.Company.Name : ($"{l.User.Employees.FirstName} {l.User.Employees.secoundName} {l.User.Employees.LastName}")
                                            }).ToListAsync();

            if (!Likes.Any())
                return Result<PageResult<GetLikesDTO>>.Fail("Not Found");

            return Result<PageResult<GetLikesDTO>>.SuccessResult(new PageResult<GetLikesDTO>
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                Items = Likes
            }, "Success");

        }
    }
}
