using System;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class CourseBL :  Repository<tblCourses, CourseModel>, ICourse
    {
        public CourseBL(EBFPEntities context) : base(context)
        {
        }

        public string GetCourseName(int course_Id)
        {
            try
            {
                var course = BFPContext.tblCourses.Find(course_Id);
                return course?.Course_Name;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
