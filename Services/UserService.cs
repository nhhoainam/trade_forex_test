using HandleMessage.Data;
using HandleMessage.Model;
using Microsoft.EntityFrameworkCore;

namespace HandleMessage.Services;

public class UserService : IService<User>
{
    private readonly TradingSystemContext _context;
    public UserService(TradingSystemContext context)
    {
        _context = context;
    }

    public IEnumerable<User> findAll()
    {
        return _context.Users.Include(u => u.accounts).ToList();
    }
}