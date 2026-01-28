using Domain.Aggregate;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

public class EnterpriseDBContext : DbContext
{
    public EnterpriseDBContext(DbContextOptions<EnterpriseDBContext> options)
        : base(options) { }

    // --------------------
    // Aggregate Roots
    // --------------------
    public DbSet<Enterprise> Enterprises => Set<Enterprise>();
    public DbSet<WasteType> WasteTypes => Set<WasteType>();

    // --------------------
    // Internal Entities
    // --------------------
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Capacity> Capacities => Set<Capacity>();
    public DbSet<RewardPolicy> RewardPolicies => Set<RewardPolicy>();
    public DbSet<CollectionAssignment> CollectionAssignments => Set<CollectionAssignment>();
    public DbSet<BonusRule> BonusRules => Set<BonusRule>();
    public DbSet<PenaltyRule> PenaltyRules => Set<PenaltyRule>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ====================
        // Enterprise (Aggregate Root)
        // ====================
        modelBuilder.Entity<Enterprise>(entity =>
        {
            entity.HasKey(e => e.EnterpriseID);

            entity.Property(e => e.UserID).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.TIN).IsRequired();
            entity.Property(e => e.AvatarName);
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.ContactInfo).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasIndex(e => e.TIN).IsUnique();

            entity.HasMany(e => e.Members)
                  .WithOne()
                  .HasForeignKey(m => m.EnterpriseID)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Capacities)
                  .WithOne()
                  .HasForeignKey(c => c.EnterpriseID)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.RewardPolicies)
                  .WithOne()
                  .HasForeignKey(r => r.EnterpriseID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================
        // Member
        // ====================
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.MemberID);

            entity.Property(m => m.UserID).IsRequired();
            entity.Property(m => m.AssignedAt).IsRequired();
            entity.Property(m => m.UnassignedAt);

            entity.Property(m => m.EnterpriseID).IsRequired();
        });

        // ====================
        // Capacity
        // ====================
        modelBuilder.Entity<Capacity>(entity =>
        {
            entity.HasKey(c => c.CapacityID);

            entity.Property(c => c.MaxDailyCapacity).IsRequired();
            entity.Property(c => c.RegionCode).IsRequired();
            entity.Property(c => c.CurrentLoad).IsRequired();
            entity.Property(c => c.UnitOfMeasure).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.ClosedAt);

            entity.Property(c => c.EnterpriseID).IsRequired();
            entity.Property(c => c.WasteType).IsRequired();

            entity.HasIndex(c => new { c.EnterpriseID, c.WasteType });

            entity.HasMany(rp => rp.CollectionAssignments)
              .WithOne()
              .HasForeignKey(b => b.CapacityID)
              .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================
        // RewardPolicy
        // ====================
        modelBuilder.Entity<RewardPolicy>(entity =>
        {
            entity.HasKey(r => r.RewardPolicyID);

            entity.Property(r => r.Name).IsRequired();
            entity.Property(r => r.Description);
            entity.Property(r => r.BasePoint).IsRequired();
            entity.Property(r => r.EffectiveDate).IsRequired();
            entity.Property(r => r.ExpiredDate);

            entity.Property(r => r.EnterpriseID).IsRequired();

            entity.HasMany(rp => rp.BonusRules)
                  .WithOne()
                  .HasForeignKey(b => b.RewardPolicyID)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(rp => rp.PenaltyRules)
                  .WithOne()
                  .HasForeignKey(p => p.RewardPolicyID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================
        // BonusRule
        // ====================
        modelBuilder.Entity<BonusRule>(entity =>
        {
            entity.HasKey(b => b.BonusRuleID);

            entity.Property(b => b.Name).IsRequired();
            entity.Property(b => b.Description);
            entity.Property(b => b.BonusPoint).IsRequired();
            entity.Property(b => b.IsActive).HasDefaultValue(true);

            entity.Property(b => b.RewardPolicyID).IsRequired();
        });

        // ====================
        // PenaltyRule
        // ====================
        modelBuilder.Entity<PenaltyRule>(entity =>
        {
            entity.HasKey(p => p.PenaltyRuleID);

            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Description);
            entity.Property(p => p.PenaltyPoint).IsRequired();
            entity.Property(p => p.IsActive).HasDefaultValue(true);

            entity.Property(p => p.RewardPolicyID).IsRequired();
        });

        // ====================
        // CollectionAssignment (Separate Aggregate)
        // ====================
        modelBuilder.Entity<CollectionAssignment>(entity =>
        {
            entity.HasKey(ca => ca.CollectionAssignmentID);

            entity.Property(ca => ca.CollectionReportID);
            entity.Property(ca => ca.AssigneeID).IsRequired();
            entity.Property(ca => ca.Note);
            entity.Property(ca => ca.PriorityLevel).IsRequired();
            entity.Property(ca => ca.AcceptedAt);
            entity.Property(ca => ca.Status).IsRequired();

            entity.Property(ca => ca.CapacityID).IsRequired();
        });

        // ====================
        // WasteType (Lookup Aggregate)
        // ====================
        modelBuilder.Entity<WasteType>(entity =>
        {
            entity.HasKey(w => w.Type);

            entity.Property(w => w.Type).IsRequired();
            entity.Property(w => w.Description);

            entity.HasIndex(w => w.Type).IsUnique();
        });

        // ====================
        // AuditLog
        // ====================
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.AuditLogId);

            entity.Property(a => a.EntityName).IsRequired();
            entity.Property(a => a.Action).IsRequired();
            entity.Property(a => a.PerformedBy).HasMaxLength(100);
            entity.Property(a => a.OldValue);
            entity.Property(a => a.NewValue);

            entity.Property(a => a.Timestamp)
                  .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
