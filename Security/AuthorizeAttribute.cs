[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string Permission { get; }
    public string Description { get; }

    public AuthorizeAttribute(string permission, string description)
    {
        Permission = permission ?? throw new ArgumentNullException();
        Description = description ?? throw new ArgumentNullException();;
    }
}