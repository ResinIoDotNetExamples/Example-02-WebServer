using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    public class HttpHandler
    {
        private HttpListener _http;
        private readonly int _httpPort;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _running = false;
        private NamespaceNodeInfo _rootNode = new NamespaceNodeInfo();
		private Task _httpTask;

        public HttpHandler(int port)
        {
            _httpPort = port;
        }

        public void Start()
        {
            if (_running)
                return;
            _running = true;
            _httpTask = Task.Run(() => RunTask(), _cts.Token);
        }

		public void WaitForExit()
		{
			_httpTask.Wait ();
		}

        private async Task RunTask()
        {
            try
            {
                _http = new HttpListener();
                _http.Prefixes.Add(string.Format("http://*:{0}/", _httpPort));
                _http.Start();

                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var context = await _http.GetContextAsync();
                        try
                        {
                            await ProcessRequest(context);
                        }
                        catch (Exception exc)
                        {
                            // request processing exceptions should not break the listen loop
                            Console.WriteLine(exc);
                        }
                    }
                    catch (HttpListenerException)
                    {
                        // A dropped connection or other error in the underlying http conversation
						// Overlook this and try to carry on
                        if (_cts.IsCancellationRequested)
                            break;
                    }
                    catch (TaskCanceledException)
                    {
						// The server listening loop is exitted by raising the cancellation token
                        break;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                        //TODO: it is be bad to suppress all exceptions here because it could result in a tight loop. Set some sort of failure counter
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // not an error, just a signal to exit - return silently
            }            
        }

        public void Stop()
        {
            if (!_running)
                return;
            _running = false;

            _cts.Cancel();
            if (_http != null)
            {
                _http.Stop();
                _http = null;
            }
        }

        private async Task ProcessRequest(HttpListenerContext ctx)
        {
            string msg = string.Format("HTTP request from {2} on {3} : {0} {1}", ctx.Request.HttpMethod, ctx.Request.Url, ctx.Request.RemoteEndPoint, ctx.Request.LocalEndPoint);
            Console.WriteLine(msg);

            var url = ctx.Request.Url;
            var path = url.GetLeftPart(UriPartial.Path);
            var prefix = url.GetLeftPart(UriPartial.Authority);
            path = path.Remove(0, prefix.Length);
            var urlTokens = path.Split('/');
            NamespaceNodeInfo node;
            lock (_rootNode)
            {
                node = FindServiceNode(urlTokens);
            }
            if (node != null && node.Service!=null)
            {
                try
                {
                    await node.Service.ProcessRequest(this, ctx);
                }
                catch (NotImplementedException)
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    ctx.Response.AddHeader("Allow","GET PUT DELETE PATCH");

                    var sb = new StringBuilder();
                    sb.Append("<html><body><h1>Method not allowed</h1>");
                    sb.Append(string.Format("The requested http method '{0}' is not supported for Molarity services", ctx.Request.HttpMethod));
                    sb.Append("</body></html>");

                    var data = Encoding.UTF8.GetBytes(sb.ToString());
                    ctx.Response.ContentLength64 = data.Length;
                    ctx.Response.OutputStream.Write(data, 0, data.Length);
                    ctx.Response.OutputStream.Close();
                }
                return;
            }

            ctx.Response.StatusCode = (int) HttpStatusCode.NotFound;

            var sb404 = new StringBuilder();
            sb404.Append("<html><body><h1>" + "404 Service not found" + "</h1>");
            sb404.Append("The requested service or document could not be found");
            sb404.Append("</body></html>");

            var data404 = Encoding.UTF8.GetBytes(sb404.ToString());
            ctx.Response.ContentLength64 = data404.Length;
            ctx.Response.OutputStream.Write(data404, 0, data404.Length);
            ctx.Response.OutputStream.Close();
        }

        public void AddService(string path, IContentService service)
        {
            lock (_rootNode)
            {
                var urlTokens = path.Split('/');
                var node = FindServiceNode(urlTokens, true);
                if (node != null)
                {
                    node.Service = new ContentService(service);
                }
            }
            Console.WriteLine("Service added : {0}", path);
        }

        private NamespaceNodeInfo FindServiceNode(IEnumerable<string> urlTokens, bool fCreate = false)
        {
            if (urlTokens == null || !urlTokens.Any())
                return _rootNode;

            var tokens = new List<string>(urlTokens);

            var node = _rootNode;
            while (tokens.Count > 0)
            {
                var search = tokens[0].ToLowerInvariant();
                if (string.IsNullOrEmpty(search))
                {
                    tokens.RemoveAt(0);
                    continue;
                }

                if (node.Children.ContainsKey(search))
                {
                    node = node.Children[search];
                }
                else
                {
                    if (!fCreate)
                        return null;
                    var next = new NamespaceNodeInfo(search);
                    node.Children.Add(search, next);
                    node = next;
                }
                tokens.RemoveAt(0);
            }
            return node;
        }

    }
}
