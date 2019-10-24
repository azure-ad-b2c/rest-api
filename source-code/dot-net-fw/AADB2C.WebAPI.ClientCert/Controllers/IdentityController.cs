using AADB2C.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
            // Check the certificate
            if (IsValidClientCertificate() == false)
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Your client certificate is not valid", HttpStatusCode.Conflict));
            }

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
            // Check the certificate
            if (IsValidClientCertificate() == false)
            {
                return Content(HttpStatusCode.Conflict, new B2CResponseModel("Your client certificate is not valid", HttpStatusCode.Conflict));
            }

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

        private bool IsValidClientCertificate()
        {
            string ClientCertificateSubject = ConfigurationManager.AppSettings["ClientCertificate:Subject"];
            string ClientCertificateIssuer = ConfigurationManager.AppSettings["ClientCertificate:Issuer"];
            string ClientCertificateThumbprint = ConfigurationManager.AppSettings["ClientCertificate:Thumbprint"];

            X509Certificate2 clientCertInRequest = RequestContext.ClientCertificate;
            if (clientCertInRequest == null)
            {
                Trace.WriteLine("Certificate is NULL");
                return false;
            }

            // Basic verification
            if (clientCertInRequest.Verify() == false)
            {
                Trace.TraceError("Basic verification failed");
                return false;
            }

            // 1. Check the time validity of the certificate
            if (DateTime.Compare(DateTime.Now, clientCertInRequest.NotBefore) < 0 ||
                DateTime.Compare(DateTime.Now, clientCertInRequest.NotAfter) > 0)
            {
                Trace.TraceError($"NotBefore '{clientCertInRequest.NotBefore}' or NotAfter '{clientCertInRequest.NotAfter}' not valid");
                return false;
            }

            // 2. Check the subject name of the certificate
            if (!string.IsNullOrEmpty(ClientCertificateSubject))
            {
                bool foundSubject = false;
                string[] certSubjectData = clientCertInRequest.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in certSubjectData)
                {
                    if (String.Compare(s.Trim(), ClientCertificateSubject) == 0)
                    {
                        foundSubject = true;
                        break;
                    }
                }

                if (!foundSubject)
                {
                    Trace.TraceError($"Subject name '{clientCertInRequest.Subject}' is not valid");
                    return false;
                }
            }

            // 3. Check the issuer name of the certificate
            if (!string.IsNullOrEmpty(ClientCertificateIssuer))
            {
                bool foundIssuerCN = false;
                string[] certIssuerData = clientCertInRequest.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in certIssuerData)
                {
                    if (String.Compare(s.Trim(), ClientCertificateIssuer) == 0)
                    {
                        foundIssuerCN = true;
                        break;
                    }
                }

                if (!foundIssuerCN)
                {
                    Trace.TraceError($"Issuer '{clientCertInRequest.Issuer}' is not valid");
                    return false;
                }
            }

            // 4. Check the thumbprint of the certificate
            if (!string.IsNullOrEmpty(ClientCertificateThumbprint))
            {
                if (String.Compare(clientCertInRequest.Thumbprint.Trim().ToUpper(), ClientCertificateThumbprint) != 0)
                {
                    Trace.TraceError($"Thumbprint '{clientCertInRequest.Thumbprint.Trim().ToUpper()}' is not valid");
                    return false;
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
            return true;
        }
    }
}
