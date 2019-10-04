using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Models
{
    public class MockStudentRespository : IStudentRepository
    {
        private List<Student> _studentList;

        public MockStudentRespository()
        {
            _studentList = new List<Student>() {
                new Student() {Id =1,Name ="张三",ClassName=ClassNameEnum.FirstGrade,Email="Tony-zhang@52abp.com" },
                 new Student() {Id =2,Name ="李四",ClassName=ClassNameEnum.SecondGrade,Email="Tony-zhang@52abp.com" },
                  new Student() {Id =3,Name ="王老五",ClassName=ClassNameEnum.ThreeGrade,Email="Tony-zhang@52abp.com"  },
            };
        }

        public Student GetStudent(int id)
        {
            return _studentList.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _studentList;
        }

        public Student Add(Student student)
        {
            student.Id = _studentList.Max(s => s.Id) + 1;
            _studentList.Add(student);
            return student;
            //wyjaixuexi
        }

        public Student Update(Student updateStudent)
        {
            Student student = _studentList.FirstOrDefault(s => s.Id == updateStudent.Id);
            if (student != null)
            {
                student.Name = updateStudent.Name;
                student.Email = updateStudent.Email;
                student.ClassName = updateStudent.ClassName;
            }
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _studentList.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _studentList.Remove(student);
            }
            return student;
        }
    }
}