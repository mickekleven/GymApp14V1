using System.ComponentModel.DataAnnotations;

namespace GymApp14V1.Core.ViewModels
{

    public class RoleViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public PageHeaderViewModel PageHeader { get; set; } = new();
    }
}
