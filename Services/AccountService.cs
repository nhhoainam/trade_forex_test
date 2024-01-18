using HandleMessage.Data;
using HandleMessage.Model;

namespace HandleMessage.Services;

public class AccountService : IService<Account>
{
    private readonly TradingSystemContext _context;
    public AccountService(TradingSystemContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> findAll()
    {
        return _context.Accounts.ToList();
    }
}