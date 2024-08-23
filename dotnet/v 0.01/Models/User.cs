namespace Models
{
    public class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    }
}
