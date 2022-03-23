using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IFeedbackService
    {
        IEnumerable<FeedbackQuestion> GetQuestions();
        Task AddFeedback(CustomerFeedback feedback);
        IEnumerable<CustomerFeedback> GetFeedbacks();
        Task<CustomerFeedback> GetFeedback(long id);
    }
}
