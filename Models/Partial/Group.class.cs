using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class Group
    {
        public DTO.GroupDto GetDto()
        {
            var groupDto = new DTO.GroupDto
            {
                Id = this.Id,
                Name = this.Name,

            };

            return groupDto;
        }

        public void SetDto(DTO.GroupDto groupDto)
        {
            this.Id = groupDto.Id;
            this.Name = groupDto.Name;

        }
    }
}
