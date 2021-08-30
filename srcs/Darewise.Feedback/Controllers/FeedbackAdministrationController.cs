using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Darewise.Feedback.Mapping;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<IEnumerable<FeedbacksModel>> Get([FromQuery] FeedbackGetPaginatedForm query)
        {
            IEnumerable<FeedbackEntity>? feedbacks = await _repository.GetAsync(query.Limit ?? 50, query.Offset ?? 0);
            return _mapper.Map(feedbacks);
        }

        [HttpGet("{userId:Guid}", Name = "GetByUserId")]
        public async Task<IEnumerable<FeedbacksModel>> GetByUserId(Guid userId, [FromQuery] FeedbackGetPaginatedForm query)
        {
            IEnumerable<FeedbackEntity>? feedbacks = await _repository.GetByUserIdAsync(userId, query.Limit ?? 50, query.Offset ?? 0);
            return _mapper.Map(feedbacks);
        }
    }
}