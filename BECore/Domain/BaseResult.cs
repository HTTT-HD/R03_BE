using Common.Utils;
using Microsoft.AspNetCore.Http;

namespace Domain
{
    public abstract class BaseResult
    {
        public virtual ServiceResponse Accepted(object data = default) => ServiceResponse.Succeed(StatusCodes.Status202Accepted, data);

        public virtual ServiceResponse BadRequest(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status400BadRequest, code, message);

        public virtual ServiceResponse Created(object data = default) => ServiceResponse.Succeed(StatusCodes.Status201Created, data);
        public virtual ServiceResponse Conflict(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status409Conflict, code, message);

        public virtual ServiceResponse Forbidden(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status403Forbidden, code, message);

        public virtual ServiceResponse NotFound(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status404NotFound, code, message);

        public virtual ServiceResponse Ok(object data = default) => ServiceResponse.Succeed(StatusCodes.Status200OK, data);

        public virtual ServiceResponse Unauthorized(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status401Unauthorized, code, message);
    }
}
