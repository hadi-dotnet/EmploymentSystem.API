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

namespace Job.Services.Services
{
    public class ConversationService : IConversationService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;

        public ConversationService(AppDBContext context, IUserService userService)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<Result> CreateConversation(string userId2)
        {
            var userId1 = _userService.GetCuurentUserID();
            if (string.IsNullOrWhiteSpace(userId2))
                return Result.Fail("Invalid User ID");

            var existingConversation = await _dbContext.Conversations
                .SingleOrDefaultAsync(x =>
                    x.UserID1 == userId1 && x.UserID2 == userId2 ||
                    x.UserID1 == userId2 && x.UserID2 == userId1);

            if (existingConversation != null)
                return Result.Fail("The Conversation Already Exists");

            var conversation = new Conversations
            {
                UserID1 = userId1,
                UserID2 = userId2,
                CreatedAT = DateTime.UtcNow
            };

            await _dbContext.Conversations.AddAsync(conversation);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Conversation Created Successfully");
        }

        public async Task<Result> SendMessage(MessageDTO messageDto)
        {
            if (messageDto == null || string.IsNullOrWhiteSpace(messageDto.MassageText))
                return Result.Fail("Invalid Message Data");

            var currentUserId = _userService.GetCuurentUserID();
            var conversation = await _dbContext.Conversations
                .FirstOrDefaultAsync(x => x.ID == messageDto.ConversationId);

            if (conversation == null)
                return Result.Fail("Conversation Not Found");

            if (!IsParticipant(conversation, currentUserId))
                return Result.Fail("You Have No Access To This Conversation");

            var message = new Messages
            {
                ConversationId = conversation.ID,
                MassageText = messageDto.MassageText,
                SenderBy = currentUserId,
                SendAT = DateTime.UtcNow
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Message Sent Successfully");
        }

        public async Task<Result<List<GetConversationDTO>>> GetConversations()
        {
            var userId = _userService.GetCuurentUserID();

            var conversations = await _dbContext.Conversations
                .Where(x => x.UserID1 == userId || x.UserID2 == userId)
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
                    ReceiverName = GetReceiverName(con, userId)
                })
                .ToListAsync();

            return Result<List<GetConversationDTO>>.SuccessResult(conversations, "Success");
        }

        public async Task<Result<List<GetMessageDTO>?>> GetMessages(int conversationId)
        {
            var userId = _userService.GetCuurentUserID();

            var messages = await _dbContext.Messages
                .Where(x => x.ConversationId == conversationId &&
                            (x.Conversation.UserID1 == userId || x.Conversation.UserID2 == userId))
                .Select(x => new GetMessageDTO
                {
                    Message = x.MassageText,
                    SendAT = x.SendAT
                })
                .ToListAsync();

            if (!messages.Any())
                return Result<List<GetMessageDTO>?>.Fail("No Messages Found");

            return Result<List<GetMessageDTO>?>.SuccessResult(messages, "Success");
        }



        private static bool IsParticipant(Conversations conversation, string userId) =>
            conversation.UserID1 == userId || conversation.UserID2 == userId;

        private static string GetReceiverName(Conversations con, string currentUserId)
        {
            if (con.UserID1 == currentUserId)
            {
                return con.AppUser2.UserType switch
                {
                    UserTypeEnum.Employee => $"{con.AppUser2.Employees.FirstName} {con.AppUser2.Employees.secoundName} {con.AppUser2.Employees.LastName}",
                    UserTypeEnum.Company => con.AppUser2.Company.Name,
                    _ => "Not Found"
                };
            }

            if (con.UserID2 == currentUserId)
            {
                return con.AppUser1.UserType switch
                {
                    UserTypeEnum.Employee => $"{con.AppUser1.Employees.FirstName} {con.AppUser1.Employees.secoundName} {con.AppUser1.Employees.LastName}",
                    UserTypeEnum.Company => con.AppUser1.Company.Name,
                    _ => "Not Found"
                };
            }

            return "Not Found";
        }
    }

}
