using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;
using MyMessages.Data.Infrastructure;
using MyMessages.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext db;

        public MessageRepository(AppDbContext db)
        {
            this.db = db;
        }

        public void Add(Message message)
        {
            db.Messages.Add(message);
        }

        public Task<List<Message>> FindAllAsync(int userId, int? fromId, int? count, params Expression<Func<Message, object>>[] includes)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Argument cannot be less or equal to 0", nameof(count));
            }

            var query = db.Messages
                .Where(message => message.UserId == userId)
                .IncludeMultiple(includes)
                .OrderByDescending(message => message.Id)
                .AsQueryable();

            if (fromId != null)
            {
                // "LESS or equal" because we are going in the descending direction
                query = query.Where(message => message.Id <= fromId);
            }

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            return query.ToListAsync();
        }

        public Task<Message> FindByIdAsync(int id, params Expression<Func<Message, object>>[] includes)
        {
            return db.Messages
                .IncludeMultiple(includes)
                .FirstOrDefaultAsync(message => message.Id == id);
        }

        public void Remove(Message message)
        {
            db.Messages.Remove(message);
        }

        public void Update(Message message)
        {
            db.Messages.Update(message);
        }
    }
}
