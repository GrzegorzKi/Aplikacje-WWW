using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Services.Services {
  public abstract class BaseService {
    protected readonly ApplicationDbContext DbContext;
    protected readonly IMapper Mapper;
    protected readonly ILogger Logger;

    protected BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) {
      DbContext = dbContext;
      Mapper = mapper;
      Logger = logger;
    }
  }
}
