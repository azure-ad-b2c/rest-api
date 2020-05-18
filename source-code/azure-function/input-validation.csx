#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

    // Check request body
    if (String.IsNullOrEmpty(requestBody))
    {
        return (ActionResult)new BadRequestObjectResult(new ResponseError(){userMessage = "Request content is empty."});
    }

    // Print out the request body
    log.LogInformation("Request body: " + requestBody);

    // Convert the request body into dynamic JSON data object
    dynamic data = JsonConvert.DeserializeObject(requestBody);

    //Check whether the loyaltyId element is presented
    if (data.loyaltyId == null ||  data.loyaltyId.ToString() == "")
    {
        return (ActionResult)new BadRequestObjectResult(new ResponseError(){userMessage = "Missing required `loyaltyId` element."});
    }

    //Check whether the email element is presented
    if (data.email == null ||  data.email.ToString() == "")
    {
        return (ActionResult)new BadRequestObjectResult(new ResponseError(){userMessage = "Missing required `email` element"});
    }

    if (data.loyaltyId.ToString() == "1234")
    {
        return (ActionResult)new BadRequestObjectResult(new ResponseError(){ userMessage = $"Loyalty ID '{data.loyaltyId}' is not associated with '{data.email}' email address."});
    }

    Random random = new Random();
    return (ActionResult)new OkObjectResult(new ResponseContent(){promoCode = random.Next(12345, 99999).ToString()});
}

public class ResponseContent
{
    public string promoCode { get; set; }
}

public class ResponseError
{
   public ResponseError()
   {
      version = "1.0.1";
      status = 409;
   }

    public string version { get; set; }
    public int status { get; set; }
    public string userMessage { get; set; }
}