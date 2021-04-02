using MyMessages.Data.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IPictureRepository
    {
        Task<Picture> FindByIdAsync(int id, params Expression<Func<Picture, object>>[] includes);
        void Remove(Picture picture);
    }
}
