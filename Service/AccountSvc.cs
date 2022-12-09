using DTO;
using Microsoft.EntityFrameworkCore;
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
            if (await _context.Account.FirstOrDefaultAsync(account => account.UserName.Equals(userName)) == null)
                return false;

            return true;
        }
        public async Task AddAccount(AccountDto accountDto)
        {
            var account = new Account();
            account.SetDto(accountDto);

            if (!await IsAccountExits(accountDto.UserName))
            {
                await _context.Account.AddAsync(account);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsHaveAccount(string userName, string password)
        {
            if (await _context.Account.FirstOrDefaultAsync(account => account.UserName.Equals(userName) && account.Password.Equals(password)) == null)
                return false;

            return true;
        }
        public async Task<Dictionary<int,string>> GetIdAndFullNameByAccountId(params int[] idAccount)
        {
            var accounts = _context.Account.Where(account => idAccount.Contains(account.Id)).ToList();
            var result = new Dictionary<int, string>(); //id(account): fullName

            foreach (var item in accounts)
            {
                result.Add(item.GetDto().Id,item.GetDto().FullName);
            }

            return result ;
        }
        public async Task<AccountDto> GetAccountByUserName(string userName)
        {
            var account = await _context.Account.FirstOrDefaultAsync(account => account.UserName.Equals(userName));

            if (account == null)
                return null;
            
            return account.GetDto();
        }
        public async Task<AccountDto> GetAccountById(int accountId)
        {
            var account = await _context.Account.FirstOrDefaultAsync(account => account.Id == accountId);

            if (account == null)
                return null;

            return account.GetDto();
        }
        public async Task<List<int>> GetAccountsIdInGroupByGroupId(int groupId)
        {
            var accountsId = new List<int>();
            var accounts = _context.Account.Include(item => item.Group)
                                               .Where(account => account.Group.Any(group => group.Id == groupId));

            foreach (var item in accounts)
            {
                accountsId.Add(item.Id);
            }

            return accountsId;
        }
        public async Task<Dictionary<int, string>> GetAllAccount()
        {
            var result = new Dictionary<int, string>(); //id(account): fullName
            var allAccounts = _context.Account.ToList();

            foreach (var item in allAccounts)
            {
                result.Add(item.GetDto().Id, item.GetDto().FullName);
            }

            return result;
        }
    }
}
