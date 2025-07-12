using MiCampus.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Database
{
    public class CampusDbContext : IdentityDbContext<
        UserEntity,
        RoleEntity,
        string
        >
    {
        public CampusDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetIdentityTablesNames(builder);
        }

        private static void SetIdentityTablesNames(ModelBuilder builder)
        {
            builder.Entity<UserEntity>().ToTable("sec_users");
            builder.Entity<RoleEntity>().ToTable("sec_roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("sec_users_roles")
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<IdentityUserClaim<string>>().ToTable("sec_users_claims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("sec_roles_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("sec_users_logins");
            builder.Entity<IdentityUserToken<string>>().ToTable("sec_users_tokens");
        }

        public DbSet<CampusEntity> Campuses { get; set; }
        public DbSet<ContentEntity> Contents { get; set; }
        public DbSet<ContentFeedbackEntity> ContentsFeedbacks { get; set; }
        public DbSet<ContentImageEntity> ContentsImages { get; set; }
        public DbSet<SubjectEntity> Subjetcs { get; set; }
        public DbSet<SubjectTakenEntity> SubjectsTaken { get; set; }
        public DbSet<UniversityCareerEntity> UniversityCareers { get; set; }

    }
}   
