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

        public List<string> GetGroupByUserName(string userName)
        {
            var result = new List<string>();
            var groups = _context.Group.Include(item => item.Account).Where(group => group.Account.Any(account => account.UserName.Equals(userName)));
            //var a = _context.Account.Include(account => account.Group).ToList();

            foreach (var item in groups)
            {
                result.Add(item.GetDto().Name);
            }

            return result;
        }
    }
}
