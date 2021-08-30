using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Darewise.Feedback.Controllers;
using Darewise.Feedback.Mapping;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NFluent;
using Xunit;

namespace Darewise.Feedback.Tests
{
    public class FeedbackRepositoryTest
    {
        private static Guid _userId = Guid.Parse("8DD0E473-74F2-4F1C-B215-64EA0A6BB61E");
        
        [Fact(DisplayName = "EFCoreRepository_Test_Save_Add_OK")]
        public async Task Mapper_Test_Single_OK()
        {
            var services = new ServiceCollection();

            // I would have preferred InMemoryDatabase but EFCore.InMemory does not support SQL & NoSQL inline
            services.AddDbContext<FeedbackDbContext>(options =>
            {
                options.UseNpgsql($"Host=localhost;Database=feedback-api;Username=postgres;Password=strong_pass2018");
            });
            services.AddDbContextFactory<FeedbackDbContext>(options =>
            {
                options.UseNpgsql($"Host=localhost;Database=feedback-api;Username=postgres;Password=strong_pass2018");
            });
            services.AddLogging();
            services.AddScoped<IFeedbackRepository, EfCoreFeedbackRepository>();

            ServiceProvider? serviceProvider = services.BuildServiceProvider();
            var repository = serviceProvider.GetRequiredService<IFeedbackRepository>();
            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<FeedbackDbContext>>();
            await using FeedbackDbContext? context = dbContextFactory.CreateDbContext();
            await context.Database.EnsureCreatedAsync();

            Check.That(repository).IsInstanceOf<EfCoreFeedbackRepository>();

            var feedback = new FeedbackEntity()
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                Categories = new HashSet<string> {"FEEDBACK"},
                Date = DateTime.UtcNow,
                Message = "hello world",
            };
            FeedbackEntity? newFeedback = await repository.SaveAsync(feedback);

            Check.That(newFeedback.Id).IsEqualTo(feedback.Id);
        }

    }
}