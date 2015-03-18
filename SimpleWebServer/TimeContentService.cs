using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
	public class TimeContentService : IContentService
	{
		public Task GetHandler(HttpHandler httpHandler, HttpListenerContext ctx)
		{
			var sb = new StringBuilder();
			sb.AppendLine ("<!DOCTYPE html PUBLIC \"-//IETF//DTD HTML 2.0//EN\">");
			sb.AppendLine ("<HTML>");
			sb.AppendLine ("  <HEAD>");
			sb.AppendLine ("    <TITLE>Current Time</TITLE>");
			sb.AppendLine ("  </HEAD>");
			sb.AppendLine ("  <BODY>");
			sb.AppendLine (string.Format("    <H1>The current UTC time is : {0}</H1>",
			                             DateTime.UtcNow.ToLongTimeString()));
			sb.AppendLine ("  </BODY>");
			sb.AppendLine ("</HTML>");

			var data = Encoding.UTF8.GetBytes(sb.ToString());
			ctx.Response.ContentLength64 = data.Length;
			ctx.Response.ContentType = "text/html";
			ctx.Response.OutputStream.Write(data, 0, data.Length);
			ctx.Response.OutputStream.Close();

			return Task.FromResult(0);
		}

		public Task PutHandler(HttpHandler httpHandler, HttpListenerContext ctx)
		{
			throw new NotSupportedException ();
		}

		public Task DeleteHandler(HttpHandler httpHandler, HttpListenerContext ctx)
		{
			throw new NotSupportedException ();
		}

		public Task PatchHandler(HttpHandler httpHandler, HttpListenerContext ctx)
		{
			throw new NotSupportedException ();
		}
	}
}

