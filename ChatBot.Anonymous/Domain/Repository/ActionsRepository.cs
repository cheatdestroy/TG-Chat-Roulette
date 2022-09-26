using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Domain.Entities;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Anonymous.Domain.Repository
{
    public class ActionsRepository : IAction
    {
        private readonly BotDbContext _context;
        private readonly BotConfiguration _configuration;

        public ActionsRepository(BotDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration.GetMainConfigurationToObject();
        }

        public IQueryable<ActionData> Get(int? limit = null, int? offset = null)
        {
            var actions = _context.Actions
                .Skip(offset ?? _configuration.DefaultOffset)
                .Take(limit ?? _configuration.DefaultLimit);

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
                action.LastUpdate = DateTime.Now;

                _context.Actions.Update(action);
            }

            await _context.SaveChangesAsync();

            return action;
        }
    }
}
