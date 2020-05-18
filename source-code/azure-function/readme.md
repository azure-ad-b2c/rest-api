# Azure AD B2C integrate REST API using Azure functions

[Azure Functions](https://docs.microsoft.com/azure/azure-functions/functions-overview) allows you to run small pieces of code (called "functions") without worrying about application infrastructure. With Azure Functions, the cloud infrastructure provides all the up-to-date servers you need to keep your application running at scale.

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