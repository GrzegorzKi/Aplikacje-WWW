using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
  public interface ITeacherService {
    TeacherVm AddOrUpdateTeacher(AddOrUpdateTeacherVm addOrUpdateTeacherVm);
    TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterExpression);
    IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpression = null);
  }
}
