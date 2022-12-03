using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Group
    {
        AppChatContext _context;

        public Group(AppChatContext context)
        {
            _context = context;
        }
    }
}
