using DTO;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GroupSvc
    {
        AppChatContext _context;

        public GroupSvc(AppChatContext context)
        {
            _context = context;
        }

        public async Task<List<GroupDto>> GetGroupNameByAccountId(int accountId)
        {
            var result = new List<GroupDto>();
            var groups = _context.Group.Include(item => item.Account)
                                        .Where(group => group.Account.Any(account => account.Id == accountId));

            foreach (var item in groups)
            {
                result.Add(item.GetDto());
            }

            return result;
        }
        public async Task<List<GroupDto>> GetAllGroup()
        {
            var allGroupsDto = new List<GroupDto>();
            var allGroup = _context.Group.ToList();

            foreach (var item in allGroup)
            {
                allGroupsDto.Add(item.GetDto());
            }

            return allGroupsDto;
        }
        public async Task AddAccountIntoGroup(string groupName, List<AccountDto> accountsDto)
        {
            var oldGroup = await _context.Group.FirstOrDefaultAsync(group => group.Name.Equals(groupName));
            var accountsNeedAdd = new List<AccountDto>();

            if (oldGroup != null)
            {
                //xu ly truong hop add user da o trong group tu truoc
                for (int i = 0; i < accountsDto.Count; i++)
                {
                    if (!oldGroup.Account.Any(account => account.Id == accountsDto[i].Id))
                        accountsNeedAdd.Add(accountsDto[i]); //add nhung user dang khong co group can add
                }               

                //add user to group
                foreach (var item in accountsNeedAdd)
                {
                    await _context.Database.ExecuteSqlAsync($"insert into [Account_Group] values({item.Id},{oldGroup.Id})");
                }
            }
            else
            {
                var newGroup = new Group();
                newGroup.Name = groupName;

                foreach (var item in accountsDto)
                {
                    newGroup.Account.Add(await _context.Account.FirstOrDefaultAsync(account => account.Id == item.Id));
                }

                await _context.Group.AddAsync(newGroup);
            }    

            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsGroupNameExits(string groupName)
        {
            return await _context.Group.AnyAsync(group => group.Name.Equals(groupName));
        }
    }
}
