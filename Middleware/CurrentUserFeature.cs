public class CurrentUserFeature
{
    public Guid Id { get; private set; }
    public int Account { get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public IEnumerable<string> Permissions { get; private set; }

    public CurrentUserFeature(Guid id, int account, string name, string email, IEnumerable<string> permissions)
    {
        Id = id;
        Account = account;
        Name = name;
        Email = email;
        Permissions = permissions;
    }
}