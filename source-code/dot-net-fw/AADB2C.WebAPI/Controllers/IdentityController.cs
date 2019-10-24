using AADB2C.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Http;

namespace Contoso.AADB2C.APIBasicAuth.Controllers
{
    public class IdentityController : ApiController
    {
        Random rnd = new Random();

        [Route("api/identity/loyalty")]
        [HttpPost]
        public IHttpActionResult Loyalty()
        {
            // If not data came in, then returen
            if (this.Request.Content == null) throw new Exception();

            // Read the input claims from the request body
            string input = Request.Content.ReadAsStringAsync().Result;

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Request content is empty", HttpStatusCode.Conflict));
            }

            // Convert the input string into InputClaimsModel object
            InputClaimsModel inputClaims = JsonConvert.DeserializeObject(input, typeof(InputClaimsModel)) as InputClaimsModel;

            if (inputClaims == null)
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            //Check if the language parameter is presented
            if (string.IsNullOrEmpty(inputClaims.language))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Language code is null or empty", HttpStatusCode.Conflict));
            }

            //Check if the objectId parameter is presented
            if (string.IsNullOrEmpty(inputClaims.objectId))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("User object Id is null or empty", HttpStatusCode.Conflict));
            }

            // Create output claims object and set the loyalty number with random value
            B2CResponseModel outputClaims = new B2CResponseModel(string.Empty, HttpStatusCode.OK);
            outputClaims.loyaltyNumber = inputClaims.language + "-" + rnd.Next(1000, 9999).ToString();

            // Return the output claim(s)
            return Ok(outputClaims);
        }

        [Route("api/identity/Validate")]
        [HttpPost]
        public IHttpActionResult Validate()
        {
            // If not data came in, then returen
            if (this.Request.Content == null) throw new Exception();

            // Read the input claims from the request body
            string input = Request.Content.ReadAsStringAsync().Result;

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Request content is empty", HttpStatusCode.Conflict));
            }

            // Convert the input string into InputClaimsModel object
            InputClaimsModel inputClaims = JsonConvert.DeserializeObject(input, typeof(InputClaimsModel)) as InputClaimsModel;

            if (inputClaims == null)
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            //Check if the language parameter is presented
            if (string.IsNullOrEmpty(inputClaims.language))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Language code is null or empty", HttpStatusCode.Conflict));
            }

            //Check if the email parameter is presented
            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Email is null or empty", HttpStatusCode.Conflict));
            }

            // Validate the email address 
            if (inputClaims.email.ToLower().StartsWith("test"))
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Your email address can't start with 'test'", HttpStatusCode.Conflict));
            }

            // Create output claims object and set the loyalty number with random value
            B2CResponseModel outputClaims = new B2CResponseModel(string.Empty, HttpStatusCode.OK);
            outputClaims.loyaltyNumber = inputClaims.language + "-" + rnd.Next(1000, 9999).ToString();
            outputClaims.email = inputClaims.email.ToLower();

            // Return the output claim(s)
            return Ok(outputClaims);
        }
    }
}
