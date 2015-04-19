using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using DAL.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditV> AuditsV { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<HouseV> HousesV { get; set; }
        public DbSet<ZForm> Forms { get; set; }
        public DbSet<ZField> Fields { get; set; }
        public DbSet<ZValue> Values { get; set; }
        public DbSet<ZValueAll> ValuesAll { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FieldCategory> FieldCategories { get; set; }
        public DbSet<RuleOperation> RuleOperations { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<Dict> Dicts { get; set; }
        public DbSet<DictValue> DictsValues { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupHouse> GroupHouses { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<ReportParam> ReportParams { get; set; }
        public DbSet<DValue> DValues { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Message> Messages { get; set; }
        //public DbSet<MessageV> MessagesV { get; set; }
        public DbSet<RoleHouse> RolesHouses { get; set; }
        public DbSet<RoleForm> RolesForms { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<GenReport> GenReports { get; set; }
        public DbSet<PrivateKey> PrivateKeys { get; set; }
        public DbSet<ImpFile> ImpFiles { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<UnreadMessage> UnreadMessages { get; set; }
        public DbSet<ReportTemplate> ReportTemplates { get; set; }
        public DbSet<GroupRepTemplate> GroupRepTemplates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().ToTable("Sessions", "public");
            modelBuilder.Entity<User>().ToTable("Users","public");
            modelBuilder.Entity<Audit>().ToTable("Audit", "public");
            modelBuilder.Entity<AuditV>().ToTable("Audit_v", "public");
            modelBuilder.Entity<House>().ToTable("Houses", "public");
            modelBuilder.Entity<HouseV>().ToTable("Houses_v", "public");
            modelBuilder.Entity<ZForm>().ToTable("Forms", "public");
            modelBuilder.Entity<ZField>().ToTable("Fields", "public");
            modelBuilder.Entity<ZValue>().ToTable("Values", "public");
      //      modelBuilder.Entity<ZValue>().ToTable("ValuesAll_v", "public");
            modelBuilder.Entity<FieldCategory>().ToTable("FieldCategories", "public");
            modelBuilder.Entity<Category>().ToTable("Categories", "public");
            modelBuilder.Entity<RuleOperation>().ToTable("RuleOperations", "public");
            modelBuilder.Entity<Rule>().ToTable("Rules", "public");
            modelBuilder.Entity<Formula>().ToTable("Formulas", "public");
            modelBuilder.Entity<Dict>().ToTable("Dicts", "public");
            modelBuilder.Entity<DictValue>().ToTable("DictsValues", "public");
            modelBuilder.Entity<Role>().ToTable("Roles", "public");
            modelBuilder.Entity<Group>().ToTable("Groups", "public");
            modelBuilder.Entity<GroupHouse>().ToTable("GroupHouses", "public");
            modelBuilder.Entity<Key>().ToTable("Keys", "public");
            modelBuilder.Entity<ReportParam>().ToTable("ReportParams", "public");
            modelBuilder.Entity<DValue>().ToTable("Values_v", "public");
            modelBuilder.Entity<ZValueAll>().ToTable("ValuesAll_v", "public");
            modelBuilder.Entity<Attachment>().ToTable("Attachments", "public");
            modelBuilder.Entity<Message>().ToTable("Messages", "public");
           // modelBuilder.Entity<MessageV>().ToTable("Messages_v", "public");
            modelBuilder.Entity<RoleHouse>().ToTable("RolesHouses", "public");
            modelBuilder.Entity<RoleForm>().ToTable("RolesForms", "public");
            modelBuilder.Entity<Right>().ToTable("Rights", "public");
            modelBuilder.Entity<GenReport>().ToTable("GenReports", "public");
            modelBuilder.Entity<PrivateKey>().ToTable("PrivateKeys", "public");
            modelBuilder.Entity<ImpFile>().ToTable("ImpFiles", "public");
            modelBuilder.Entity<Result>().ToTable("Results", "public");
            modelBuilder.Entity<Settings>().ToTable("Settings", "public");
            modelBuilder.Entity<UnreadMessage>().ToTable("UnreadMessages", "public");
            modelBuilder.Entity<ReportTemplate>().ToTable("ReportTemplates", "public");
            modelBuilder.Entity<GroupRepTemplate>().ToTable("GroupRepTemplates", "public");

      
            // // Chinook Database for PostgreSQL doesn't auto-increment Ids
            // modelBuilder.Conventions
            // .Delete<StoreGeneratedIdentityKeyConvention>();
        }
    }
}
