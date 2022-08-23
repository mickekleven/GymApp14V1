namespace GymApp14V1.Core.ViewModels
{

    public class RoleViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public PageHeaderViewModel PageHeader { get; set; } = new();
    }
}
