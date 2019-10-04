using System.Collections.Generic;

namespace StudentManagement.Models
{
    public interface IStudentRepository
    {/// <summary>
     /// 通过id获取学生信息
     /// </summary>
     /// <param name="id"></param>
     /// <returns></returns>
        Student GetStudent(int id);

        /// <summary>
        /// 得到所有学生信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetStudents();

        /// <summary>
        /// 添加学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Add(Student student);

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Update(Student student);

        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Delete(int id);
    }
}