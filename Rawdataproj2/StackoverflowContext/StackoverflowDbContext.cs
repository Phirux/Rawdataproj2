﻿using DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackoverflowContext
{
    class StackoverflowDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Answer> Answers { get; set; } 
        public DbSet<Question> Questions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Note> Notes { get; set; }

        public DbSet<Link> Links { get; set; }
        public DbSet<Search> Searches { get; set; }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("server=wt-220.ruc.dk;database=raw2;uid=aip;pwd=aip_325");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Search>().ToTable("Search_history");

            //inheritance
            modelBuilder.Entity<Post>()
                .HasDiscriminator<int>("PostType")
                .HasValue<Question>(1)
                .HasValue<Answer>(2);
             
            //many to one 
            modelBuilder.Entity<Question>().HasMany(o => o.Answers).WithOne()
                .HasForeignKey(d => d.ParentID); 

            //properties
            modelBuilder.Entity<Search>().Property(x => x.ID).HasColumnName("userid"); 
            modelBuilder.Entity<PostTag>().Property(x => x.ID).HasColumnName("postid");
            modelBuilder.Entity<Tag>().Property(x => x.ID).HasColumnName("postid"); 
            modelBuilder.Entity<Link>().Property(x => x.ID).HasColumnName("postid");
            modelBuilder.Entity<Comment>().Property(x => x.ID).HasColumnName("postid");

            modelBuilder.Entity<Search>().Property(x => x.Text).HasColumnName("SearchText");



            //many-to-many
            modelBuilder.Entity<Note>().HasKey(x => new { x.UserID, x.PostID });
            modelBuilder.Entity<Comment>().HasKey(x => new { x.UserID, x.PostID });
            modelBuilder.Entity<Bookmark>().HasKey(x => new { x.UserID, x.PostID });
               

        }
    }
}
