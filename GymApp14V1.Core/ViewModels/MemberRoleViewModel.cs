namespace GymApp14V1.Core.ViewModels
{
    public class MemberRoleViewModel
    {
        public MemberViewModel Member { get; set; } = new();
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public PageHeaderViewModel PageHeader { get; set; } = new();
    }
}
