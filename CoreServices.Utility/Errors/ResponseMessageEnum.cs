using System.ComponentModel;

namespace CoreServices.Utility.Errors
{
    public enum ResponseMessageEnum
    {
        [Description("SUCCESS")]
        Success,
        [Description("EXCEPTION")]
        Exception,
        [Description("RESQUEST DENIED")]
        Unauthorized,
        [Description("VALIDATION ERROR")]
        ValidationError,
        [Description("FAILED")]
        Failure
    }
}
