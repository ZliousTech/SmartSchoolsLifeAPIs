using SmartSchoolLifeAPI.Core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class SocialMediaController : ApiController
    {
        SmartSchoolsEntities2 db;

        public SocialMediaController()
        {
            db = new SmartSchoolsEntities2();

        }

        [HttpGet]
        public IEnumerable<SocialMediaRole> GetRoles(string UserId)
        {
            var UserIdParameter = new SqlParameter("@UserId", UserId);

            var q = db.Database.SqlQuery<SocialMediaRole>("getRoles @UserId", UserIdParameter).ToList();

            return q;
        }

        [HttpPost]
        public SocialMediaUser Login(SocialMediaUser usr)
        {
            string Password = PasswordEncDec.Encrypt(usr.Password);
            var user = db.SocialMediaUsers.SingleOrDefault(s => s.UserName == usr.UserName && s.Password == Password);
            if (user == null)
                return null;
            else
                return user;
        }
    }
}
