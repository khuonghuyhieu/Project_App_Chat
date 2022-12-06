using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MessageGroupSvc
    {
        AppChatContext _context;

        public MessageGroupSvc(AppChatContext context)
        {
            _context = context;
        }
    }
}
