using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class MessageGroup
    {
        public DTO.MessageGroupDto GetDto()
        {
            var messageGroupDto = new DTO.MessageGroupDto
            {
                Id = this.Id,
                SenderId = this.SenderId,
                GroupId = this.GroupId,
                Message = this.Message,
                TimeSend = this.TimeSend,
            };

            return messageGroupDto;
        }

        public void SetDto(DTO.MessageGroupDto messageGroupDto)
        {
            this.Id = messageGroupDto.Id;
            this.SenderId = messageGroupDto.SenderId;
            this.GroupId = messageGroupDto.GroupId;
            this.Message = messageGroupDto.Message;
            this.TimeSend = messageGroupDto.TimeSend;
        }
    }
}
