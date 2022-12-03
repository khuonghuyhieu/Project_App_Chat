using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MessageGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Message { get; set; }
        public DateTime TimeSend { get; set; }
    }
}
