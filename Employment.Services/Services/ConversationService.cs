using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Infrastructure.Context.Configuration;
using Job.Services.JobServices.DTOs.ConversationsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class ConversationService : IConversationService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;
        public ConversationService(AppDBContext context, IUserService userService)
        {
            _dbContext = context;
            _userService = userService;
        }

        public async Task<Result> CreateConversation(string UserID2)
        {
            var UserID1 = _userService.GetCuurentUserID();

            var IsConvrsationExest = await _dbContext.Conversations.SingleOrDefaultAsync(x => (x.UserID1 == UserID1 && x.UserID2 == UserID2) || (x.UserID1 == UserID2 && x.UserID2 == UserID1));
            if (IsConvrsationExest != null)
                return Result.Fail("The Convrsation Is Already Exest");

            var Convrsation = new Conversations { UserID1 = UserID1, UserID2 = UserID2, CreatedAT = DateTime.Now };
            await _dbContext.Conversations.AddAsync(Convrsation);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Convrsation Created Complete");
        }

        public async Task<Result> SendMessage(MessageDTO message)
        {
            var CurrentUserID = _userService.GetCuurentUserID();
            var Conver = await _dbContext.Conversations.FirstOrDefaultAsync(x => x.ID == message.ConversationId);
            if (Conver == null)
                return Result.Fail("The Conversation Is Not Exest");

            if (Conver.UserID1 == CurrentUserID || Conver.UserID2 == CurrentUserID)
            {
                var Message = new Messages
                {
                    ConversationId = Conver.ID,
                    MassageText = message.MassageText,
                    SenderBy = CurrentUserID,
                    SendAT = DateTime.Now,

                };

                await _dbContext.Messages.AddAsync(Message);
                await _dbContext.SaveChangesAsync();

                return Result.SuccessResult("The message has been sent");
            }

            return Result.Fail("You Have No Access IN This Conversation");
        }

        public async Task<Result< List< GetConversationDTO>>> GetConversations()
        {
            var userID = _userService.GetCuurentUserID();

            var conversations = await _dbContext.Conversations
                .Where(x => x.UserID1 == userID || x.UserID2 == userID)
                .Select(con => new GetConversationDTO
                {
                    ConversationID = con.ID,
                    LastMessage = con.Message
                        .OrderByDescending(m => m.SendAT)
                        .Select(m => m.MassageText)
                        .FirstOrDefault(),
                    LastMessageTime = con.Message
                        .OrderByDescending(m => m.SendAT)
                        .Select(m => m.SendAT)
                        .FirstOrDefault(),
                    ReceiverName =
                        (con.UserID1 == userID && con.AppUser2.UserType == UserTypeEnum.Employee) ?
                            $"{con.AppUser2.Employees.FirstName} {con.AppUser2.Employees.secoundName} {con.AppUser2.Employees.LastName}" :
                        (con.UserID1 == userID && con.AppUser2.UserType == UserTypeEnum.Company) ?
                            con.AppUser2.Company.Name :
                        (con.UserID2 == userID && con.AppUser1.UserType == UserTypeEnum.Employee) ?
                            $"{con.AppUser1.Employees.FirstName} {con.AppUser1.Employees.secoundName} {con.AppUser1.Employees.LastName}" :
                        (con.UserID2 == userID && con.AppUser1.UserType == UserTypeEnum.Company) ?
                            con.AppUser1.Company.Name :
                        "Not Found"
                })
                .ToListAsync();
            return Result<List<GetConversationDTO>>.SuccessResult(conversations,"Success");        
        }



        public async Task<Result<List<GetMessageDTO>?>> GetMessages (int ConversationID)
        {
            var UserID = _userService.GetCuurentUserID();       

            var Message = await _dbContext.Messages.Where(x => x.ConversationId == ConversationID 
                        && (x.Conversation.UserID1 == UserID || x.Conversation.UserID2 == UserID))
                       .Select(x => new GetMessageDTO { Message = x.MassageText, SendAT = x.SendAT }).ToListAsync();
            if(Message.Count == 0)
                return Result<List<GetMessageDTO>?>.Fail("There Is No Message");
            return Result<List<GetMessageDTO>?>.SuccessResult(Message, "Success");

        }
    }
}
