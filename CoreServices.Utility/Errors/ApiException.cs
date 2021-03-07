using CoreServices.Utility.Faults;
using System;
using System.Net;

namespace CoreServices.Utility.Errors
{
    public class ApiException<T> : Exception where T : Fault
    {
        private T _faultDetails;
        public int StatusCode { get; set; }

        public Fault Fault
        {
            get => _faultDetails;
            protected set => _faultDetails = (T)value;
        }

        public ApiException()
        {
            _faultDetails = (T)new Fault
            {
                ErrorOccured = true,
                FaultDescription = Faultkey.MsgGenericException.Description,
                Faultkey = Faultkey.MsgGenericException
            };
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public ApiException(T faultDetails, int statusCode = 500)
        {
            _faultDetails = faultDetails;
            StatusCode = statusCode;
        }

        public ApiException(string message, int statusCode = 500):base(message)
        {
            StatusCode = statusCode;
        }


    }
}
