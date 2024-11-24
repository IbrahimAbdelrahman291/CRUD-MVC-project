using DemoBLL.Interfaces;
using DemoDAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Repository
{
    public class UnitOfWork : IUintOfWork ,IDisposable
    {
        private readonly MVCAppDbContext _dbContext;

        public IDepartmentRepository departmentRepository { get ; set; }
        public IEmployeeRepository employeeRepository { get; set ; }

        public UnitOfWork(MVCAppDbContext dbContext)
        {
            departmentRepository = new DepartmentRepository(dbContext);
            employeeRepository = new EmployeeRepository(dbContext);
            _dbContext = dbContext;
        }
        public Task<int> CompleteAsync() 
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
