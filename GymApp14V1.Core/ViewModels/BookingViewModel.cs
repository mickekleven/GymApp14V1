namespace GymApp14V1.Core.ViewModels
{
    public enum MemberAction { NONE, Add, Update, Delete, UserMessage }

    public class BookingViewModel
    {
#nullable disable

        public int Id { get; set; }

        public MemberViewModel Member { get; set; }
        public GymClassViewModel GymClass { get; set; }
        public IEnumerable<GymClassViewModel> GymClasses { get; set; }
        public MemberAction MemberAction { get; set; }

        public int GymClassId { get; set; }

        public string UserMessage { get; set; }

    }
}
