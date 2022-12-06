using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using WorkForceGuards.Models;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Backup;

namespace WorkForceManagementV0.Contexts
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {
            Database.SetCommandTimeout(TimeSpan.FromSeconds(120000));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.HasDefaultSchema("dbo");
            builder.Entity<Asset>()
                .HasIndex(a => a.Barcode)
                .IsUnique();
            builder.Entity<StaffMember>()
                .HasIndex(s => s.EmployeeId)
                .IsUnique();
            builder.Entity<StaffMember>()
                .HasIndex(s => s.Alias)
                .IsUnique();
            builder.Entity<StaffMember>()
                .HasIndex(s => s.Name)
                .IsUnique();
            builder.Entity<StaffMember>()
                .HasIndex(s => s.Email)
                .IsUnique();
            builder.Entity<HeadOfSection>()
                .HasIndex(h => h.EmployeeId)
                .IsUnique();
            builder.Entity<HeadOfSection>()
            .HasIndex(h => h.Alias)
            .IsUnique();
            builder.Entity<Location>()
                .HasIndex(l => l.Name)
                .IsUnique();
            builder.Entity<AssetType>()
                .HasIndex(a => a.Name)
                .IsUnique();
            builder.Entity<AssetTerm>()
                .HasIndex(a => a.Name)
                .IsUnique();
            builder.Entity<StaffType>()
                .HasIndex(s => s.Name)
                .IsUnique();
            builder.Entity<TransportationRoute>()
                .HasIndex(t => t.Name)
                .IsUnique();
            builder.Entity<Shift>()
                .HasIndex(e => e.Name)
                .IsUnique();

            builder.Entity<AttendanceType>()
                .HasIndex(t => t.Name)
                .IsUnique();
            builder.Entity<Activity>()
                .HasIndex(t => t.Name)
                .IsUnique();

            builder.Entity<DailyAttendance>(entity => {
                entity.HasIndex(e => new { e.StaffMemberId, e.ScheduleId, e.AttendanceTypeId, e.TransportationRouteId,e.Day })
                .IsUnique();
            });
            builder.Entity<Forecast>()
               .HasIndex(a => a.Name)
               .IsUnique();

            builder.Entity<Schedule>().HasIndex(t => t.Name).IsUnique();

            builder.Entity<Activity>().HasIndex(t => t.Name).IsUnique();


            builder.Entity<Activity>().HasIndex(t => t.Color).IsUnique();



            builder.Entity<Shift>()
                    .HasIndex(t => t.Name)
                    .IsUnique();

            builder.Entity<Color>()
                    .HasIndex(t => t.ColorName)
                    .IsUnique();

            //-----------------------------------------------Add Soft Deleted
            builder.Entity<Asset>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<AssetType>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<HeadOfSection>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<Location>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<StaffMember>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<StaffType>().HasQueryFilter(s => !s.IsDeleted);
                    //builder.Entity<TransportationRoute>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<BreakTypeOption>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<DayOffOption>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<ShiftRule>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<Activity>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<Color>().HasQueryFilter(s => !s.IsDeleted);
                    builder.Entity<Shift>().HasQueryFilter(s => !s.IsDeleted);

            builder.Entity<DailyAttendancePattern>()
            .Property(e => e.DayOffs)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

        }

     

        public DbSet<Location> Locations { get; set; }
        public DbSet<TransportationRoute> TransportationRoutes { get; set; }
        public DbSet<HeadOfSection> HeadOfSections { get; set; }
        public DbSet<StaffType> StaffTypes { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<AssetTerm> AssetTerms { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetTermValue> AssetTermValues { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<DailyAttendance> DailyAttendances { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ForeCasting> ForeCastings { get; set; }
        public DbSet<BreakTypeOption> breakTypeOptions { get; set; }

        public DbSet<DayOffOption> dayOffOptions { get; set; }

        public DbSet<ShiftRule> shiftRules { get; set; }

        public DbSet<Color> colors { get; set; }
        public DbSet<Interval> Intervals { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<ForecastDetails> ForecastDetails { get; set; }
        public DbSet<TmpForecastDetails> TmpForecastDetails { get; set; }

        public DbSet<ScheduleDetail> ScheduleDetail { get; set; }
        public DbSet<TmpScheduleDetail> TmpScheduleDetails { get; set; }
        public DbSet<ForecastHistory> ForecastHistories { get; set; }
        //public DbSet<Lock> Locks { get; set; }
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }
        public DbSet<SwapRequestStatus> SwapRequestStatuses { get; set; }
        public DbSet<SwapRequestDetail> SwapRequestDetails { get; set; }
        public DbSet<BkpDailyAttendance> BkpDailyAttendances { get; set; }
        public DbSet<BkpScheduleDetail> BkpScheduleDetails { get; set; }
        public DbSet<IpccAgent> IpccAgents { get; set; }
        public DbSet<SubLocation> SubLocations { get; set; }
        public DbSet<DailyAttendancePattern> DailyAttendancePatterns { get; set; }

    }
}
