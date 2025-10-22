using System.ComponentModel.DataAnnotations;

namespace ChatNest.Models.DTO
{
    public class AdminChangeUserRoleDTO
    {
        [Required]
        public Guid AdminID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string NewRole { get; set; }
    }

    public class AdminToggleUserStatusDTO
    {
        [Required]
        public Guid AdminID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public bool NewStatus { get; set; }
    }

    public class AdminResponseModelDTO
    {
        public int MessageID { get; set; }
        public string MessageDescription { get; set; }
    }
}
