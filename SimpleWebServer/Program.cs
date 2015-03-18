using System;

namespace SimpleWebServer
{
	class MainClass
	{
		private static HttpHandler _httpHandler;

		public static void Main (string[] args)
		{
			int port = 8080;
			if (args.Length > 0)
			{
				if (!int.TryParse (args [0], out port)) {
					Console.WriteLine ("{0} is not a valid port number", args [0]);
					return;
				}
			}

			// Create an http server that will listen on port 80
			_httpHandler = new HttpHandler (port);

			// Install a couple content services
			var hello = new HelloWorldContentService ();
			_httpHandler.AddService ("/", hello);
			_httpHandler.AddService ("/hello", hello);
			_httpHandler.AddService ("/time", new TimeContentService ());

			// Start the http server
			_httpHandler.Start ();

			// If you haven't provided a content service that will call 'Stop', then
			//   this will wait forever.
			_httpHandler.WaitForExit ();
		}
	}
}
