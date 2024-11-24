using AutoMapper;
using DemoBLL.Interfaces;
using DemoBLL.Repository;
using DemoDAL.Models;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUintOfWork uintOfWork,IMapper mapper)
        {
            //_departmentRepository = new DepartmentRepository();
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _uintOfWork.departmentRepository.GetAllAsync();
            var MappedDeparments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDeparments);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM) 
        {
            if (ModelState.IsValid)
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _uintOfWork.departmentRepository.AddAsync(MappedDepartment);
                int Result = await _uintOfWork.CompleteAsync();
                if (Result>0)
                {
                    TempData["Message"] = "Department is created";    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Details(int? id ,string viewName = "Details") 
        {
            if (id is null)
            {
                BadRequest();   
            }
            var department = await _uintOfWork.departmentRepository.GetByIdAsync(id.Value);
            var MappedEmployee = _mapper.Map<Department, DepartmentViewModel>(department);
            if (department is null) 
            {
                return NotFound();
            }
            return View(viewName, MappedEmployee);//view name to return spacific view (don't repeat your self)
        }
        public async Task<IActionResult> Edit(int? id) 
        {
            //if (id is null)
            //    BadRequest();
            //var dept = _departmentRepository.GetById(id.Value);
            //if (dept is null)
            //    NotFound();
            //return View(dept);
            return await Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM, [FromRoute] int id) 
        {
            if (departmentVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _uintOfWork.departmentRepository.Update(MappedDepartment);
                    await _uintOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex) 
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Delete(int? id) 
        {
            if(id is null)
                return BadRequest();
            var dept = await _uintOfWork.departmentRepository.GetByIdAsync(id.Value);
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(dept);
            if (dept is null)
                return NotFound();
            return View(MappedDepartment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM, [FromRoute] int id) 
        {
            if (departmentVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid) 
            {
                try 
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _uintOfWork.departmentRepository.Delete(MappedDepartment);
                    await _uintOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex) 
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
    }
}
