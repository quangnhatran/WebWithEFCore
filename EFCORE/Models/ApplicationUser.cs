using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EFCORE.Models
{
    public class ApplicationUser:IdentityUser
    {
        //bo sung các thong tin con thieu
        public string FullName { get; set; } //Tên đầy đủ

        public DateTime DateOfBirth { get; set; } //Ngày sinh
    }
}
