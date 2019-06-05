using VirtualPhysicsLab.Web.Data;

namespace VirtualPhysicsLab.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected VPLContext VPLContext { get; }
        public BaseRepository(VPLContext context)
        {
            VPLContext = context;
        }
    }
}
