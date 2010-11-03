An HTTP Client for .NET. Allows you to easily make HTTP calls. 


TODO
====

- Needs clean-up
- Authentication needs to be better implemented
- Add other TODO's items here... (i.e. not even close to finished but functional)

USAGE
=====

To do a POST with JSON:


  var customer = new Customer();

  customer.Name = "Joe";
  customer.Email = "joe@smith.com";


  var http = new HttpClient();

  http.Post("url", customer, "application/json");
  
 
To get some data in JSON format:

  var http = new HttpClient();

  var response = 
	http
	.WithAccept("application/json")
	.Get("url");


  dynamic customer = response.Body;

  Console.WriteLine(String.Format("Name: {0} - Email: {1}, customer.Name, customer.Email));

EasyHttp's Body is dynamic. It will automatically decode json, xml for you so you can just access the properties. 


Encoding
========

HttpClient will automatically encode it for you to application/json. 

Content-Types currently supported: application/json, application/xml, application/x-www-form-urlencoded


