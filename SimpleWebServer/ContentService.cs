using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleWebServer
{
	public class ContentService
	{
		private readonly IContentService _service;

		public ContentService (IContentService impl)
		{
			_service = impl;
		}

		public Task ProcessRequest (HttpHandler httpHandler, HttpListenerContext ctx)
		{
			var method = ctx.Request.HttpMethod.ToLowerInvariant ();
			switch (method) {
			case "get":
				return GetHandler (httpHandler, ctx);
			case "put":
				return PutHandler (httpHandler, ctx);
			case "delete":
				return DeleteHandler (httpHandler, ctx);
			case "patch":
				return PatchHandler (httpHandler, ctx);
			default:
				throw new NotImplementedException ();
			}
		}

		private Task GetHandler (HttpHandler httpHandler, HttpListenerContext ctx)
		{
			return _service.GetHandler (httpHandler, ctx);
		}

		private Task PutHandler (HttpHandler httpHandler, HttpListenerContext ctx)
		{
			return _service.PutHandler (httpHandler, ctx);
		}

		private Task DeleteHandler (HttpHandler httpHandler, HttpListenerContext ctx)
		{
			return _service.DeleteHandler (httpHandler, ctx);
		}

		private Task PatchHandler (HttpHandler httpHandler, HttpListenerContext ctx)
		{
			return _service.PatchHandler (httpHandler, ctx);
		}
	}
}

