namespace GymApp14V1.Core.ViewModels
{
#nullable disable
    public class AdminViewModel
    {
        public MemberViewModel Member { get; set; }
        public IEnumerable<MemberViewModel> Members { get; set; }
    }
}
