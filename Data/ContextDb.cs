namespace Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    public class ContextDb : DbContext
    {
        private string _connectionString;

        public ContextDb(DbContextOptions options, string connectionString = null)
            : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                options.UseSqlServer(_connectionString);
            }
            base.OnConfiguring(options);
        }
        public virtual DbSet<TaskToDo> TaskToDo { get; set; }
    }
}