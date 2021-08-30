using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Darewise.Feedback.Mapping;
using Darewise.Feedback.Utils;
using Darewise.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Darewise.Feedback.Controllers
{
    [ApiController]
    [Route("/feedback/user")]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly AttachmentUploadOptions _uploadOptions;
        private readonly ILogger<FeedbackController> _logger;
        private readonly IFeedbackRepository _repository;
        private readonly IFeedbackBlobStorage _blobStorage;
        private readonly IMessagePublisher<FeedbackCreated> _publisher;
        private readonly IMapper<FeedbackEntity, FeedbacksModel> _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public FeedbackController(ILogger<FeedbackController> logger, IFeedbackRepository repository, IMapper<FeedbackEntity, FeedbacksModel> mapper, IHttpContextAccessor contextAccessor,
            IFeedbackBlobStorage blobStorage, IMessagePublisher<FeedbackCreated> publisher, AttachmentUploadOptions uploadOptions)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _blobStorage = blobStorage;
            _publisher = publisher;
            _uploadOptions = uploadOptions;
        }

        [HttpGet]
        public async Task<IEnumerable<FeedbacksModel>> Get()
        {
            Guid userId = _contextAccessor.GetUserId();
            // get feedbacks
            IEnumerable<FeedbackEntity> tmp = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map(tmp);
        }

        /// <summary>
        /// Returning the feedback model is not necessary, it could be exploited if people found how to use the API as a free storage
        /// </summary>
        /// <param name="form"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost]
        public async Task Create([FromForm] FeedbackCreateForm form, [FromForm(Name = "attachments")] List<IFormFile> attachments)
        {
            Guid userId = _contextAccessor.GetUserId();
            var feedback = new FeedbackEntity()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                UserId = userId,
                Title = form.Title,
                Message = form.Message,
                Categories = form.Categories ?? new HashSet<string>() { "FEEDBACK" },
            };

            if (attachments.Any())
            {
                feedback.Attachments = new List<FeedbackAttachment>();
                
                // limit per file
                if (attachments.Any(s => s.Length > _uploadOptions.LimitPerFile))
                {
                    throw new BadHttpRequestException("File exceeds the set limit (10MB per file max)", 400);
                }

                // total file upload
                if (attachments.Sum(s => s.Length) > _uploadOptions.LimitTotal)
                {
                    throw new BadHttpRequestException("File exceeds the set limit (50MB total)", 400);
                }

                // Attachments count
                if (attachments.Count > _uploadOptions.LimitFilesCount)
                {
                    throw new BadHttpRequestException("File exceeds the set limit (10 attachments total)", 400);
                }

                foreach (var attachment in attachments)
                {
                    // add some security checks for MIME type & file headers etc...
                    // Given that it's just raw byte stream redirection, nothing should be executed on the container

                    await using Stream? readStream = attachment.OpenReadStream();

                    var blob = new FeedbackBlob { Data = readStream, Filename = attachment.FileName, ContentType = attachment.ContentType };

                    FeedbackAttachment storageInfo = await _blobStorage.SaveFeedback(feedback.Id, blob);
                    feedback.Attachments.Add(storageInfo);
                }
            }

            feedback = await _repository.SaveAsync(feedback);

            _logger.LogInformation(
                $"Feedback successfully created - FeedbackID: {feedback.Id.ToString()} | UserId: {feedback.UserId.ToString()} | Categories: {string.Join(' ', feedback.Categories)}");

            await _publisher.PublishAsync(new FeedbackCreated()
            {
                Id = feedback.Id,
                UserId = feedback.UserId,
                CreatedAt = feedback.Date,
            });
        }
    }
}