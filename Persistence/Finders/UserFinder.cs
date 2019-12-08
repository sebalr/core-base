using System.Linq;
using CoreBase.Entities;

namespace CoreBase.Persistance.finders
{
    public interface IUserFinder : IBaseFinder<User>
    {
        User getByUsername(string username);
    }

    public class UserFinder : BaseFinder<User>, IUserFinder
    {
        public UserFinder(DatabaseContext baseContext) : base(baseContext) { }

        public User getByUsername(string username)
        {
            return _dbSet.Where(x => x.Username == username)
                .FirstOrDefault();
        }
    }
}
