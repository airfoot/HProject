using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using HProject.Domain;
using Microsoft.AspNet.Identity;

namespace HProject.Infrastructure
{
    public class HProjectDbContext : IdentityDbContext<User>
    {
        public HProjectDbContext():base("MSSQLServer")
        {

        }

        static HProjectDbContext()
        {
            Database.SetInitializer<HProjectDbContext>(new IdentityDbInit());
        }

        public static HProjectDbContext Create()
        {
            return new HProjectDbContext();
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<ITArticleApplication> ITArticleApplications { get; set; }
        public virtual DbSet<ITArticleApplicationItem> ITArticleApplicationItems { get; set; }
        public virtual DbSet<ITArticle> ITArticles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasOptional(d => d.DepartmentManager)
                .WithMany();
            modelBuilder.Entity<User>()
                .HasRequired(u => u.UserDepartment)
                .WithMany();

            modelBuilder.Entity<ITArticleApplicationItem>()
                .HasRequired(i => i.ITArticleItem)
                .WithMany();
               

            modelBuilder.Entity<ITArticleApplication>()
                .HasRequired(a => a.Applicant)
                .WithMany();
            modelBuilder.Entity<ITArticleApplication>()
                .HasMany(a => a.ITArticleItemList)
                .WithRequired()
                .WillCascadeOnDelete();
        }


    }
    
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<HProjectDbContext>
    {
        protected override void Seed(HProjectDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(HProjectDbContext context)
        {
            UserManagers userMgr = new UserManagers(new UserStore<User>(context));
            RoleManagers roleMgr = new RoleManagers(new RoleStore<Role>(context));

            Department department1 = new Department();
            department1.Id = Guid.NewGuid();
            department1.Name = "IT";

            Department department2 = new Department();
            department2.Id = Guid.NewGuid();
            department2.Name = "HR";

            Department department3 = new Department();
            department3.Id = Guid.NewGuid();
            department3.Name = "Finance";

            Department department4 = new Department();
            department4.Id = Guid.NewGuid();
            department4.Name = "Admin";

            Department department5 = new Department();
            department5.Id = Guid.NewGuid();
            department5.Name = "Production";

            Department department6 = new Department();
            department6.Id = Guid.NewGuid();
            department6.Name = "Sales";

            context.SaveChanges();

            string roleName = "Admin";
            string ITRole = "IT";

            string userName = "admin";
            string password = "123456";
            string email = "admin@admin.com";
            string fullName = "testadmin fullname";


            string userName1 = "test1";
            string password1 = "123456";
            string email1 = "test1@user.com";
            string fullName1 = "test1 DisplayName";

            string userName2 = "test2";
            string password2 = "123456";
            string email2 = "test2user@user.com";
            string fullName2 = "test2user fullname";

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new Role(roleName));
            }
            if (!roleMgr.RoleExists(ITRole))
            {
                roleMgr.Create(new Role(ITRole));
            }

            User user = userMgr.FindByName(userName);
            if (user == null)
            {
                User localUser = new User();
                localUser.UserName = userName;
                localUser.Email = email;
                localUser.FullName = fullName;
                localUser.UserDepartment = department1;

                userMgr.Create(localUser, password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }
            if (!userMgr.IsInRole(user.Id, ITRole))
            {
                userMgr.AddToRole(user.Id, ITRole);
            }

            User user1 = userMgr.FindByName(userName1);
            if (user1 == null)
            {
                User localUser = new User();
                localUser.UserName = userName1;
                localUser.Email = email1;
                localUser.FullName = fullName1;
                localUser.UserDepartment = department2;

                userMgr.Create(localUser, password1);
                user1 = userMgr.FindByName(userName1);
            }

            //if (!userMgr.IsInRole(user1.Id, ITRole))
            //{
            //    userMgr.AddToRole(user1.Id, ITRole);
            //}

            User user2 = userMgr.FindByName(userName2);
            if (user2 == null)
            {
                User localUser = new User();
                localUser.UserName = userName2;
                localUser.Email = email2;
                localUser.FullName = fullName2;
                localUser.UserDepartment = department3;

                userMgr.Create(localUser, password2);

            }

          
            context.SaveChanges();

        }
    } 
        

}