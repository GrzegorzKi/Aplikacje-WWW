using System.Linq;
using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Configuration.Profiles {
  public class MainProfile : Profile {
    public MainProfile() {
      // SubjectService.cs
      CreateMap<Subject, SubjectVm>()
        .ForMember(vm => vm.TeacherName,
          expression => expression.MapFrom(subject => $"{subject.Teacher.FirstName} {subject.Teacher.LastName}"))
        .ForMember(vm => vm.Groups,
          expression => expression.MapFrom(subject => subject.SubjectGroups.Select(group => group.Group)));
      CreateMap<AddOrUpdateSubjectVm, Subject>();
      CreateMap<SubjectVm, AddOrUpdateSubjectVm>();

      // GroupService.cs
      CreateMap<Group, GroupVm>();
      CreateMap<GroupVm, AddOrUpdateGroupVm>();
      CreateMap<AddOrUpdateGroupVm, Group>();
      CreateMap<GroupVm, DeleteGroupVm>();
      CreateMap<DeleteGroupVm, Group>();

      // GradeService.cs
      CreateMap<Grade, GradeVm>()
        .ForMember(vm => vm.GradeValue,
          expression => expression.MapFrom(grade => grade.GradeValue.ToString()))
        .ForMember(vm => vm.StudentName,
          expression => expression.MapFrom(grade => $"{grade.Student.FirstName} {grade.Student.LastName}"));
      CreateMap<AddGradeToStudentVm, Grade>();

      // StudentService.cs
      CreateMap<Student, StudentVm>()
        .ForMember(vm => vm.StudentName,
          expression => expression.MapFrom(student => $"{student.FirstName} {student.LastName}"));
      // CreateMap<AddOrUpdateStudentVm, StudentVm>();
      CreateMap<StudentVm, DetachStudentFromGroupVm>()
          .ForMember(dest => dest.GroupId,
              expression => expression.MapFrom(vm => vm.Group != null ? vm.Group.Id : 0))
          .ForMember(dest => dest.StudentId,
              expression => expression.MapFrom(vm => vm.Id));

      // TeacherService.cs
      CreateMap<Teacher, TeacherVm>()
        .ForMember(vm => vm.TeacherName,
          expression => expression.MapFrom(teacher => $"{teacher.FirstName} {teacher.LastName}"));
      CreateMap<AddOrUpdateTeacherVm, Teacher>();
      CreateMap<CreateEmailVm, EmailVm>();
      CreateMap<Teacher, EmailVm>()
        .ForMember(vm => vm.SenderName,
          expression => expression.MapFrom(teacher => $"{teacher.FirstName} {teacher.LastName}"))
        .ForMember(vm => vm.SenderEmail,
          expression => expression.MapFrom(teacher => teacher.Email));
      CreateMap<Parent, EmailVm>()
        .ForMember(vm => vm.RecipientName,
          expression => expression.MapFrom(parent => $"{parent.FirstName} {parent.LastName}"))
        .ForMember(vm => vm.RecipientEmail,
          expression => expression.MapFrom(parent => parent.Email));

      // ParentService.cs
      CreateMap<Parent, ParentVm>()
          .ForMember(vm => vm.ParentName,
              expression => expression.MapFrom(parent => $"{parent.FirstName} {parent.LastName}"));
    }
  }
}
