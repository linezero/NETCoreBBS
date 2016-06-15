using System.Threading.Tasks;
using NetCoreBBS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace NetCoreBBS
{
    public interface IUserServices
    {
        Task<BBSUser> User { get; }
    }
    public class UserServices : IUserServices
    {
        public UserManager<BBSUser> UserManager { get; }
        private IHttpContextAccessor Context;
        public UserServices(UserManager<BBSUser> userManager, IHttpContextAccessor context)
        {
            UserManager = userManager;
            Context = context;
        }
        public Task<BBSUser> User
        {
            get
            {
                return UserManager.GetUserAsync(Context.HttpContext.User);
            }
        }
    }
}
