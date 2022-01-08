using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace BECore.API.Attributes
{
    public class ElasticSearchAttribute : Attribute, IAsyncActionFilter
    {
        private readonly bool _enable;

        public ElasticSearchAttribute(bool enable)
        {
            _enable = enable;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_enable)
            {

            }
            else
            {
                await next();
                return;
            }
        }
    }
}
