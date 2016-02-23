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
        }

        [Display(Name="Assigned user")]
        public string userName { get; set; }
        [Required]
        [Display(Name="Routine Name")]
        public string routineTitle { get; set; }
        [Required]
        [Display(Name="Timed?")]
        public bool isTimed { get; set; }
        [Display(Name="Expected Duration")]
        public System.TimeSpan expectedDuration { get; set; }
        [Display(Name="Notify me on completion?")]
        public bool isNotifiable { get; set; }
        [Display(Name="Title for Positive Feedback")]
        public string positiveFeedbackTitle { get; set; }
        [Display(Name="Message for Positive Feedback")]
        public string positiveFeedbackMessage { get; set; }
        [Display(Name="Title for Negative Feedback")]
        public string negativeFeedbackTitle { get; set; }
        [Display(Name="Message for Negative Feedback")]
        public string negativeFeedbackMessage { get; set; }
        [Display(Name="Message type for Negative Feedback")]
        public virtual MessageTypeModel MessageType { get; set; }
        [Display(Name="Message type for Positive Feedback")]
        public virtual MessageTypeModel MessageType1 { get; set; }
        [Required]
        [Display(Name="Tasks")]
        public virtual ICollection<TaskModel> Tasks { get; set; }
    }

    public class TaskModel
    {
        [Required]
        [Display(Name="Sequence number")]
        public byte sequenceNo { get; set; }
        [Required]
        [Display(Name="Task name")]
        public string taskNam { get; set; }
        [Required]
        [Display(Name="Task description")]
        public string taskDescription { get; set; }
        [Required]
        [Display(Name="Timed?")]
        public bool isTimed { get; set; }
        [Display(Name="Expected duration")]
        public System.TimeSpan expectedDuration { get; set; }
        [Display(Name="Prompt title")]
        public string promptTitle { get; set; }
        [Display(Name="Prompt message")]
        public string promptMessage { get; set; }
        [Display(Name="Title for Positive Feedback")]
        public string positiveFeedbackTitle { get; set; }
        [Display(Name="Message for Positive Feedback")]
        public string positiveFeedbackMessage { get; set; }
        [Display(Name="Title for Negative Feedback")]
        public string negativeFeedbackTitle { get; set; }
        [Display(Name="Message for Negative Feedback")]
        public string negativeFeedbackMessage { get; set; }
        [Display(Name="Message type for Negative Feedback")]
        public virtual MessageTypeModel MessageType { get; set; }
        [Display(Name="Message type for Positive Feedback")]
        public virtual MessageTypeModel MessageType1 { get; set; }
        [Display(Name="Message type for Prompt")]
        public virtual MessageTypeModel MessageType2 { get; set; }
        [Required]
        [Display(Name="Category of Task")]
        public virtual TaskCategoryModel TaskCategory { get; set; }
    }

    public class TaskCategoryModel
    {
        [Required]
        [Display(Name = "Task category")]
        public string categoryName { get; set; }
    }
    
    public class MessageTypeModel
    {
        [Required]
        [Display(Name = "Message type")]
        public string messageTypeName { get; set; }
    }
}