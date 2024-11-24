using DemoBLL.Interfaces;
using DemoDAL.Contexts;
using DemoDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Repository
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MVCAppDbContext dbContext):base(dbContext)
        {

        }
    }
}
