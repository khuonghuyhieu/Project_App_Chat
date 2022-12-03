using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public partial class Group
    {
        public DTO.Group GetDto()
        {
            var groupDto = new DTO.Group
            {
                Id = this.Id,
                Name = this.Name,
                
            };

            return groupDto;
        }

        public void SetDto(DTO.Group groupDto)
        {
            this.Id = groupDto.Id;
            this.Name = groupDto.Name;
            
        }
    }
}
