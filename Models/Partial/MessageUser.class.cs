using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class MessageUser
    {
        public DTO.MessageUserDto GetDto()
        {
            var messageUserDto = new DTO.MessageUserDto
            {
                Id = this.Id,
                SenderId = this.SenderId,
                ReceiveId = this.ReceiveId,              
                Message = this.Message,
                TimeSend = this.TimeSend,
            };

            return messageUserDto;
        }

        public void SetDto(DTO.MessageUserDto messageUserDto)
        {
            this.Id = messageUserDto.Id;
            this.SenderId = messageUserDto.SenderId;
            this.ReceiveId = messageUserDto.ReceiveId;
            this.Message = messageUserDto.Message;
            this.TimeSend = messageUserDto.TimeSend;
        }
    }
}
