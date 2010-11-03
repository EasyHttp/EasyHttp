An HTTP Client for .NET. Allows you to easily make HTTP calls. 


TODO
====

- Needs clean-up
- Authentication needs to be better implemented


USAGE
=====

Do do a POST with JSON:


  var customer = new Customer();

  customer.Name = "Joe";
  customer.Email = "joe@smith.com";


  var http = new HttpClient();

  http.Post("url", customer, "application/json");
  
 
Do get some data in JSON format:

  var http = new HttpClient();

  var response = 
	http
	.WithAccept("application/json")
	.Get("url");


  dynamic customer = response.Body;

  Console.WriteLine(String.Format("Name: {0} - Email: {1}, customer.Name, customer.Email));


Encoding
========

HttpClient will automatically encode it for you to application/json. 

Content-Types currently supported: application/json, application/xml, application/x-www-form-urlencoded


