using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PJCAdmin.Models
{
    public class RoutineModel
    {
        //TODO add string length requirements
        public RoutineModel()
        {
            this.Tasks = new HashSet<TaskModel>();
            this.Feedbacks = new HashSet<FeedbackModel>();
        }

        [Display(Name="Assigned user")]
        public string assigneeUserName { get; set; }
        [Required]
        [Display(Name="Routine Name")]
        public string routineTitle { get; set; }
        [Required]
        [Display(Name="Timed?")]
        public bool isTimed { get; set; }
        [Display(Name="Expected Duration")]
        public Nullable<System.TimeSpan> expectedDuration { get; set; }
        [Required]
        [Display(Name="Notify me on completion?")]
        public bool isNotifiable { get; set; }
        [Display(Name="Disabled?")]
        public bool isDisabled { get; set; }
        [Required]
        [Display(Name="Tasks")]
        public virtual ICollection<TaskModel> Tasks { get; set; }
        [Display(Name = "Routine Feedback")]
        public virtual ICollection<FeedbackModel> Feedbacks { get; set; }
    }

    public class TaskModel
    {
        public TaskModel()
        {
            this.Feedbacks = new HashSet<FeedbackModel>();
        }
        [Required]
        [Display(Name="Sequence number")]
        public byte sequenceNo { get; set; }
        [Required]
        [Display(Name="Task name")]
        public string taskName { get; set; }
        [Required]
        [Display(Name="Task description")]
        public string taskDescription { get; set; }
        [Required]
        [Display(Name="Timed?")]
        public bool isTimed { get; set; }
        [Display(Name="Expected duration")]
        public Nullable<System.TimeSpan> expectedDuration { get; set; }
        [Required]
        [Display(Name="Category of Task")]
        public virtual TaskCategoryModel TaskCategory { get; set; }
        [Display(Name = "Task Feedback")]
        public virtual ICollection<FeedbackModel> Feedbacks { get; set; }
    }

    public class TaskCategoryModel
    {
        [Required]
        [Display(Name = "Task category")]
        public string categoryName { get; set; }
    }

    public class FeedbackModel
    {
        [Required]
        [Display(Name="Title")]
        public string feedbackTitle { get; set; }
        [Required]
        [Display(Name="Content")]
        public string feedbackMessage { get; set; }
        [Required]
        [Display(Name="Feedback Media")]
        public virtual MediaTypeModel MediaType { get; set; }
        [Required]
        [Display(Name = "Feedback Type")]
        public virtual FeedbackTypeModel FeedbackType { get; set; }
    }

    public class MediaTypeModel
    {
        [Required]
        [Display(Name="Type")]
        public string mediaTypeName { get; set; }
    }

    public class FeedbackTypeModel
    {
        [Required]
        [Display(Name = "Type")]
        public string feedbackTypeName { get; set; }
    }
}