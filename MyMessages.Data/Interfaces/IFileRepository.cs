using MyMessages.Data.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IFileRepository
    {
        Task<File> FindByIdAsync(int id, params Expression<Func<File, object>>[] includes);
        void Remove(File file);
    }
}
