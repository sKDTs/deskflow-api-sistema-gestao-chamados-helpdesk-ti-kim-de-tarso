using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class DeskFlowDbContext : DbContext
{
    public DeskFlowDbContext(DbContextOptions<DeskFlowDbContext> options)
        : base(options)
    {
    }
}
