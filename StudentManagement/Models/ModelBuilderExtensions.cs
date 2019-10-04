using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "吴玉龙",
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "123@qq.com"
                },
                new Student
                {
                    Id = 2,
                    Name = "李正才",
                    ClassName = ClassNameEnum.SecondGrade,
                    Email = "124@qq.com"
                }
                );
        }
    }
}