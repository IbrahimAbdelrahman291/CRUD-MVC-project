﻿using DemoBLL.Interfaces;
using DemoDAL.Contexts;
using DemoDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MVCAppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
