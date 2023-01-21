using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain.Context;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Domain.Repository.Interfaces;
using TG.ChatBot.Common.Models;

namespace TG.ChatBot.Common.Domain.Repository
{
    public class ChatRoomRepository : IChatRoom
    {
        private readonly BotDbContext _context;

        public ChatRoomRepository(BotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatRoom>> Get(
            Guid? roomId,
            long? firstUserId = null,
            long? secondUserId = null,
            StatusRoom? status = null,
            int? offset = null,
            int? limit = null)
        {
            var entities = _context.ChatRooms.AsQueryable();

            if (roomId != null && roomId != default(Guid))
            {
                entities = entities.Where(x => x.Id == roomId);
            }

            if (firstUserId != null)
            {
                entities = entities.Where(x => x.FirstUserId == firstUserId || x.SecondUserId == firstUserId);
            }
            
            if (secondUserId != null)
            {
                entities = entities.Where(x => x.FirstUserId == secondUserId || x.SecondUserId == secondUserId);
            }

            if (status != null)
            {
                entities = entities.Where(x => x.StatusRoom == (int)status);
            }

            var chatRoom = await entities.Skip(offset ?? 0)
                .ToListAsync();

            return chatRoom;
        }

        public async Task<ChatRoom?> SaveChatRoom(ChatRoom chatRoom)
        {
            if (chatRoom.Id != default(Guid))
            {
                _context.ChatRooms.Update(chatRoom);
            }
            else
            {
                _context.ChatRooms.Add(chatRoom);
            }

            await _context.SaveChangesAsync();

            var updatedChatRoom = await _context.ChatRooms
                .Include(x => x.FirstUser)
                .Include(x => x.SecondUser)
                .FirstOrDefaultAsync(x => x == chatRoom);

            return updatedChatRoom;
        }
    }
}
