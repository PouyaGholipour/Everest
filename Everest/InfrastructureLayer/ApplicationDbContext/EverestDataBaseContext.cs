using DomainLayer.Entities;
using InfrastructureLayer.Configs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.ApplicationDbContext
{
    public class EverestDataBaseContext : DbContext
    {
        
        public EverestDataBaseContext(DbContextOptions<EverestDataBaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new CommentConfig());
            modelBuilder.ApplyConfiguration(new CourseConfig());
            modelBuilder.ApplyConfiguration(new JournalConfig());
            modelBuilder.ApplyConfiguration(new ProgConfig());
            modelBuilder.ApplyConfiguration(new ReportConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RoleUserConfig());
            modelBuilder.ApplyConfiguration(new CourseUserConfig());
            modelBuilder.ApplyConfiguration(new ProgUserConfig());
            modelBuilder.ApplyConfiguration(new PermissionConfig());
            modelBuilder.ApplyConfiguration(new RolePermissionConfig());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Prog> Progs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<ProgUser> ProgUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
