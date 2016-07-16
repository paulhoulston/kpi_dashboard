using System;
using Nancy;

namespace Optionis.KPIs.Dashboard.Modules
{
    class SerializedError
    {
        readonly Enum _errorCode;
        readonly string _errorMessage;
        readonly IResponseFormatter _response;

        public SerializedError(Enum errorCode, string errorMessage, IResponseFormatter response)
        {
            _errorCode = errorCode;
            _errorMessage = errorMessage;
            _response = response;
        }

        public Response ErrorResponse()
        {
            return _response.AsJson(new
            {
                Error = new
                {
                    Code = _errorCode,
                    Message = _errorMessage
                }
            }, HttpStatusCode.BadRequest);
        }
    }
}