using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Domain.Entities;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Anonymous.Domain.Repository
{
    public class ActionsRepository : IAction
    {
        private readonly BotDbContext _context;

        public ActionsRepository(BotDbContext context)
        {
            _context = context;
        }

        public IQueryable<ActionData> Get(int? limit = null, int? offset = null)
        {
            var actions = _context.Actions.Skip(offset ?? 0)
                .Take(limit ?? 25);

            return actions;
        }

        public async Task<ActionData?> GetByUserId(long userId)
        {
            var userAction = await _context.Actions.FirstOrDefaultAsync(x => x.UserId == userId);

            return userAction;
        }

        public async Task<ActionData> SaveAction(
            long userId, 
            int? actionId = null, 
            int? actionStep = null)
        {
            var action = await GetByUserId(userId: userId);

            if (action == null)
            {
                action = new ActionData()
                {
                    UserId = userId,
                    ChatId = userId,
                    CurrentAction = actionId,
                    CurrentStep = actionStep
                };

                _context.Actions.Add(action);
            }
            else
            {
                action.CurrentAction = actionId;
                action.CurrentStep = actionStep;

                _context.Actions.Update(action);
            }

            await _context.SaveChangesAsync();

            return action;
        }
    }
}
