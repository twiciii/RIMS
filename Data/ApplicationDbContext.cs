using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RIMS.Models.Entities;
using System.Text.Json;

namespace RIMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - entity class names
        public DbSet<RIMSResident> rimsResidents { get; set; } = null!;
        public DbSet<RIMSResidentCategory> rimsResidentCategories { get; set; } = null!;
        public DbSet<RIMSDocumentApplication> rimsDocumentApplication { get; set; } = null!;
        public DbSet<RIMSDocument> rimsDocuments { get; set; } = null!;
        public DbSet<RIMSAddress> rimsAddresses { get; set; } = null!;
        public DbSet<RIMSStreets> rimsStreets { get; set; } = null!;
        public DbSet<RIMSAuditTrail> rimsAuditTrail { get; set; } = null!;
        public DbSet<RIMSUserActions> rimsUserActions { get; set; } = null!;
        public DbSet<RIMSAssistance> rimsAssistance { get; set; } = null!;
        public DbSet<RIMSHouseholdMembers> rimsHouseholdMembers { get; set; } = null!;

        // Identity tables
        public DbSet<RIMSUsers> rimsUsers { get; set; } = null!;
        public DbSet<RIMSRoles> rimsRoles { get; set; } = null!;
        public DbSet<RIMSUserRoles> rimsUserRoles { get; set; } = null!;
        public DbSet<RIMSUserClaims> rimsUserClaims { get; set; } = null!;
        public DbSet<RIMSRoleClaims> rimsRoleClaims { get; set; } = null!;
        public DbSet<RIMSUserLogins> rimsUserLogins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table names
            builder.Entity<RIMSResident>().ToTable("rimsResidents");
            builder.Entity<RIMSResidentCategory>().ToTable("rimsResidentCategories");
            builder.Entity<RIMSDocumentApplication>().ToTable("rimsDocumentApplication");
            builder.Entity<RIMSDocument>().ToTable("rimsDocuments");
            builder.Entity<RIMSAddress>().ToTable("rimsAddresses");
            builder.Entity<RIMSStreets>().ToTable("rimsStreets");
            builder.Entity<RIMSAuditTrail>().ToTable("rimsAuditTrail");
            builder.Entity<RIMSUserActions>().ToTable("rimsUserActions");
            builder.Entity<RIMSAssistance>().ToTable("rimsAssistance");
            builder.Entity<RIMSHouseholdMembers>().ToTable("rimsHouseholdMembers");
            builder.Entity<RIMSUsers>().ToTable("rimsUsers");
            builder.Entity<RIMSRoles>().ToTable("rimsRoles");
            builder.Entity<RIMSUserRoles>().ToTable("rimsUserRoles");
            builder.Entity<RIMSUserClaims>().ToTable("rimsUserClaims");
            builder.Entity<RIMSRoleClaims>().ToTable("rimsRoleClaims");
            builder.Entity<RIMSUserLogins>().ToTable("rimsUserLogins");

            // Primary keys
            builder.Entity<RIMSResident>().HasKey(r => r.Id);
            builder.Entity<RIMSResidentCategory>().HasKey(rc => rc.Id);
            builder.Entity<RIMSDocumentApplication>().HasKey(da => da.Id);
            builder.Entity<RIMSDocument>().HasKey(d => d.Id);
            builder.Entity<RIMSAddress>().HasKey(a => a.Id);
            builder.Entity<RIMSStreets>().HasKey(s => s.Id);
            builder.Entity<RIMSAuditTrail>().HasKey(at => at.Id);
            builder.Entity<RIMSUserActions>().HasKey(ua => ua.Id);
            builder.Entity<RIMSAssistance>().HasKey(a => a.Id);
            builder.Entity<RIMSHouseholdMembers>().HasKey(hm => hm.Id);

            // Identity primary keys
            builder.Entity<RIMSUsers>().HasKey(u => u.Id);
            builder.Entity<RIMSRoles>().HasKey(r => r.Id);
            builder.Entity<RIMSUserRoles>().HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<RIMSUserClaims>().HasKey(uc => uc.Id);
            builder.Entity<RIMSRoleClaims>().HasKey(rc => rc.Id);
            builder.Entity<RIMSUserLogins>().HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            // Property configurations

            // RIMSResident
            builder.Entity<RIMSResident>(entity =>
            {
                entity.Property(r => r.Id).HasColumnName("ResidentId");
                entity.Property(r => r.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(r => r.MiddleName).HasMaxLength(50);
                entity.Property(r => r.LastName).HasMaxLength(50).IsRequired();
                entity.Property(r => r.Suffix).HasMaxLength(10);
                entity.Property(r => r.DateOfBirth).HasColumnType("date").IsRequired();
                entity.Property(r => r.PlaceOfBirth).HasMaxLength(100).IsRequired();
                entity.Property(r => r.Sex).HasMaxLength(10).IsRequired();
                entity.Property(r => r.Gender).HasMaxLength(10).IsRequired();
                entity.Property(r => r.Nationality).HasMaxLength(50).IsRequired();
                entity.Property(r => r.Address).HasMaxLength(120);
            });

            // RIMSDocument - ADDED THIS CONFIGURATION
            builder.Entity<RIMSDocument>(entity =>
            {
                entity.Property(d => d.Id).HasColumnName("DocumentId");
                entity.Property(d => d.DocumentName).HasMaxLength(255).IsRequired();
                entity.Property(d => d.FilePath).HasMaxLength(255).IsRequired();
                entity.Property(d => d.Description).HasMaxLength(500);
                entity.Property(d => d.Requirements).HasMaxLength(1000);

                // FIX: Add precision for Fee property to resolve the warning
                entity.Property(d => d.Fee)
                      .HasPrecision(18, 2); // 18 total digits, 2 decimal places

                entity.Property(d => d.ProcessingDays).IsRequired();
                entity.Property(d => d.ValidityDays).IsRequired();
                entity.Property(d => d.IsActive).IsRequired();
                entity.Property(d => d.DocumentType).HasMaxLength(50);
                entity.Property(d => d.DocumentCode).HasMaxLength(50);
                entity.Property(d => d.CreatedDate).IsRequired();
                entity.Property(d => d.ModifiedDate);
                entity.Property(d => d.CreatedBy).HasMaxLength(100);
                entity.Property(d => d.ModifiedBy).HasMaxLength(100);
            });

            // RIMSDocumentApplication
            builder.Entity<RIMSDocumentApplication>(entity =>
            {
                entity.Property(da => da.Id).HasColumnName("ApplicationId");
                entity.Property(da => da.EmailAddress).HasMaxLength(50).IsRequired();
                entity.Property(da => da.CivilStatus).HasMaxLength(20).IsRequired();
                entity.Property(da => da.ResidencyStatus).HasMaxLength(50).IsRequired();
                entity.Property(da => da.Religion).HasMaxLength(50).IsRequired();
                entity.Property(da => da.Relationship).HasMaxLength(100);
                entity.Property(da => da.Occupation).HasMaxLength(50).IsRequired();
                entity.Property(da => da.EducationalAttainment).HasMaxLength(50).IsRequired();
                entity.Property(da => da.EmploymentStatus).HasMaxLength(50).IsRequired();
                entity.Property(da => da.MonthlyIncome).HasMaxLength(20).IsRequired();
                entity.Property(da => da.ContactNumber).HasMaxLength(11).IsRequired();
                entity.Property(da => da.PrecinctNo).HasMaxLength(50);
                entity.Property(da => da.PollingPlace).HasMaxLength(255);
                entity.Property(da => da.DateInsurance).HasColumnType("date");
                entity.Property(da => da.PeriodOfValidity).HasColumnType("date");
                entity.Property(da => da.ExpirationDate).HasColumnType("date");
                entity.Property(da => da.OfficeHotline).HasMaxLength(50);
                entity.Property(da => da.Purpose).HasMaxLength(255);
            });

            // RIMSAuditTrail
            builder.Entity<RIMSAuditTrail>(entity =>
            {
                entity.Property(at => at.Id).HasColumnName("AuditId");
                entity.Property(at => at.ActionType).HasMaxLength(50).IsRequired();
                entity.Property(at => at.ModuleName).HasMaxLength(50);
                entity.Property(at => at.UserId).HasMaxLength(100).IsRequired().HasColumnName("FK_UserId");
                entity.Property(at => at.ActionId).HasColumnName("FK_ActionId");
                entity.Property(at => at.IpAddress).HasMaxLength(50);
                entity.Property(at => at.UserAgent).HasMaxLength(256);
                entity.Property(at => at.ActionStatus).HasMaxLength(20);
                entity.Property(at => at.TransactionId).HasMaxLength(50);
                entity.Property(at => at.Remarks).HasMaxLength(500);
            });

            // RIMSAddress
            builder.Entity<RIMSAddress>(entity =>
            {
                entity.Property(a => a.Id).HasColumnName("AddressId");
                entity.Property(a => a.LotNo).HasMaxLength(50).IsRequired();
                entity.Property(a => a.BlockNo).HasMaxLength(50).IsRequired();
                entity.Property(a => a.BldgNo).HasMaxLength(50).IsRequired();
                entity.Property(a => a.Purok).HasMaxLength(100).IsRequired();
                entity.Property(a => a.StreetId).HasColumnName("FK_Id");
            });

            // RIMSHouseholdMembers
            builder.Entity<RIMSHouseholdMembers>(entity =>
            {
                entity.Property(hm => hm.Id).HasColumnName("Id");
                entity.Property(hm => hm.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(hm => hm.MiddleName).HasMaxLength(50);
                entity.Property(hm => hm.LastName).HasMaxLength(50).IsRequired();
                entity.Property(hm => hm.Suffix).HasMaxLength(10);
                entity.Property(hm => hm.Relationship).HasMaxLength(100).IsRequired();
                entity.Property(hm => hm.IsHeadOfHousehold).IsRequired();
                entity.Property(hm => hm.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // RIMSUsers configuration - ADD THIS SECTION
            builder.Entity<RIMSUsers>(entity =>
            {
                entity.Property(u => u.Id)
                    .HasColumnName("UserId")
                    .HasMaxLength(100); // Make sure this matches the FK length

                entity.Property(u => u.UserName).HasMaxLength(256);
                entity.Property(u => u.NormalizedUserName).HasMaxLength(256);
                entity.Property(u => u.Email).HasMaxLength(256);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(256);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
            });

            // Relationships

            // ResidentCategory -> Resident (one-to-many)
            builder.Entity<RIMSResidentCategory>()
                .HasOne(rc => rc.Resident)
                .WithMany(r => r.ResidentCategories)
                .HasForeignKey(rc => rc.ResidentId)
                .HasConstraintName("FK_rimsResidentCategories_ResidentId")
                .OnDelete(DeleteBehavior.Cascade);

            // DocumentApplication -> Resident (one-to-many)
            builder.Entity<RIMSDocumentApplication>()
                .HasOne(da => da.Resident)
                .WithMany(r => r.DocumentApplications)
                .HasForeignKey(da => da.FK_ResidentId)
                .HasConstraintName("FK_rimsDocumentApplication_ResidentId")
                .OnDelete(DeleteBehavior.Restrict);

            // DocumentApplication -> Document (one-to-many)
            builder.Entity<RIMSDocumentApplication>()
                .HasOne(da => da.Document)
                .WithMany(d => d.DocumentApplications)
                .HasForeignKey(da => da.FK_DocumentId)
                .HasConstraintName("FK_rimsDocumentApplication_DocumentId")
                .OnDelete(DeleteBehavior.Restrict);

            // DocumentApplication -> Address (one-to-many)
            builder.Entity<RIMSDocumentApplication>()
                .HasOne(da => da.Address)
                .WithMany(a => a.DocumentApplications)
                .HasForeignKey(da => da.AddressID)
                .HasConstraintName("FK_rimsDocumentApplication_AddressId")
                .OnDelete(DeleteBehavior.Restrict);

            // Address -> Streets (one-to-many)
            builder.Entity<RIMSAddress>()
                .HasOne(a => a.Street)
                .WithMany(s => s.Addresses)
                .HasForeignKey(a => a.StreetId)
                .HasConstraintName("FK_Addresses_Id")
                .OnDelete(DeleteBehavior.Restrict);

            // AuditTrail -> User (one-to-many)
            builder.Entity<RIMSAuditTrail>()
                .HasOne(at => at.User)
                .WithMany(u => u.AuditTrails)
                .HasForeignKey(at => at.UserId)
                .HasConstraintName("FK_rimsAuditTrail_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            // AuditTrail -> UserAction (one-to-many)
            builder.Entity<RIMSAuditTrail>()
                .HasOne(at => at.UserAction)
                .WithMany(ua => ua.AuditTrails)
                .HasForeignKey(at => at.ActionId)
                .HasConstraintName("FK_rimsAuditTrail_ActionId")
                .OnDelete(DeleteBehavior.Restrict);

            // Assistance -> Resident (one-to-many)
            builder.Entity<RIMSAssistance>()
                .HasOne(a => a.Resident)
                .WithMany()
                .HasForeignKey(a => a.ResidentId)
                .OnDelete(DeleteBehavior.Cascade);

            // HouseholdMembers -> Resident (as member)
            builder.Entity<RIMSHouseholdMembers>()
                .HasOne(hm => hm.Resident)
                .WithMany(r => r.HouseholdMembers)
                .HasForeignKey(hm => hm.ResidentId)
                .HasConstraintName("FK_rimsHouseholdMembers_Resident")
                .OnDelete(DeleteBehavior.Restrict);

            // HouseholdMembers -> HeadOfHousehold
            builder.Entity<RIMSHouseholdMembers>()
                .HasOne(hm => hm.HeadOfHousehold)
                .WithMany()
                .HasForeignKey(hm => hm.HeadOfHouseholdId)
                .HasConstraintName("FK_rimsHouseholdMembers_HeadOfHousehold")
                .OnDelete(DeleteBehavior.Restrict);

            // Identity relationships
            builder.Entity<RIMSUserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .HasConstraintName("FK_rimsUserRoles_rimsUsers_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RIMSUserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .HasConstraintName("FK_rimsUserRoles_rimsRoles_RoleId")
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.Entity<RIMSResident>()
                .HasIndex(r => new { r.LastName, r.FirstName });

            builder.Entity<RIMSDocumentApplication>()
                .HasIndex(da => da.FK_ResidentId);

            builder.Entity<RIMSDocumentApplication>()
                .HasIndex(da => da.FK_DocumentId);

            builder.Entity<RIMSAuditTrail>()
                .HasIndex(at => at.UserId)
                .HasDatabaseName("IX_rimsAuditTrail_UserId");

            builder.Entity<RIMSAuditTrail>()
                .HasIndex(at => at.ActionDate);

            builder.Entity<RIMSAuditTrail>()
                .HasIndex(at => at.ActionType)
                .HasDatabaseName("IX_rimsAuditTrail_ActionType");

            builder.Entity<RIMSAddress>()
                .HasIndex(a => a.StreetId);

            builder.Entity<RIMSResidentCategory>()
                .HasIndex(rc => rc.ResidentId);

            // HouseholdMembers indexes
            builder.Entity<RIMSHouseholdMembers>()
                .HasIndex(hm => hm.ResidentId);

            builder.Entity<RIMSHouseholdMembers>()
                .HasIndex(hm => hm.HeadOfHouseholdId);

            builder.Entity<RIMSHouseholdMembers>()
                .HasIndex(hm => hm.IsHeadOfHousehold);

            // Default values
            builder.Entity<RIMSAuditTrail>()
                .Property(at => at.ActionDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<RIMSHouseholdMembers>()
                .Property(hm => hm.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }

        // Your existing SaveChangesAsync and audit trail methods remain the same
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(auditEntries, cancellationToken);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            // Your existing implementation
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is RIMSAuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Metadata.GetTableName() ?? string.Empty
                };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue!;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue!;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue!;
                                auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            }
                            break;
                    }
                }

                auditEntries.Add(auditEntry);
            }

            foreach (var auditEntry in auditEntries.Where(a => !a.HasTemporaryProperties))
            {
                rimsAuditTrail.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(a => a.HasTemporaryProperties).ToList();
        }

        private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries, CancellationToken cancellationToken = default)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue!;
                    else
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue!;
                }

                rimsAuditTrail.Add(auditEntry.ToAudit());
            }

            await base.SaveChangesAsync(cancellationToken);
        }
    }

    // Audit helper (keep your existing implementation)
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; } = string.Empty;
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public RIMSAuditTrail ToAudit()
        {
            return new RIMSAuditTrail
            {
                ActionDate = DateTime.UtcNow,
                ActionType = Entry.State.ToString(),
                ModuleName = Entry.Metadata.Name,
                TableName = TableName,
                KeyValues = JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
                ActionStatus = "Success"
            };
        }
    }
}