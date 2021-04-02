using MyMessages.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> FindAllAsync(int userId, int? fromId, int? count, params Expression<Func<Message, object>>[] includes);
        Task<Message> FindByIdAsync(int id, params Expression<Func<Message, object>>[] includes);
        void Add(Message message);
        void Update(Message message);
        void Remove(Message message);
    }
}
