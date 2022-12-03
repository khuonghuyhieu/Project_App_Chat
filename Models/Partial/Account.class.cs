using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class Account
    {
        public DTO.Account GetDto ()
        {
            var accountDto = new DTO.Account
            {
                Id = this.Id,
                UserName = this.UserName,
                Password = this.Password,
                FullName = this.FullName,
            };

            return accountDto;
        }

        public void SetDto(DTO.Account accountDto)
        {
            this.Id = accountDto.Id;
            this.UserName = accountDto.UserName;
            this.Password = accountDto.Password;
            this.FullName = accountDto.FullName;
        }
    }
}
