using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MessageUserSvc
    {
        AppChatContext _context;

        public MessageUserSvc(AppChatContext context)
        {
            _context = context;
        }
    }
}
