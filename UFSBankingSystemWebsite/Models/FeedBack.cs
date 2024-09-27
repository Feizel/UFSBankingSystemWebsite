using System;

namespace UFSBankingSystem.Models
{
    public class FeedBack
    {
        public int FeedBackID { get; set; } // Unique identifier for the feedback
        public string UserEmail { get; set; } // Email of the user providing the feedback
        public string Message { get; set; } // Feedback message
        public int Rating { get; set; } // Rating given by the user (e.g., 1-5)
        public DateTime FeedbackDate { get; set; } // Date when the feedback was provided
    }
}