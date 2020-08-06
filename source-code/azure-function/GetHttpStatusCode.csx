#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
            log.LogInformation(string.Format("correlationId: {0}, Timestamp: {1}", Guid.NewGuid(), DateTime.UtcNow.ToString("o")));

            string query = req.Query["status"];
            int httpStatus = 200; //default is 200
            var isValidStatus = Int32.TryParse(query, out httpStatus);
            ObjectResult result;
             
            if (isValidStatus)
            {
                if (httpStatus >= 200 && httpStatus <= 299)
                {
                    var response = new ResponseContent() { randomNumber = (new Random()).Next(Int32.MaxValue) };
                    result = new ObjectResult(response) { StatusCode = httpStatus };
                    log.LogInformation(string.Format("randomNumber: {0} , StatusCode {1}, TimeStamp {2}", response.randomNumber, result.StatusCode, DateTime.UtcNow.ToString("o")));

                    return result;
                }
                if (httpStatus == 409)
                {
                    return Get409Message(log);
                }

                result = new ObjectResult("") { StatusCode = httpStatus };
                log.LogInformation(string.Format("StatusCode {0}, TimeStamp {1}", result.StatusCode, DateTime.UtcNow.ToString("o")));
                return result;
            }
            return Get409Message(log);
}

  public static ActionResult Get409Message(ILogger log)
        {
            var result = new ObjectResult(new ResponseError()) { StatusCode = 409 };
            log.LogInformation(string.Format("StatusCode {0}, TimeStamp {1}", result.StatusCode, DateTime.UtcNow.ToString("o")));
            return result;
        }


 public class ResponseContent
    {
        public ResponseContent()
        {
            randomNumber = 0;
        }
        public int randomNumber { get; set; }
    }

    public class ResponseError
    {
        public ResponseError()
        {
            version = "1.0.1";
            status = 409;
            requestId = Guid.NewGuid().ToString();
            userMessage = "We are having an issue while processing this request, please try later.";
            developerMessage = string.Format("Sending status 409 back. Date: {0}", DateTime.UtcNow.ToString("o"));
            moreInfo = "https://docs.microsoft.com/en-us/azure/active-directory-b2c/restful-technical-profile";
        }

        public string version { get; set; }
        public int status { get; set; }
        public string code { get; set; }
        public string userMessage { get; set; }
        public string developerMessage { get; set; }
        public string requestId { get; set; }
        public string moreInfo { get; set; }

    }
