using System;
using System.Collections.Generic;

namespace SimpleWebServer
{
	public class NamespaceNodeInfo
	{
		private readonly Dictionary<string, NamespaceNodeInfo> _children = new Dictionary<string, NamespaceNodeInfo>();
		private ContentService _service;

		public NamespaceNodeInfo()
		{

		}

		public NamespaceNodeInfo(string name)
		{
			this.Name = name;
		}

		public string Name { get; private set; }
		public IDictionary<string, NamespaceNodeInfo> Children { get { return _children; } }

		public ContentService Service
		{
			get { return _service; }
			set
			{
				if (value != null && _service != null)
				{
					throw new Exception("Service already set");
				}
				_service = value;
			}
		}
	}
}

