namespace GymApp14V1.Models.ViewModels
{
#nullable disable
    public class AdminViewModel
    {
        public MemberViewModel Member { get; set; }
        public IEnumerable<MemberViewModel> Members { get; set; }
    }
}
