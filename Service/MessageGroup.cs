using Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MessageGroup
    {
        AppChatContext _context;

        public MessageGroup(AppChatContext context)
        {
            _context = context;
        }
    }
}
