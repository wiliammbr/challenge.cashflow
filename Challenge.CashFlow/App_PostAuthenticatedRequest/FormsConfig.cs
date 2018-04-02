using Challenge.CashFlow.DAL;
using Challenge.CashFlow.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Challenge.CashFlow.App_PostAuthenticatedRequest
{
    public class FormsConfig
    {
        public static void RegisterAuthenticationBehavior(HttpRequest request)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (CashFlowContext entities = new CashFlowContext())
                        {
                            User user = entities.Users.SingleOrDefault(u => u.UserName == username);

                            roles = user.Roles;
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }
    }
}