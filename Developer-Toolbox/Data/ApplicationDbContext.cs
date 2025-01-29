﻿using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<BadgeTag> BadgeTags { get; set; }
        public DbSet<WeeklyChallengeExercise> WeeklyChallengeExercises { get; set; }
        public DbSet<WeeklyChallenge> WeeklyChallenges { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BadgeChallenge> BadgeChallenges { get; set; }

        public DbSet<LockedExercise> LockedExercises { get; set; }
        public DbSet<LearningPath> LearningPaths { get; set; }

        public DbSet<LockedSolution> LockedSolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<QuestionTag>()
            .HasKey(c => new
            {
                c.Id,
                c.QuestionId,
                c.TagId
            });

            // definire relatii cu modelele Question si Tag (FK)
            modelBuilder.Entity<QuestionTag>()
            .HasOne(c => c.Tag)
            .WithMany(c => c.QuestionTags)
            .HasForeignKey(c => c.TagId);
            modelBuilder.Entity<QuestionTag>()
            .HasOne(c => c.Question)
            .WithMany(c => c.QuestionTags)
            .HasForeignKey(c => c.QuestionId);

            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<Bookmark>()
            .HasKey(c => new
            {
                c.Id,
                c.UserId,
                c.QuestionId
            });

            // definire relatii cu modelele User si Question (FK)
            modelBuilder.Entity<Bookmark>()
            .HasOne(c => c.Question)
            .WithMany(c => c.Bookmarks)
            .HasForeignKey(c => c.QuestionId);
            modelBuilder.Entity<Bookmark>()
            .HasOne(c => c.User)
            .WithMany(c => c.Bookmarks)
            .HasForeignKey(c => c.UserId);

            ///

            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity< Reaction>()
            .HasKey(c => new
            {
                c.Id,
                c.UserId,
                c.QuestionId
            });

            // definire relatii cu modelele User si Question (FK)
            modelBuilder.Entity<Reaction>()
            .HasOne(c => c.Question)
            .WithMany(c => c.Reactions)
            .HasForeignKey(c => c.QuestionId);
            modelBuilder.Entity<Reaction>()
            .HasOne(c => c.User)
            .WithMany(c => c.Reactions)
            .HasForeignKey(c => c.UserId);

            // definire primary key compus
            modelBuilder.Entity<UserBadge>()
            .HasKey(ub => new { ub.UserId, ub.BadgeId });
            // definire relatii cu modelele ApplicationUser si Badge (FK)
            modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.User)
            .WithMany(ub => ub.UserBadges)
            .HasForeignKey(ub => ub.UserId);
            modelBuilder.Entity<UserBadge>()
            .HasOne(ub => ub.Badge)
            .WithMany(ub => ub.UserBadges)
            .HasForeignKey(ub => ub.BadgeId);

            // definire primary key compus
            modelBuilder.Entity<BadgeTag>()
            .HasKey(bt => new { bt.BadgeId, bt.TagId });
            // definire relatii cu modelele Badge si Tag (FK)
            modelBuilder.Entity<BadgeTag>()
            .HasOne(bt => bt.Tag)
            .WithMany(bt => bt.BadgeTags)
            .HasForeignKey(bt => bt.TagId);
            modelBuilder.Entity<BadgeTag>()
            .HasOne(bt => bt.Badge)
            .WithMany(bt => bt.BadgeTags)
            .HasForeignKey(bt => bt.BadgeId);


            // Configurarea relației Many-to-Many
            modelBuilder.Entity<WeeklyChallengeExercise>()
            .HasKey(wce => new { wce.WeeklyChallengeId, wce.ExerciseId });

            modelBuilder.Entity<WeeklyChallengeExercise>()
            .HasOne(wce => wce.WeeklyChallenge)
            .WithMany(wce => wce.WeeklyChallengeExercises)
            .HasForeignKey(wce => wce.WeeklyChallengeId);

            modelBuilder.Entity<WeeklyChallengeExercise>()
            .HasOne(wce => wce.Exercise)
            .WithMany(wce => wce.WeeklyChallengeExercises)
            .HasForeignKey(wce => wce.ExerciseId);

           
             modelBuilder.Entity<BadgeChallenge>()
             .HasKey(bc => new { bc.BadgeId, bc.WeeklyChallengeId });   
             modelBuilder.Entity<BadgeChallenge>()
             .HasOne(bc => bc.WeeklyChallenge)
             .WithMany(bc => bc.BadgeChallenges)
             .HasForeignKey(bc => bc.WeeklyChallengeId);
             modelBuilder.Entity<BadgeChallenge>()
             .HasOne(bc => bc.Badge)
             .WithMany(bc => bc.BadgeChallenges)
             .HasForeignKey(bc => bc.BadgeId);

            modelBuilder.Entity<ApplicationUser>()
            .Property(au => au.ReputationPoints)
            .HasDefaultValueSql("0");

        }
    }
}