using Microsoft.EntityFrameworkCore;

namespace IdeasVault.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) { }