using DTO;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountSvc
    {
        AppChatContext _context;

        public AccountSvc(AppChatContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAccountExits(string userName)
        {
            if (_context.Account.FirstOrDefault(account => account.UserName.Equals(userName)) == null)
                return false;

            return true;
        }
        public async Task AddAccount(AccountDto accountDto)
        {
            var account = new Account();
            account.SetDto(accountDto);

            if (!await IsAccountExits(accountDto.UserName))
            {
                _context.Account.Add(account);
                _context.SaveChanges();
            }
        }
        public async Task<bool> IsHaveAccount(string userName, string password)
        {
            if (_context.Account.FirstOrDefault(account => account.UserName.Equals(userName) && account.Password.Equals(password)) == null)
                return false;

            return true;
        }
        public async Task<Dictionary<string,string>> GetUserNameAndFullNameByUserName(params string[] usersName)
        {
            var accounts = _context.Account.Where(account => usersName.Contains(account.UserName)).ToList();
            var result = new Dictionary<string, string>();

            foreach (var item in accounts)
            {
                result.Add(item.GetDto().UserName,item.GetDto().FullName);
            }

            return result ;
        }
        
       
    }
}
