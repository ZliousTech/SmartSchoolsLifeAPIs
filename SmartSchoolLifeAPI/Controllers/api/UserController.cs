using SmartSchoolLifeAPI.Core.Models;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class UserController : ApiController
    {
        SmartSchoolsEntities3 db;

        public UserController()
        {
            db = new SmartSchoolsEntities3();
        }

        private SchoolBranch GetSchoolName(int? schoolId)
        {
            var paramSchoolId = new SqlParameter("@SchoolId", schoolId);
            var schoolBransh = db.Database.SqlQuery<SchoolBranch>("SELECT SchoolArabicName, SchoolEnglishName FROM SchoolBranches WHERE SchoolID = @SchoolID ", paramSchoolId)
                .SingleOrDefault();

            return schoolBransh;
        }

        [HttpPost]
        public User Login(User usr)
        {
            string Password = PasswordEncDec.Encrypt(usr.Password);
            var user = db.Users.SingleOrDefault(s => s.UserName == usr.UserName && s.Password == Password);
            if (user == null)
                return null;
            else
            {
                var schoolBransh = GetSchoolName(user.SchoolID);
                user.SchoolArabicName = schoolBransh.SchoolArabicName;
                user.SchoolEnglishName = schoolBransh.SchoolEnglishName;
                return user;
            }
        }

        [HttpPut]
        public void ChangePW(User usr)
        {
            string Password = PasswordEncDec.Encrypt(usr.Password);
            var user = db.Users.SingleOrDefault(s => s.UserName == usr.UserName);
            user.Password = Password;
            db.SaveChanges();
        }

        [HttpGet]
        public UserPlainPassword GetStaffUserPassword(UserPassword usr)
        {
            var paramUserName = new SqlParameter("@StaffUserName", usr.UserName);
            var paramCountryKeyCode = new SqlParameter("@staffCountrKeyCode", usr.CountryKeyCode);
            var paramMobileNumber = new SqlParameter("@staffMobileNo", usr.MobileNumber);
            var PlainPass = "";

            if (usr.UserType == "System")
            {
                var UserInfo = db.Database.SqlQuery<UserPassword>("GetStaffUserPassword @StaffUserName, @staffCountrKeyCode, @staffMobileNo", paramUserName, paramCountryKeyCode, paramMobileNumber).SingleOrDefault();
                if (UserInfo != null)
                {
                    PlainPass = UserInfo.Password;
                    PlainPass = PasswordEncDec.Decrypt(PlainPass);
                }
            }
            else
            {
                if (usr.UserType == "Guardian")
                {
                    var UserInfo = db.Database.SqlQuery<UserPassword>("GetGuardianPassword @StaffUserName, @staffCountrKeyCode, @staffMobileNo", paramUserName, paramCountryKeyCode, paramMobileNumber).SingleOrDefault();
                    if (UserInfo != null)
                    {
                        PlainPass = UserInfo.Password;
                        PlainPass = PasswordEncDec.Decrypt(PlainPass);
                    }
                }
            }

            UserPlainPassword _userPlainPass = new UserPlainPassword();
            _userPlainPass.Password = PlainPass;

            return _userPlainPass;
        }

    }
}
