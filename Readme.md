# ASP.NET Core WebApi Logging with SeriLog and ELK stack

In this repository I want to give a plain starting point at how to log into ElasticSearch using SeriLog and view logs in Kibana ( ELK stack ).

This lets us find what exactly is going on with the system at any given point of time and helps us build a scalable and filterable system.  

In this repository, am using a sample ASP.net code that contains a controller which is dealing with FoodItems. You can GET/POST/PUT/PATCH and DELETE them.   

## Versions

``` http://localhost:29435/swagger ```

![ASPNETCOREWebAPIVersions](./.github/versions.jpg)


## Nuget Packages to be installed

- Serilog.AspNetCore
  - This package also brings in a bunch of enrichers that automatically start to install any request in our system.

- Serilog.Sinks.ElasticSearch
  - This is where we are going to push the data in to Elastic Search

- Serilog.Enrichers.Environment

- ElasticSearch



## appsettings.json

Update the Logging section with below code:

```json
  "ApplicationName":  "elastic-search-app",
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Information",
        "System": "Warning"
      }
   }
```

## POST a foodItem

``` http://localhost:29435/api/v1/foods ```

```javascript
  {
      "name": "Lasagne",
      "type": "Main",
      "calories": 3000,
      "created": "2017-09-16T17:50:08.1510899+02:00"
  }
```

![ASPNETCOREWebAPIGET](./.github/post.jpg)

## PUT a foodItem

``` http://localhost:29435/api/v1/foods/5 ```

``` javascript
{
    "name": "Lasagne2",
    "type": "Main",
    "calories": 3000,
    "created": "2017-09-16T17:50:08.1510899+02:00"
}
```

![ASPNETCOREWebAPIGET](./.github/put.jpg)


## PATCH a foodItem

``` http://localhost:29435/api/v1/foods/5 ```

``` javascript
[
  { "op": "replace", "path": "/name", "value": "mynewname" }
]
```

![ASPNETCOREWebAPIGET](./.github/patch.jpg)

## DELETE a foodItem

``` http://localhost:29435/api/v1/foods/5 ```


![ASPNETCOREWebAPIGET](./.github/delete.jpg)
