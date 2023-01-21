using Microsoft.EntityFrameworkCore;
using TG.ChatBot.Common.Domain.Context;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Domain.Repository.Interfaces;

namespace TG.ChatBot.Common.Domain.Repository
{
    public class UsersIgnoredRepository : IUsersIgnored
    {
        private readonly BotDbContext _context;

        public UsersIgnoredRepository(BotDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IgnoreUsers>> Get(long? ownerId = null, long? targetId = null, int? offset = null, int? limit = null)
        {
            var entities = _context.IgnoreUsers.AsQueryable();

            if (ownerId != null)
            {
                entities = entities.Where(x => x.UserId == ownerId);
            }

            if (targetId != null)
            {
                entities = entities.Where(x => x.IgnoredUserId == targetId);
            }

            var ignoredUsers = await entities
                .Skip(offset ?? 0)
                .ToListAsync();

            return ignoredUsers;
        }
    }
}
