
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UploadFile.Models;
namespace Upload_File.Models
{
    public class dbContext:DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
    }
}
