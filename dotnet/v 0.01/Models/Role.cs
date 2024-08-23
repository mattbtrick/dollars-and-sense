namespace Models
{
    public class Role
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
