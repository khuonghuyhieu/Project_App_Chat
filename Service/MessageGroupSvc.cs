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
    public class MessageGroupSvc
    {
        AppChatContext _context;

        public MessageGroupSvc(AppChatContext context)
        {
            _context = context;
        }

        public async Task<List<MessageOldRes>> GetMessageOldGroup(MessageOldReq messageOldReq)
        {
            var messageOldGroup = new List<MessageOldRes>();
            var messageGroup = _context.MessageGroup.Where(messageGroup => messageGroup.GroupId == messageOldReq.GroupId)
                                                    .OrderBy(mess => mess.TimeSend);

            foreach (var item in messageGroup)
            {
                messageOldGroup.Add(new MessageOldRes
                {
                    FullName = item.Sender.FullName,
                    Message = item.Message
                });
            }    

            return messageOldGroup;
        }
        public async Task AddMessageGroup(MessageReq messageReq)
        {
            var messageGroupDto = new MessageGroupDto
            {
                SenderId = messageReq.SenderId,
                GroupId = messageReq.ReceiverId,
                Message = messageReq.Content,
                TimeSend = messageReq.TimeSend,
            };
            var messageGroup = new MessageGroup();
            messageGroup.SetDto(messageGroupDto);

            await _context.MessageGroup.AddAsync(messageGroup);
            await _context.SaveChangesAsync();
        }

    }
}
