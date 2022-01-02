using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class FeedbackVM
    {
        public long Id { get; set; }
        [Required]
        public long? ApplicantId { get; set; }
        [Required]
        public long? ApplicationId { get; set; }
        [Required]
        public long? Question1Id { get; set; }
        [Required]
        public int? Answer1 { get; set; }
        [Required]
        public long? Question2Id { get; set; }
        [Required]
        public int? Answer2 { get; set; }
        [Required]
        public long? Question3Id { get; set; }
        [Required]
        public int? Answer3 { get; set; }
        [Required]
        public long? Question4Id { get; set; }
        [Required]
        public int? Answer4 { get; set; }
        [Required]
        public long? Question5Id { get; set; }
        [Required]
        public int? Answer5 { get; set; }

        public string Comment { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public string Rating { get; set; }
        public string Applicant { get; set; }
        public string TrackNumber { get; set; }

        public string FormattedCreatedDate
        {
            get
            {
                return CreatedDate.ToString("MMM d, yyyy 'at' hh:mmtt");
            }
        }

        public ApplicantFeedback ToApplicantFeedback()
        {
            return new ApplicantFeedback
            {
                Id = Id,
                ApplicantId = ApplicantId.Value,
                ApplicationId = ApplicationId.Value,
                Rating = Math.Round((decimal)((Answer1 + Answer2 + Answer3 + Answer4 + Answer5) / 5), MidpointRounding.AwayFromZero).ToString("N"),
                Comment = Comment,
                CreatedDate = DateTimeOffset.Now,
                FeedbackAnswers = ToFeedbackAnswers()
            };
        }

        public List<FeedbackAnswer> ToFeedbackAnswers()
        {
            return new List<FeedbackAnswer>
            {
                new FeedbackAnswer{CreatedDate=DateTimeOffset.Now, FeedbackQuestionId=Question1Id.Value, rating=Answer1.Value},
                new FeedbackAnswer{CreatedDate=DateTimeOffset.Now, FeedbackQuestionId=Question2Id.Value, rating=Answer2.Value},
                new FeedbackAnswer{CreatedDate=DateTimeOffset.Now, FeedbackQuestionId=Question3Id.Value, rating=Answer3.Value},
                new FeedbackAnswer{CreatedDate=DateTimeOffset.Now, FeedbackQuestionId=Question4Id.Value, rating=Answer4.Value},
                new FeedbackAnswer{CreatedDate=DateTimeOffset.Now, FeedbackQuestionId=Question5Id.Value, rating=Answer5.Value}
            };
        }

        public static FeedbackVM FromApplicantFeedback(ApplicantFeedback feedback, int? clientTimeOffset = null)
        {
            var applicant = feedback.Application.Applicant;
            return new FeedbackVM
            {
                Id = feedback.Id,
                Applicant = $"{applicant.FirstName} {applicant.LastName} ({applicant.PhoneNumber})",
                Rating = feedback.Rating,
                Comment = feedback.Comment ?? "",
                TrackNumber=feedback.Application.TrackNumber,
                CreatedDate = clientTimeOffset == null ? feedback.CreatedDate : feedback.CreatedDate.ToOffset(TimeSpan.FromMinutes(clientTimeOffset.Value)),
            };
        }
    }
}
