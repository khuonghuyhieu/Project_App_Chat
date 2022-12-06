
using ClassLibrary;
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
    public class MessageUserSvc
    {
        AppChatContext _context;
        AccountSvc _accountSvc;

        public MessageUserSvc(AppChatContext context)
        {
            _context = context;
            _accountSvc = new AccountSvc(context);
        }
        public async Task AddMessageUserToUser(Message message)
        {
            //convert Message to MessageUserDto
            var messageUserDto = new MessageUserDto
            {
                SenderId = _accountSvc.GetAccountById(message.SenderId).Result.Id,
                ReceiveId = _accountSvc.GetAccountById(message.ReceiverId).Result.Id,
                Message = message.Content,
                TimeSend = message.TimeSend,
            };

            var messageUser = new MessageUser();
            messageUser.SetDto(messageUserDto);

            await _context.MessageUser.AddAsync(messageUser);
            await _context.SaveChangesAsync();
        }
        public async Task<Dictionary<string,string>> GetOldMessageUser(MessageOld messageOld)
        {
            var messageUsersDto = new Dictionary<string, string>(); //fullName: message
            var messageUser = _context.MessageUser.Where(message =>
                                                        (message.SenderId == messageOld.SenderId && message.ReceiveId == messageOld.ReceiverId)
                                                     || (message.SenderId == messageOld.ReceiverId && message.ReceiveId == messageOld.SenderId))
                                                   .OrderBy(mess => mess.TimeSend);

            foreach (var item in messageUser)
            {
                messageUsersDto.Add(item.Sender.FullName,item.Message);
            }

            return messageUsersDto;
        }
    }
}
