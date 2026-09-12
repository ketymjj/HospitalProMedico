using HospitalProMedico.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Hospitalization> Hospitalizations => Set<Hospitalization>();
    public DbSet<Surgery> Surgeries => Set<Surgery>();
    public DbSet<HealthPlan> HealthPlans => Set<HealthPlan>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Role).HasConversion<string>();
            e.HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasIndex(x => x.Cpf).IsUnique();
            e.Property(x => x.Gender).HasConversion<string>();
            e.Property(x => x.BloodType).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Name).HasMaxLength(200);
            e.HasOne(x => x.HealthPlan).WithMany(h => h.Patients).HasForeignKey(x => x.HealthPlanId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Doctor>(e =>
        {
            e.HasIndex(x => x.Crm).IsUnique();
            e.Property(x => x.Specialty).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.Property(x => x.Type).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.HasOne(x => x.Patient).WithMany(p => p.Appointments).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany(d => d.Appointments).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Hospitalization>(e =>
        {
            e.Property(x => x.Type).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.HasOne(x => x.Patient).WithMany(p => p.Hospitalizations).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany(d => d.Hospitalizations).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Surgery>(e =>
        {
            e.Property(x => x.Status).HasConversion<string>();
            e.HasOne(x => x.Patient).WithMany(p => p.Surgeries).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany(d => d.Surgeries).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HealthPlan>(e =>
        {
            e.Property(x => x.Type).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.Property(x => x.Method).HasConversion<string>();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Patient).WithMany(p => p.Payments).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MedicalRecord>(e =>
        {
            e.HasOne(x => x.Patient).WithMany(p => p.MedicalRecords).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Doctor).WithMany(d => d.MedicalRecords).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.Property(x => x.Type).HasConversion<string>();
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
