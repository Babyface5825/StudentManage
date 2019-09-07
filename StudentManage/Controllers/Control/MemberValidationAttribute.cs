using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace StudentManage.Controllers
{
    public class MemberValidationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //获取Cookies中的Login
            var memberValidation = HttpContext.Current.Request.Cookies.Get("LoginUser");
            //如果memberValidation为null  或者 memberValidation不等于Success
            if (memberValidation == null)
            {
                //页面跳转到 登录页面
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "adminMain", controller = "Login", action = "Index" }));
                return;
            }
            //通过验证
            return;
        }
    }
}