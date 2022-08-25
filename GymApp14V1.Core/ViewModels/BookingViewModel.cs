using System.ComponentModel.DataAnnotations;

namespace GymApp14V1.Core.ViewModels
{
    public enum MemberAction { NONE, Add, Update, Delete, UserMessage }

    public class BookingViewModel
    {
#nullable disable

        public int Id { get; set; }

        [Required]
        [Display(Name = "Gym class name")]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration;
        public string Description { get; set; }


        public bool IsAttending { get; set; }

        public MemberViewModel Member { get; set; }
        public GymClassViewModel GymClass { get; set; }
        public IEnumerable<GymClassViewModel> GymClasses { get; set; }
        public MemberAction MemberAction { get; set; }

        public int GymClassId { get; set; }

        public string UserMessage { get; set; }

        public PageHeaderViewModel PageHeader { get; set; }

    }
}
