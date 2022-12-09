using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AddAccountsToGroup
    {
        public string GroupName { get; set; }
        public List<AccountDto> Accounts { get; set; }
    }
}
