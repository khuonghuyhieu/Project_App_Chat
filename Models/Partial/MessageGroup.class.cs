using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class MessageGroup
    {
        public DTO.MessageGroup GetDto()
        {
            var messageGroupDto = new DTO.MessageGroup
            {
                Id = this.Id,
                GroupId = this.GroupId,
                Message = this.Message,
                TimeSend = this.TimeSend,
            };

            return messageGroupDto;
        }

        public void SetDto(DTO.MessageGroup messageGroupDto)
        {
            this.Id = messageGroupDto.Id;
            this.GroupId = messageGroupDto.GroupId;
            this.Message = messageGroupDto.Message;
            this.TimeSend = messageGroupDto.TimeSend;
        }
    }
}
