# EasyHttp

Read this!

An easy to use HTTP client that supports:

* HEAD, PUT, DELETE, GET, POST
* Cookies
* Authentication
* Dynamic and Static Typing
* XML, JSON and WWW-Url form encoded encoding/decoding
* File upload both via PUT and POST (multipart/formdata)
* Some other neat little features....

## License

Licensed under Modified BSD (i.e. pretty much MIT). 

For full License and included software licenses please see LICENSE.TXT


Please log all issues here: http://youtrack.codebetter.com/issues/EHTTP

## Installation

You can either download the source and compile or use nuget at http://nuget.org. To install with nuget:

  Install-Package EasyHttp

## Documentation

The documentation can be found on the [wiki](https://github.com/hhariri/EasyHttp/wiki). 

## Usage

### Using static types 

To post/put a customer to  some service: 

  
```
	var customer = new Customer(); 
	customer.Name = "Joe"; 
	customer.Email = "joe@smith.com";
	var http = new HttpClient();
	http.Post("url", customer, HttpContentTypes.ApplicationJson);
```
 
To get some data in JSON format:

```
	var http = new HttpClient();
	http.Request.Accept = HttpContentTypes.ApplicationJson;
	var response = http.Get("url");
	var customer = response.StaticBody<Customer>();
	Console.WriteLine("Name: {0}", customer.Name);
```

### Using dynamic  types

To post/put a customer to  some service: 

```
	var customer = new ExpandoObject(); // Or any dynamic type
	customer.Name = "Joe";
	customer.Email = "joe@smith.com";
	var http = new HttpClient();
	http.Post("url", customer, HttpContentTypes.ApplicationJson);
```
 
To get some data in JSON format:


```
	var http = new HttpClient();
	http.Request.Accept = HttpContentTypes.ApplicationJson;
	var response = http.Get("url");
	var customer = response.DynamicBody;
	Console.WriteLine("Name {0}", customer.Name);
```

Both in Static and Dynamic versions, hierarchies are supported.

## Perform a get with parameters

To get some data from a service

 ```
	var http = new HttpClient();
	http.Get("url", new {Name = "test"});
```

Should translate to the following url being passed. url?Name=test the value will be urlencoded.

To get some data in JSon format.

 ```
	var http = new HttpClient();
	http.Request.Accept = HttpContentTypes.ApplicationJson;
	http.Get("url", new {Name = "test"});
```


## Serialization / Deserialization Conventions

For serialization / deserialization, you can use pretty much any type of naming convention, be it Propercase, CamelCase, lowerCamelCase, with_underscores, etc. If for some reason, your convention is not picked up, you can always decorate the property with an attribute:

```
 
   [JsonName("mycustomname")] 
   public string SomeWeirdCombination { get; set; }
```

## Credits

Copyright (c) 2010 - 2011 Hadi Hariri and Project Contributors

JsonFX: Licensed under MIT. EasyHttp uses the awesome JsonFX library at http://github.com/jsonfx
