using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleWebServer
{
	public interface IContentService
	{
		Task GetHandler(HttpHandler httpHandler, HttpListenerContext ctx);
		Task PutHandler(HttpHandler httpHandler, HttpListenerContext ctx);
		Task DeleteHandler(HttpHandler httpHandler, HttpListenerContext ctx);
		Task PatchHandler(HttpHandler httpHandler, HttpListenerContext ctx);
	}
}

