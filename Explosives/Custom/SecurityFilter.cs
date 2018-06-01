using System.Linq;

namespace Explosives.Custom
{
    using System.Web;
    using System.Web.Mvc;

    public class SecurityFilter : ActionFilterAttribute
    {
        private readonly string _Key;
        private readonly object[] _Args;
        public SecurityFilter(string key, params object[] args)
        {
            _Key = key;
            _Args = args;
        }

        //Called
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session[_Key] == null || !_Args.Contains(session[_Key]))
            {
                filterContext.Result = new RedirectResult("/Account/Login", false);
            }

            base.OnActionExecuting(filterContext);

        }
    }
}