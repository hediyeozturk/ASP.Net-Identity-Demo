using System.Collections.Generic;

namespace Identity.Management.JwtApp.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel(){ UserName ="hediye", EmailAddress ="hediye.tasar@dtechsoft.com", Password  ="myPass", Name ="Hediye", Surname ="Ozturk", Role="Seller"},
            new UserModel(){ UserName ="admn", EmailAddress ="admn@dtechsoft.com", Password  ="myPass", Name ="Admin", Surname ="Adm", Role="Administrator"}

        };
    }
}
