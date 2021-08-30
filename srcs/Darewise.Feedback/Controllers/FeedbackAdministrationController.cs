using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Darewise.Feedback.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darewise.Feedback.Controllers
{
    [ApiController]
    [Route("/feedbacks")]
    [Authorize(Roles = "feedback-admin")]
    public class FeedbackAdministrationController : ControllerBase
    {
        private readonly ILogger<FeedbackAdministrationController> _logger;
        private readonly IFeedbackRepository _repository;
        private readonly IMapper<FeedbackEntity, FeedbacksModel> _mapper;

        public FeedbackAdministrationController(ILogger<FeedbackAdministrationController> logger, IFeedbackRepository repository, IMapper<FeedbackEntity, FeedbacksModel> mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetFeedbacks")]
        public async Task<IEnumerable<FeedbacksModel>> Get([FromQuery] FeedbackGetPaginatedForm query)
        {
            if (query.Limit < 0)
            {
                throw new BadHttpRequestException("Limit should have positive values only");
            }

            if (query.Offset < 0)
            {
                throw new BadHttpRequestException("Offset should have positive values only");
            }

            IEnumerable<FeedbackEntity>? feedbacks = await _repository.GetAsync(query.Limit ?? 50, query.Offset ?? 0);
            return _mapper.Map(feedbacks);
        }

        [HttpGet("{userId:Guid}", Name = "GetByUserId")]
        public async Task<IEnumerable<FeedbacksModel>> GetByUserId(Guid userId, [FromQuery] FeedbackGetPaginatedForm query)
        {
            if (query.Limit < 0)
            {
                throw new BadHttpRequestException("Limit should have positive values only");
            }

            if (query.Offset < 0)
            {
                throw new BadHttpRequestException("Offset should have positive values only");
            }

            IEnumerable<FeedbackEntity>? feedbacks = await _repository.GetByUserIdAsync(userId, query.Limit ?? 50, query.Offset ?? 0);
            return _mapper.Map(feedbacks);
        }

        [HttpPost(Name = "GetFeedbacksFiltered")]
        public async Task<IEnumerable<FeedbacksModel>> GetFeedbacksFiltered([FromForm] FeedbackGetSearchForm query)
        {
            if (query.Limit < 0)
            {
                throw new BadHttpRequestException("Limit should have positive values only");
            }

            if (query.Offset < 0)
            {
                throw new BadHttpRequestException("Offset should have positive values only");
            }

            IEnumerable<FeedbackEntity>? feedbacks = await _repository.GetAsync(query.Limit ?? 50, query.Offset ?? 0, query.UserId, query.Message, query.Category, query.From, query.To);
            return _mapper.Map(feedbacks);
        }
    }
}