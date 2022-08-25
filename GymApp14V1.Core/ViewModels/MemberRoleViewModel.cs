namespace GymApp14V1.Core.ViewModels
{
    public class MemberRoleViewModel
    {
        public IEnumerable<MemberViewModel> Members { get; set; } = new List<MemberViewModel>();

        public MemberViewModel Member { get; set; } = new();
        public IEnumerable<string> MemberRoles { get; set; } = new List<string>();
        public IEnumerable<string> AspNetRoles { get; set; } = new List<string>();

        public IEnumerable<string> NotSelectedRoles { get; set; } = new List<string>();

        public PageHeaderViewModel PageHeader { get; set; } = new();

        public string SelectedRole { get; set; } = string.Empty;
    }
}
