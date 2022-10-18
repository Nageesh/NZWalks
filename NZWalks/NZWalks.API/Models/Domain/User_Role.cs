namespace NZWalks.API.Models.Domain
{
    public class User_Role
    {
        public Guid Id { get; set; }

        //Navigation property for User
        public Guid UserId { get; set; }
        public User user  { get; set; }

        public Guid RoleId { get; set; }
        public Role role   { get; set; }
    }
}
