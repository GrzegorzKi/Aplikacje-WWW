using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Services.Services {
  public class GroupService : BaseService, IGroupService {
    public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
      : base(dbContext, mapper, logger) { }
  }
}
