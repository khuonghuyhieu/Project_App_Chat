using DTO;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GroupSvc
    {
        AppChatContext _context;

        public GroupSvc(AppChatContext context)
        {
            _context = context;
        }

        public async Task<List<GroupDto>> GetGroupNameByAccountId(int accountId)
        {
            var result = new List<GroupDto>();
            var groups = _context.Group.Include(item => item.Account)
                                        .Where(group => group.Account.Any(account => account.Id == accountId));

            foreach (var item in groups)
            {
                result.Add(item.GetDto());
            }

            return result;
        }
    }
}
