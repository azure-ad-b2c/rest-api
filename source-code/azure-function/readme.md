# Azure AD B2C integrate REST API using Azure functions

[Azure Functions](https://docs.microsoft.com/azure/azure-functions/functions-overview) allows you to run small pieces of code (called "functions") without worrying about application infrastructure. With Azure Functions, the cloud infrastructure provides all the up-to-date servers you need to keep your application running at scale.


## REST API samples

Using Azure AD B2C, you can add your own business logic to a user journey by calling your own RESTful service. The Identity Experience Framework can send and receive data from your RESTful service to exchange claims.

- [input-validation.csx](input-validation.csx), validates user input data
- [obtain-claims.csx](obtain-claims.csx), enriches user data by further integrating with corporate line-of-business applications
- [GetHttpStatusCode.csx](GetHttpStatusCode.csx), return HTTP [response code](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html) as passed in the query string, e.g. ```?status=200```. This help you simulate a call to a REST-API and test B2C policy against various HTTP responses. Live sample of this function is available at [GetHttpStatusCode]( https://gethttpresponsestatus.azurewebsites.net/api/GetHttpStatusCode?status=200)

  Following ```cUrl``` commands show usage pattern of GetHttpStatusCode:

  ```
  $ curl -s -D -  https://gethttpresponsestatus.azurewebsites.net/api/GetHttpStatusCode?status=409
  ```
  ```
  HTTP/1.1 409 Conflict
  Content-Length: 363
  Content-Type: application/json; charset=utf-8

  {"version":"1.0.1","status":409,"code":null,"userMessage":"We are having an issue while processing this request, please try later.","developerMessage":"Sending status 409 back. Date: 2020-08-06T21:55:39.9805241Z","requestId":"70188cd5-f224-4418-bb2f-c196fd9c8353","moreInfo":"https://docs.microsoft.com/en-us/azure/active-directory-b2c/restful-technical-profile"}
  ```

  ```
  $ curl -s -D -  https://gethttpresponsestatus.azurewebsites.net/api/GetHttpStatusCode?status=201
  ```
  ```
  HTTP/1.1 201 Created
  Content-Length: 26
  Content-Type: application/json; charset=utf-8

  {"randomNumber":433436479
  ```

  ```
  $ curl -s -D -  https://gethttpresponsestatus.azurewebsites.net/api/GetHttpStatusCode?status=500
  ```
  ```
  HTTP/1.1 500 Internal Server Error
  Content-Type: application/json; charset=utf-8
  Content-Length: 0
  ```
  ```
  $ curl -s -D -  https://gethttpresponsestatus.azurewebsites.net/api/GetHttpStatusCode
  ```
  ```
  HTTP/1.1 409 Conflict
  Content-Length: 363
  Content-Type: application/json; charset=utf-8

  {"version":"1.0.1","status":409,"code":null,"userMessage":"We are having an issue while processing this request, please try later.","developerMessage":"Sending status 409 back. Date: 2020-08-06T21:55:39.9805241Z","requestId":"70188cd5-f224-4418-bb2f-c196fd9c8353","moreInfo":"https://docs.microsoft.com/en-us/azure/active-directory-b2c/restful-technical-profile"}
  ```

## Create your Azure function in the Azure portal

Sign in to the Azure portal at [https://portal.azure.com](https://portal.azure.com) with your Azure account.

### Create a function app

You must have a function app to host the execution of your functions. A function app lets you group functions as a logical unit for easier management, deployment, scaling, and sharing of resources. Follow the guidance how to [create a function app from the Azure portal](https://docs.microsoft.com/azure/azure-functions/functions-create-function-app-portal).

### Create an HTTP triggered function

1. Sign in to the [Azure portal](https://portal.azure.com/).
1. Make sure you're using the directory that contains subscription (not the Azure AD B2C tenant).
1. Choose **All services** in the top-left corner of the Azure portal, and then search for and **Function app**.
1. Expand your function app, then select the **+** button next to **Functions**.
1. Select **HTTP trigger**
1. For the **Name** type 'ValidateProfile'. For the authorization level choose the default `Function`, and click **Create**
1. Replace the entire content of the **input-validation.csx** or **obtain-claims.csx** file with the content of the [run.csx](run.csx) file.
1. Click **Save**
1. Click **Get function URL** and copy the URL of your Azure function.  
