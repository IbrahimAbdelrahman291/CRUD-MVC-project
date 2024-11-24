using AutoMapper;
using DemoBLL.Interfaces;
using DemoBLL.Repository;
using DemoDAL.Models;
using DemoPL.Helpers;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUintOfWork uintOfWork,IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string name=null)
        {
            //view data => keyVlauePair [dictionary object]
            //ViewData["Message"] = "Hello view data";
            //view bag dynamic property
            //ViewBag.Messsage = "hello view bag";
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> MappedEmployee;
            if (name is null)
            {
                 employees = await _uintOfWork.employeeRepository.GetAllAsync();
                 MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            }
            else
            {
                employees = await _uintOfWork.employeeRepository.SearchAsync(name);
                MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            }

            return View(MappedEmployee);
        }
        public async Task<IActionResult> Create() 
        {
            ViewBag.Departments = await _uintOfWork.departmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM) 
        {
            if (ModelState.IsValid)
            {
                //Mapping manual
                //var employee = new Employee() 
                //{
                //    Name = employeeVM.Name,
                //    Address = employeeVM.Address,
                //    Age = employeeVM.Age,
                //    CreationDate = employeeVM.CreationDate,
                //    department = employeeVM.department,
                //    Email = employeeVM.Email 
                //};
                // var employee = (Employee)employeeVM //but must do operator overloading
                //Automatic mapping
                 
                employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _uintOfWork.employeeRepository.AddAsync(MappedEmployee);
                await _uintOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id) 
        {
            if (id is null)
            {
                return BadRequest();   
            }
            var employee = await _uintOfWork.employeeRepository.GetByIdAsync(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
            {
                return NotFound();
            }
            return View(MappedEmployee);
        }
        public async Task<IActionResult> Edit(int? id) 
        {
            if(id is null) 
            {
                return BadRequest();
            }
            var employee = await _uintOfWork.employeeRepository.GetByIdAsync(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
            {
                return NotFound();
            }
            return View(MappedEmployee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM , [FromRoute]int id) 
        {
            if (employeeVM.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try 
                {
                    employeeVM.ImageName = DocumentSettings.Upload(employeeVM.Image,"Images");
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _uintOfWork.employeeRepository.Update(MappedEmployee);
                    await _uintOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = await _uintOfWork.employeeRepository.GetByIdAsync(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
            {
                return NotFound();
            }
            return View(MappedEmployee);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM,[FromRoute]int id)
        {
            if (employeeVM.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid) 
            {
                try 
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _uintOfWork.employeeRepository.Delete(MappedEmployee);
                    int Result = await _uintOfWork.CompleteAsync();
                    if (Result > 0)
                    {
                        DocumentSettings.Delete(employeeVM.ImageName,"Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex) 
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }
            return View(employeeVM);
        }
    }
}
