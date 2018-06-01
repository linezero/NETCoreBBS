using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBBS.Entities;

namespace NetCoreBBS.Infrastructure
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicReply> TopicReplys { get; set; }
        public DbSet<TopicNode> TopicNodes { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserCollection> UserTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Topic>().ToTable("Topic");
            modelBuilder.Entity<TopicReply>().ToTable("TopicReply");
            modelBuilder.Entity<TopicNode>().ToTable("TopicNode");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserMessage>().ToTable("UserMessage");
            modelBuilder.Entity<UserCollection>().ToTable("UserCollection");
        }
    }
}
