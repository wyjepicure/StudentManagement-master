using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.IO;

namespace StudentManagement.Controllers
{
    // [Route("Home")]
    
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRespository;
        private readonly HostingEnvironment hostingEnvironment;
        private readonly ILogger logger;

        public HomeController(IStudentRepository studentRespository, HostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _studentRespository = studentRespository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        [Route("")]
        // [Route("Home")]
        // [Route("Home/Index")]
        [Route("Index")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View(_studentRespository.GetStudents());
            //  return Json(new { id = "1", name = "张三" });
        }
        #region 详情
        [Route("{id?}")]
        // [Route("Home/Details/{id?}")]
        
        public IActionResult Details(int id)
        {
            logger.LogTrace("Trace(跟踪) Log");
            logger.LogDebug("Debug(调试) Log");
            logger.LogInformation("信息(Information) Log");
            logger.LogWarning("警告(Warning) Log");
            logger.LogError("错误(Error) Log");
            logger.LogCritical("严重(Critical) Log");
            //throw new Exception("此异常发生在details视图中");
            /* Student model = _studentRespository.GetStudent(1);*/
            Student student = _studentRespository.GetStudent(id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Student = _studentRespository.GetStudent(id),
                PageTitle = "学生详情"
            };
            //弱类型
            /* ViewData["PageTitle"] = "Student Details";
             ViewData["Student"] = model;*/

            //将PageTitle和Student模型对象存储在ViewBag
            //我们正在使用动态属性PageTitle和Stuent

            /* ViewBag.PageTitle = "学生详情信息";
             ViewBag.Student = model;*/

            /*三种写法 1 ~/MyViews/Test.cshtml
                       2 ../ ../MyViews/Test
             */

            //强类型
            return View(homeDetailsViewModel);
            //  return Json(new { id = "1", name = "张三" });
        }
        #endregion

        #region 创建
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //  Student newStudent = _studentRespository.Add(student);
                //    return RedirectToAction("Details", new { id = newStudent.Id });
                string uniqueFileName = null;
                if (model.Photos != null)
                {
                    uniqueFileName = ProcessUploadFile(model);
                }
                Student student = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName
                };
                _studentRespository.Add(student);
                return RedirectToAction("Details", new { id = student.Id });
            }

            return View();
        }
        #endregion

        #region 编辑
        public ViewResult Edit(int id)
        {
            Student student = _studentRespository.GetStudent(id);

            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                ClassName = student.ClassName,
                ExistingPhotoPath = student.PhotoPath
            };

            return View(studentEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentRespository.GetStudent(model.Id);
                student.Email = model.Email;
                student.ClassName = model.ClassName;
                student.Name = model.Name;
                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filepath);
                    }

                    student.PhotoPath = ProcessUploadFile(model);
                }
                Student updateStudent = _studentRespository.Update(student);
                return RedirectToAction("index");
            }
            return View();
        }
        #endregion



        /// <summary>
        /// 将照片保存在文件路径中并并返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //因为使用了非托管资源所以需要手动进行释放
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    model.Photos.CopyTo(fileSteam);
                }
            }
            return uniqueFileName;
        }
    }
}