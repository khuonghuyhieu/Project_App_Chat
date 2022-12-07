
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
        public async Task AddMessageUser(MessageReq messageReq)
        {
            //convert Message to MessageUserDto
            var messageUserDto = new MessageUserDto
            {
                SenderId = _accountSvc.GetAccountById(messageReq.SenderId).Result.Id,
                ReceiveId = _accountSvc.GetAccountById(messageReq.ReceiverId).Result.Id,
                Message = messageReq.Content,
                TimeSend = messageReq.TimeSend,
            };

            var messageUser = new MessageUser();
            messageUser.SetDto(messageUserDto);

            await _context.MessageUser.AddAsync(messageUser);
            await _context.SaveChangesAsync();
        }
        public async Task<List<MessageOldRes>> GetOldMessageUser(MessageOldReq messageOld)
        {
            var messageOldRes = new List<MessageOldRes>();
            var messageUser = _context.MessageUser.Where(message =>
                                                        (message.SenderId == messageOld.SenderId && message.ReceiveId == messageOld.ReceiverId)
                                                     || (message.SenderId == messageOld.ReceiverId && message.ReceiveId == messageOld.SenderId))
                                                   .OrderBy(mess => mess.TimeSend);

            foreach (var item in messageUser)
            {
                messageOldRes.Add(new MessageOldRes
                {
                    FullName = item.Sender.FullName,
                    Message = item.Message,
                });
            }

            return messageOldRes;
        }
    }
}
