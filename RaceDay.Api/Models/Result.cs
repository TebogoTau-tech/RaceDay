using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        public TimeSpan RaceTime { get; set; }

        public int OverallPosition { get; set; }

        // Foreign Key linked to Enrolment
        public int EnrolmentId { get; set; }
        public Enrolment? Enrolment { get; set; }
    }
}