using Microsoft.EntityFrameworkCore;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options)
        {
            
        }
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<FeedbackAttachment>();
            base.OnModelCreating(modelBuilder);
        }
    }
}