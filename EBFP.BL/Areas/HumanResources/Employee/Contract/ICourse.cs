
namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    public interface ICourse :  IRepository<tblCourses, CourseModel>
    {
        string GetCourseName(int course_Id);
    }
}