*EasyHttp*

Licensed under BSD. For full License and included software licenses please see LICENSE.TXT

Please log all issues are tracked here: http://youtrack.codebetter.com/issues/EHTTP


USAGE
=====

To do a POST with JSON:


  var customer = new Customer();

  customer.Name = "Joe";
  customer.Email = "joe@smith.com";


  var http = new HttpClient();

  http.Post("url", customer, HttpContentTypes.ApplicationJson);
  
 
To get some data in JSON format:

  var http = new HttpClient();

  var response = 
	http
	.WithAccept(HttpContentTypes.ApplicationJson)
	.Get("url");


  dynamic customer = response.DynamicBody;

  Console.WriteLine(String.Format("Name: {0} - Email: {1}, customer.Name, customer.Email));


  If you want Static:

  var customer = response.StaticBody<Customer>();

  If you want just raw undecoded text:

  var customer = response.RawText;

