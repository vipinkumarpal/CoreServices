using CoreServices.Utility.Errors;
using CoreServices.Utility.Extension;
using CoreServices.Utility.Faults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreServices.Utility.Middleware
{
    public class ExceptionMiddleware
    {
        private static ILogger _logger;

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)

        {

            _logger = logger;

            _next = next;

        }



        public async Task InvokeAsync(HttpContext context)

        {

            if (IsSwagger(context))

                await _next(context);

            else

            {

                var originalBodyStream = context.Response.Body;



                using (var responseBody = new MemoryStream())

                {

                    context.Response.Body = responseBody;



                    try

                    {

                        await _next(context);



                        if (context.Response.StatusCode == (int)HttpStatusCode.OK ||

                            context.Response.StatusCode == (int)HttpStatusCode.Accepted ||

                            context.Response.StatusCode == (int)HttpStatusCode.Created)

                        {

                            var body = await FormatResponse(context.Response);

                            await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);

                        }

                        else if (context.Response.StatusCode == (int)HttpStatusCode.NoContent)

                        {

                        }

                        else

                        {

                            await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);

                        }

                    }

                    catch (Exception ex)

                    {

                        await HandleExceptionAsync(context, ex);

                    }

                    finally

                    {

                        responseBody.Seek(0, SeekOrigin.Begin);

                        await responseBody.CopyToAsync(originalBodyStream);

                    }

                }

            }

        }



        private static Task HandleExceptionAsync(HttpContext context, Exception exception)

        {

            Fault fault;

            string typeOfError = null;

            int code;



            switch (exception)

            {

                case ApiException<Fault> ex:

                    fault = ex.Fault;

                    code = ex.StatusCode;

                    _logger.LogError(ex.Fault.FaultDescription, ex);

                    break;

                case ValidationException vex:

                    fault = new Fault(vex.Errors);

                    typeOfError = ResponseMessageEnum.ValidationError.GetDescription();

                    code = (int)HttpStatusCode.BadRequest;

                    context.Response.StatusCode = code;

                    _logger.LogError(vex.Message, vex);

                    break;

                case DbException dbex:

                    fault = new Fault

                    {

                        ErrorOccured = true,

                        Details = dbex.StackTrace,

                        FaultDescription = dbex.Message

                    };

                    typeOfError = ResponseMessageEnum.Exception.GetDescription();

                    code = (int)HttpStatusCode.BadRequest;

                    context.Response.StatusCode = code;

                    _logger.LogError(dbex.Message, dbex);

                    break;

                case AggregateException aex:

                    var flattenedAggregateException = aex.Flatten();

                    var apiException = flattenedAggregateException.InnerException as ApiException<Fault>;

                    const string delimiter = ",";

                    fault = new Fault

                    {

                        ErrorOccured = true,

                        FaultDescription = apiException?.Fault?.FaultDescription ??

                                                flattenedAggregateException.InnerExceptions

                                                .Select(ex => ex.InnerException?.Message ?? ex.Message)

                                                .Aggregate((i, j) => i + delimiter + j)

                    };

                    typeOfError = ResponseMessageEnum.Exception.GetDescription();

                    code = (int)HttpStatusCode.BadRequest;

                    context.Response.StatusCode = code;

                    if (apiException != null)

                        _logger.LogError(apiException.Fault?.FaultDescription ?? apiException.Message, apiException);

                    else

                        _logger.LogError(aex.Message, aex);

                    break;

                default:

#if !DEBUG

                var msg = "An unhandled error occurred.";

                string stack = null;

#else

                    var msg = $"{Faultkey.MsgGenericException.Description} - {exception.GetBaseException().Message}";

                    string stack = exception.StackTrace;

#endif



                    fault = new Fault(msg);

                    fault.Details = stack;

                    code = (int)HttpStatusCode.InternalServerError;

                    context.Response.StatusCode = code;

                    _logger.LogError(exception.Message, exception);

                    break;

            }



            context.Response.ContentType = "application/json";



            var apiResponse = new ApiResponse(code, typeOfError ?? ResponseMessageEnum.Exception.GetDescription(), null, fault);



            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);

        }



        private static Task HandleNotSuccessRequestAsync(HttpContext context, int code)

        {

            context.Response.ContentType = "application/json";



            Fault apiError;



            if (code == (int)HttpStatusCode.NotFound)

                apiError = new Fault("The specified URI does not exist. Please verify and try again.");

            else if (code == (int)HttpStatusCode.NoContent)

                apiError = new Fault("The specified URI does not contain any content.");

            else

                apiError = new Fault("Your request cannot be processed.");



            var apiResponse = new ApiResponse(code, ResponseMessageEnum.Failure.GetDescription(), null, apiError);



            context.Response.StatusCode = code;



            var json = JsonConvert.SerializeObject(apiResponse);



            return context.Response.WriteAsync(json);

        }



        private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)

        {

            context.Response.ContentType = "application/json";

            string jsonString;

            ApiResponse apiResponse;



            var bodyText = !body.ToString().IsValidJson() ? JsonConvert.SerializeObject(body) : body.ToString();



            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);



            Type type = bodyContent?.GetType();



            if (type == typeof(JObject))

            {

                var error = JsonConvert.DeserializeObject<Fault>(bodyText);

                if (error.ErrorOccured)

                {

                    apiResponse = new ApiResponse(400, ResponseMessageEnum.Failure.GetDescription(), null, error);

                    jsonString = JsonConvert.SerializeObject(apiResponse);

                }

                else

                {

                    apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);

                    jsonString = JsonConvert.SerializeObject(apiResponse);

                }

            }

            else

            {

                apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);

                jsonString = JsonConvert.SerializeObject(apiResponse);

            }



            return context.Response.WriteAsync(jsonString);

        }



        private static async Task<string> FormatResponse(HttpResponse response)

        {

            response.Body.Seek(0, SeekOrigin.Begin);

            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);



            return plainBodyText;

        }



        private bool IsSwagger(HttpContext context)

        {

            return context.Request.Path.StartsWithSegments("/swagger");

        }


    }
}
