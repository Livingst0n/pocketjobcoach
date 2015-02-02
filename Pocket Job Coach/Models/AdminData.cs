using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Job_Coach.Models
{
    public class AdminData
    {
        public int id { get; set; }
        [Required(ErrorMessage="Please enter your first name.")]
        [DataType(DataType.Text)]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [DataType(DataType.Text)]
        public string lastName { get; set; }

    }

    public class pjcDbContext : DbContext{
        public DbSet<AdminData> AdminDatas { get; set; }
        
        }
}
