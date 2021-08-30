using System.Collections.Generic;
using System.Linq;
using Darewise.Feedback.Controllers;
using Darewise.Feedback.Mapping;
using FizzWare.NBuilder;
using NFluent;
using Xunit;

namespace Darewise.Feedback.Tests;

public class MapperTest
{
    [Fact(DisplayName = "Mapper_Test_Single_OK")]
    public void Mapper_Test_Single_OK()
    {
        var mapper = new GenericMapsterMapper<FeedbackEntity, FeedbacksModel>();
        FeedbackEntity? entity = Builder<FeedbackEntity>.CreateNew().Build();
        FeedbacksModel? model = mapper.Map(entity);
        Check.That(model.Id).IsEqualTo(entity.Id);

    }
    [Fact(DisplayName = "Mapper_Test_Multiple_OK")]
    public void Mapper_Test_Multiple_OK()
    {
        var mapper = new GenericMapsterMapper<FeedbackEntity, FeedbacksModel>();
        IList<FeedbackEntity> entity = Builder<FeedbackEntity>.CreateListOfSize(2).Build();
        IEnumerable<FeedbacksModel> model = mapper.Map(entity);
        Check.That(model.Count()).IsEqualTo(entity.Count);
        Check.That(model).ContainsNoNull();
        Check.That(model).HasElementThatMatches(s => s.Id == entity.First().Id);
    }
}