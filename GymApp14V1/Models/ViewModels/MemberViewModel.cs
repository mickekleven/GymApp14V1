﻿namespace GymApp14V1.Models.ViewModels
{
    public class MemberViewModel
    {
#nullable disable
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int GymClassId { get; set; }
        public IEnumerable<GymClass> GymClasses { get; set; } = new List<GymClass>();
    }
}
