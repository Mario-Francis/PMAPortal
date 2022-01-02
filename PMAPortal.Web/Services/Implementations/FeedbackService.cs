using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class FeedbackService:IFeedbackService
    {
        private readonly IRepository<ApplicantFeedback> feedbackRepo;
        private readonly IRepository<FeedbackQuestion> feedbackQuestionRepo;

        public FeedbackService(IRepository<ApplicantFeedback> feedbackRepo,
            IRepository<FeedbackQuestion> feedbackQuestionRepo)
        {
            this.feedbackRepo = feedbackRepo;
            this.feedbackQuestionRepo = feedbackQuestionRepo;
        }

        public IEnumerable<FeedbackQuestion> GetQuestions()
        {
            return feedbackQuestionRepo.GetAll().OrderBy(q => q.Id);
        }

        public async Task AddFeedback(ApplicantFeedback feedback)
        {
            await feedbackRepo.Insert(feedback);
        }

        public IEnumerable<ApplicantFeedback> GetFeedbacks()
        {
            return feedbackRepo.GetAll().OrderByDescending(f => f.Id);
        }

        public async Task<ApplicantFeedback> GetFeedback(long id)
        {
            return await feedbackRepo.GetById(id);
        }
    }
}
