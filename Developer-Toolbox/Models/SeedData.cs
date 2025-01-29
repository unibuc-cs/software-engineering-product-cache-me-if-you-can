﻿using Developer_Toolbox.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Models
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<ApplicationDbContext>>()))
            {
                // Verificam daca in baza de date exista cel putin un rol
                // insemnand ca a fost rulat codul 
                // De aceea facem return pentru a nu insera rolurile inca o data
                // Acesta metoda trebuie sa se execute o singura data 
                if (!context.Roles.Any())
                {
                    // CREAREA ROLURILOR IN BD
                    // daca nu contine roluri, acestea se vor crea
                    context.Roles.AddRange(
                        new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "Admin".ToUpper() },
                        new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
                        new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "User", NormalizedName = "User".ToUpper() }
                    );

                    // o noua instanta pe care o vom utiliza pentru crearea parolelor utilizatorilor
                    // parolele sunt de tip hash
                    var hasher = new PasswordHasher<ApplicationUser>();

                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb0", // primary key
                        UserName = "admin",
                        EmailConfirmed = true,
                        NormalizedEmail = "ADMIN@TEST.COM",
                        Email = "admin@test.com",
                        NormalizedUserName = "ADMIN",
                        PasswordHash = hasher.HashPassword(null, "Admin1!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb1", // primary key
                        UserName = "moderator",
                        EmailConfirmed = true,
                        NormalizedEmail = "MODERATOR@TEST.COM",
                        Email = "moderator@test.com",
                        NormalizedUserName = "MODERATOR",
                        PasswordHash = hasher.HashPassword(null, "Moderator1!")
                    },
                    new ApplicationUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb2", // primary key
                        UserName = "user",
                        EmailConfirmed = true,
                        NormalizedEmail = "USER@TEST.COM",
                        Email = "user@test.com",
                        NormalizedUserName = "USER",
                        PasswordHash = hasher.HashPassword(null, "User1!")
                    }
                );

                    // ASOCIEREA USER-ROLE
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string>
                        {
                            RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                        },
                        new IdentityUserRole<string>
                        {
                            RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                        },
                        new IdentityUserRole<string>
                        {
                            RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                            UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
                        }
                    );
                }

                context.SaveChanges();

                // ADAUGAREA ACTIVITATILOR
                if (!context.Activities.Any())
                {
                    context.Activities.AddRange(
                    new Activity { Id = 1, Description = "Post question", ReputationPoints = 1, isPracticeRelated = false },
                    new Activity { Id = 2, Description = "Post answer", ReputationPoints = 3, isPracticeRelated = false },
                    new Activity { Id = 3, Description = "Be upvoted", ReputationPoints = 1, isPracticeRelated = false },
                    new Activity { Id = 4, Description = "Solve exercise", ReputationPoints = 5, isPracticeRelated = true },
                    new Activity { Id = 5, Description = "Complete challenge", ReputationPoints = 10, isPracticeRelated = true },
                    new Activity { Id = 6, Description = "Add exercise", ReputationPoints = 10, isPracticeRelated = null },
                    new Activity { Id = 7, Description = "Add challenge", ReputationPoints = 15, isPracticeRelated = null }
                    );
                }
                context.Database.OpenConnection();

                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Activities ON");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Activities OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }

            }
        }
    }

}

