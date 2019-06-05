namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IConnectionSettings
    {
        string ConnectionString { get; }
        string OrganizationUrl { get; }
        string DbConnectionString { get; }
    }
}
