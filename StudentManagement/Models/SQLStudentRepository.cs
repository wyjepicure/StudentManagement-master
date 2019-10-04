using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly ILogger<SQLStudentRepository> logger;
        private readonly AppDbContext _context;

        public SQLStudentRepository(AppDbContext appDbContext, ILogger<SQLStudentRepository> logger)
        {
            this.logger = logger;
            _context = appDbContext;
        }

        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }

        public Student GetStudent(int id)
        {
            Student student = _context.Students.Find(id);
            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            logger.LogTrace("学生信息Trace(跟踪) Log");
            logger.LogDebug("学生信息Debug(调试) Log");
            logger.LogInformation("学生信息信息(Information) Log");
            logger.LogWarning("学生信息警告(Warning) Log");
            logger.LogError("学生信息错误(Error) Log");
            logger.LogCritical("学生信息严重(Critical) Log");
            return _context.Students;
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}