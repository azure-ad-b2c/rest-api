using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AADB2C.WebAPI.ClientCert.Models;
using AADB2C.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AADB2C.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class IdentityController : Controller
    {
        Random rnd = new Random();
        private readonly AppSettingsModel AppSettings;


        // Demo: Inject an instance of an AppSettingsModel class into the constructor of the consuming class, 
        // and let dependency injection handle the rest
        public IdentityController(IOptions<AppSettingsModel> appSettings)
        {
            this.AppSettings = appSettings.Value;
        }

        [HttpPost(Name = "loyalty")]
        public async Task<ActionResult> Loyalty()
        {
            string input = null;

            // If not data came in, then return
            if (this.Request.Body == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Request content is null", HttpStatusCode.Conflict));
            }

            // Read the input claims from the request body
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                input = await reader.ReadToEndAsync();
            }

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Request content is empty", HttpStatusCode.Conflict));
            }

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Request content is empty", HttpStatusCode.Conflict));
            }

            // Convert the input string into InputClaimsModel object
            InputClaimsModel inputClaims = InputClaimsModel.Parse(input);

            if (inputClaims == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            //Check if the language parameter is presented
            if (string.IsNullOrEmpty(inputClaims.language))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Language code is null or empty", HttpStatusCode.Conflict));
            }

            //Check if the objectId parameter is presented
            if (string.IsNullOrEmpty(inputClaims.objectId))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("User object Id is null or empty", HttpStatusCode.Conflict));
            }

            try
            {
                return StatusCode((int)HttpStatusCode.OK, new B2CResponseModel(string.Empty, HttpStatusCode.OK)
                {
                    loyaltyNumber = inputClaims.language + "-" + rnd.Next(1000, 9999).ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel($"General error (REST API): {ex.Message}", HttpStatusCode.Conflict));
            }
        }

        [HttpPost(Name = "validate")]
        public async Task<ActionResult> validate()
        {
            // Check the certificate
            string certValidation = IsValidClientCertificate();
            if (certValidation != null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel(certValidation, HttpStatusCode.Conflict));
            }

            string input = null;

            // If not data came in, then return
            if (this.Request.Body == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Request content is null", HttpStatusCode.Conflict));
            }

            // Read the input claims from the request body
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                input = await reader.ReadToEndAsync();
            }

            // Check input content value
            if (string.IsNullOrEmpty(input))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Request content is empty", HttpStatusCode.Conflict));
            }

            // Convert the input string into InputClaimsModel object
            InputClaimsModel inputClaims = InputClaimsModel.Parse(input);

            if (inputClaims == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Can not deserialize input claims", HttpStatusCode.Conflict));
            }

            //Check if the language parameter is presented
            if (string.IsNullOrEmpty(inputClaims.language))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Language code is null or empty", HttpStatusCode.Conflict));
            }

            //Check if the email parameter is presented
            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Email is null or empty", HttpStatusCode.Conflict));
            }

            // Validate the email address 
            if (inputClaims.email.ToLower().StartsWith("test"))
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel("Your email address can't start with 'test'", HttpStatusCode.Conflict));
            }

            try
            {
                // Return the email in lower case format
                return StatusCode((int)HttpStatusCode.OK, new B2CResponseModel(string.Empty, HttpStatusCode.OK) {
                    loyaltyNumber = inputClaims.language + "-" + rnd.Next(1000, 9999).ToString(),
                    email = inputClaims.email.ToLower()
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new B2CResponseModel($"General error (REST API): {ex.Message}", HttpStatusCode.Conflict));
            }
        }

        private string IsValidClientCertificate()
        {

            X509Certificate2 clientCertInRequest = Request.HttpContext.Connection.ClientCertificate;
            if (clientCertInRequest == null)
            {
                return "Certificate is NULL";
            }

            // Basic verification
            if (clientCertInRequest.Verify() == false)
            {
                return "Basic verification failed";
            }

            // 1. Check the time validity of the certificate
            if (DateTime.Compare(DateTime.Now, clientCertInRequest.NotBefore) < 0 ||
                DateTime.Compare(DateTime.Now, clientCertInRequest.NotAfter) > 0)
            {
                return $"NotBefore '{clientCertInRequest.NotBefore}' or NotAfter '{clientCertInRequest.NotAfter}' not valid";
            }

            // 2. Check the subject name of the certificate
            if (!string.IsNullOrEmpty(this.AppSettings.ClientCertificateSubject))
            {
                bool foundSubject = false;
                string[] certSubjectData = clientCertInRequest.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in certSubjectData)
                {
                    if (String.Compare(s.Trim(), this.AppSettings.ClientCertificateSubject) == 0)
                    {
                        foundSubject = true;
                        break;
                    }
                }

                if (!foundSubject)
                {
                    return $"Subject name '{clientCertInRequest.Subject}' is not valid";
                }
            }

            // 3. Check the issuer name of the certificate
            if (!string.IsNullOrEmpty(this.AppSettings.ClientCertificateIssuer))
            {
                bool foundIssuerCN = false;
                string[] certIssuerData = clientCertInRequest.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in certIssuerData)
                {
                    if (String.Compare(s.Trim(), this.AppSettings.ClientCertificateIssuer) == 0)
                    {
                        foundIssuerCN = true;
                        break;
                    }
                }

                if (!foundIssuerCN)
                {
                    return $"Issuer '{clientCertInRequest.Issuer}' is not valid";
                }
            }

            // 4. Check the thumbprint of the certificate
            if (!string.IsNullOrEmpty(this.AppSettings.ClientCertificateThumbprint))
            {
                if (String.Compare(clientCertInRequest.Thumbprint.Trim().ToUpper(), this.AppSettings.ClientCertificateThumbprint) != 0)
                {
                    return $"Thumbprint '{clientCertInRequest.Thumbprint.Trim().ToUpper()}' is not valid";
                }
            }

            // 5. If you also want to test whether the certificate chains to a trusted root authority, you can uncomment the following code:
            //
            //X509Chain certChain = new X509Chain();
            //certChain.Build(certificate);
            //bool isValidCertChain = true;
            //foreach (X509ChainElement chElement in certChain.ChainElements)
            //{
            //    if (!chElement.Certificate.Verify())
            //    {
            //        isValidCertChain = false;
            //        break;
            //    }
            //}
            //if (!isValidCertChain) return false;
            return null;
        }

    }
}