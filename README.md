# Superformula Insurance Policy API

## Onboarding Consume API

There are three endpoints for this API, to consume them, follow the instructions bellow each one, adding the corresponding payload or query params.
The base url for this API is '**api/insurancepolicies**'

'Create Policy' send a POST request with a payload like this:
```
{
  "insurancePolicy": {            
    "effectiveDate": "2022-05-17T03:19:42.172Z",
    "firstName": "Walterio",
    "lastName": "Valdez",
    "licenseNumber": "L000132",
    "address": {"street" : "street" , "city": "city", "state": "state", "zipcode":"77535"},
    "expirationDate": "2022-05-17T03:19:42.172Z",
    "premium": 0,    
    "vehicleDetail": {
      "year": 1968,
      "model": "Impala",
      "manufacturer": "Chevrolet",
      "name": "Impala"
    }
  }
}
```

'Get Policies by license number' send a GET request to the endpoint '**/licensenumber/{licenseNumber}**' there are two optional parameters that can be added as query params:
- sortByYear i.e. '**/licensenumber/L000132?sortByYear=true**'
- includeExpiredPolicies  '**/licensenumber/L000132?includeExpiredPolciies=true**'

'Get Policy by Id' send a GET request to the endpoint  '**/id/{policyId}/licensenumber/{licenseNumber}**'


## State Regulation
The State Regulation can be done in a separate microservice and the Insurance Policies API will need to consume it. The API already has it's own interface for the state regulation, we might need to add the new ones and an implementation for consuming them. Also we can use the pattern Publish/Subscribe for the communication between these two.

## Hand the code off 
Productionizing the API will need the CI/CD part to handle deployments. In terms of logging, I would recomend to use Application Insights, we might need to add a few tweaks into the code to use them, but nothing hard. Azure Cognitive Search can be helpful for PII data. For extending methods, we can use versions, we might need to do a couple of changes to identify them, but for the sake of safe, this is a good approach.


## Azure Deployment
Deployment into Azure involves a few resources needed. 
- API / It can be deployed into an App Service, also this can be added simply to the CI/CD pipelines using the web deploy task 
- Database / It can be deployed into an Azure SQL Database, the task that run migrations, must be included in the pipelines.
- Service Bus / It is necessary to create a Service Bus, and make the necessary configuration to handle handle the messages