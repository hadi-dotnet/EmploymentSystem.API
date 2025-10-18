using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Core.Helper;
using Job.Infrastructure.Context.Configuration;
using Job.Services.JobServices.DTOs.ConversationsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit.Cryptography;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
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
            var UserID1 = _userService.GetCurrentUserID();
            if (UserID1 == null)
                return Result.Fail("Token Error");

            if (string.IsNullOrWhiteSpace(UserID2))
                return Result.Fail("Invalid User ID");

            if (UserID1 == UserID2)
                return Result.Fail("You Cant Create Conversation With Youself");

            var existingConversation = await _dbContext.Conversations
                .SingleOrDefaultAsync(x =>
                    x.UserID1 == UserID1 && x.UserID2 == UserID2 ||
                    x.UserID1 == UserID2 && x.UserID2 == UserID1);

            if (existingConversation != null)
                return Result.Fail("The Conversation Already Exists");

            

            var User1 = await _dbContext.AppUser.Include(x => x.Employees).Include(x => x.Company)
                            .FirstOrDefaultAsync(x => x.Id == UserID1);
            if (User1 == null)
                return Result.Fail("Not Found");

            if (!NameHelper.IsNameExist(User1))
                return Result.Fail("You Have To Enter Your Name");

            var User2 = await _dbContext.AppUser.Include(x => x.Employees).Include(x => x.Company)
                          .FirstOrDefaultAsync(x => x.Id == UserID2);
            if (User2 == null)
                return Result.Fail("Not Found");

            if (!NameHelper.IsNameExist(User2))
                return Result.Fail("You Cant Message The Account , The account does not have a name.");

            var conversation = new Conversations
            {
                UserID1 = UserID1,
                UserID2 = UserID2,
                CreatedAT = DateTime.UtcNow
            };

            await _dbContext.Conversations.AddAsync(conversation);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult($"Conversation Created Successfully .. ConversationID = {conversation.ID}");
        }

        public async Task<Result> SendMessage(MessageDTO messageDto)
        {
            if (messageDto == null || string.IsNullOrWhiteSpace(messageDto.MassageText))
                return Result.Fail("Invalid Message Data");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");
            var conversation = await _dbContext.Conversations
                .FirstOrDefaultAsync(x => x.ID == messageDto.ConversationId);

            if (conversation == null)
                return Result.Fail("Conversation Not Found");

            if (!IsParticipant(conversation, UserID))
                return Result.Fail("You Have No Access To This Conversation");

            var message = new Messages
            {
                ConversationId = conversation.ID,
                MassageText = messageDto.MassageText,
                SenderBy = UserID,
                SendAT = DateTime.Now
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Message Sent Successfully");
        }

        public async Task<Result<List<GetConversationDTO>>> GetConversations(DateTime? lastMessageTime, int pageSize)
        {
            var currentUserId = _userService.GetCurrentUserID();
            if (currentUserId == null)
                return Result<List<GetConversationDTO>>.Fail("Token Error");

            var query = _dbContext.Conversations
                .Where(c => c.UserID1 == currentUserId || c.UserID2 == currentUserId);

            
            if (lastMessageTime.HasValue)
                query = query.Where(c => c.Message.Max(m => m.SendAT) < lastMessageTime.Value);

            var conversations = await query
                .OrderByDescending(c => c.Message.Max(m => m.SendAT))
                .Take(pageSize)
                .Select(c => new GetConversationDTO
                {
                    ConversationID = c.ID,
                    LastMessage = c.Message
                        .OrderByDescending(m => m.SendAT)
                        .Select(m => m.MassageText)
                        .FirstOrDefault(),
                    LastMessageTime = c.Message
                        .OrderByDescending(m => m.SendAT)
                        .Select(m => m.SendAT)
                        .FirstOrDefault(),
                    ReceiverName =
                        c.UserID1 == currentUserId
                            ? (c.AppUser2.UserType == UserTypeEnum.Employee
                                ? $"{c.AppUser2.Employees.FirstName} {c.AppUser2.Employees.secoundName} {c.AppUser2.Employees.LastName}"
                                : c.AppUser2.Company.Name)
                            : (c.AppUser1.UserType == UserTypeEnum.Employee
                                ? $"{c.AppUser1.Employees.FirstName} {c.AppUser1.Employees.secoundName} {c.AppUser1.Employees.LastName}"
                                : c.AppUser1.Company.Name)
                })
                .ToListAsync();

            return Result<List<GetConversationDTO>>.SuccessResult(conversations, "Success");
        }


        public async Task<Result<List<GetMessageDTO>?>> GetMessages(DateTime? lastMessageTime,int PageSize, int conversationId)
        {
            var UserID = _userService.GetCurrentUserID();
            if(UserID==null)
                return Result<List<GetMessageDTO>?>.Fail("Token Error");

            var Query = _dbContext.Messages.Where(x => x.ConversationId == conversationId &&
                            (x.Conversation.UserID1 == UserID || x.Conversation.UserID2 == UserID));

            if (lastMessageTime.HasValue)
                Query = Query.Where(x => x.SendAT < lastMessageTime.Value);

            var Messages =await Query.OrderByDescending(x=>x.SendAT).Take(PageSize)
                .Select(x => new GetMessageDTO
                {
                    ID = x.ID,
                    Message = x.MassageText,
                    SendAT = x.SendAT
                })
                .ToListAsync();
      
            if (!Messages.Any())
                return Result<List<GetMessageDTO>?>.Fail("No Messages Found");

            return Result<List<GetMessageDTO>?>.SuccessResult(Messages, "Success");
        }



        private static bool IsParticipant(Conversations conversation, string userId) =>
            conversation.UserID1 == userId || conversation.UserID2 == userId;


    }

}
