using System.Threading.Tasks;
using NetCoreBBS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace NetCoreBBS
{
    public interface IUserServices
    {
        Task<User> User { get; }
    }
    public class UserServices : IUserServices
    {
        public UserManager<User> UserManager { get; }
        private IHttpContextAccessor Context;
        public UserServices(UserManager<User> userManager, IHttpContextAccessor context)
        {
            UserManager = userManager;
            Context = context;
        }
        public Task<User> User
        {
            get
            {
                return UserManager.GetUserAsync(Context.HttpContext.User);
            }
        }
    }
}
