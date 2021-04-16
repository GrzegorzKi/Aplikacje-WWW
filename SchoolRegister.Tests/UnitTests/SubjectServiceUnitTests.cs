using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
  public class SubjectServiceUnitTests : BaseUnitTests {
    private readonly ISubjectService _subjectService;

    public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext) {
      _subjectService = subjectService;
    }
    
    [Fact]
    public void AddSubject() {
      var addOrUpdateSubjectVm = new AddOrUpdateSubjectVm {
        Name = "Test subject",
        TeacherId = 1
      };

      var subject = _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);

      Assert.NotNull(subject);
      Assert.Equal("Test subject", subject.Name);
      Assert.Equal(1, subject.TeacherId);
    }

    [Fact]
    public void AddSubjectWithDescription() {
      var addOrUpdateSubjectVm = new AddOrUpdateSubjectVm {
        Name = "Test subject",
        Description = "Description for test subject",
        TeacherId = 1
      };

      var subject = _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);

      Assert.NotNull(subject);
      Assert.Equal("Test subject", subject.Name);
      Assert.Equal("Description for test subject", subject.Description);
      Assert.Equal(1, subject.TeacherId);
    }

    [Fact]
    public void UpdateSubject() {
      var addOrUpdateSubjectVm = new AddOrUpdateSubjectVm {
        Id = 2,
        Name = "Metody programowania"
      };

      var subject = _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);

      Assert.NotNull(subject);
      Assert.Equal(2, subject.Id);
      Assert.Equal("Metody programowania", subject.Name);
    }

    [Fact]
    public void UpdateSubjectWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _subjectService.AddOrUpdateSubject(null));
    }
    
    [Fact]
    public void GetSubject() {
      var subject = _subjectService.GetSubject(s => s.Id == 1);

      Assert.NotNull(subject);
      Assert.Equal(1, subject.Id);
      Assert.Equal("Aplikacje WWW", subject.Name);
    }

    [Fact]
    public void GetSubjectForNonExistentResults() {
      var subject = _subjectService.GetSubject(s => s.Id == -1);

      Assert.Null(subject);
    }

    [Fact]
    public void GetSubjectWithNullArgumentShouldThrowException() {
      Assert.Throws<ArgumentNullException>(() => _subjectService.GetSubject(null));
    }

    [Fact]
    public void GetSubjects() {
      var group = _subjectService.GetSubjects(g => g.Id == 1);

      Assert.NotNull(group);
      Assert.NotEmpty(group);
      Assert.All(group, vm => Assert.Equal(1, vm.Id));
    }

    [Fact]
    public void GetSubjectsWithNullArgument() {
      var group = _subjectService.GetSubjects(null);

      Assert.NotNull(group);
      Assert.NotEmpty(group);
    }

    [Fact]
    public void GetSubjectsForNonExistentResults() {
      var subject = _subjectService.GetSubjects(s => s.Id == -1);

      Assert.NotNull(subject);
      Assert.Empty(subject);
    }
  }
}
