using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class ParameterMap
	{
		public class ParameterMapEntry
		{
			public ModuleId Module;
			public Type ModuleType;
			public int Key;
			public object Value;
		}

		public Dictionary<ModuleKey, Dictionary<int, object>> Parameters { get; private set; }

		public ParameterMap()
		{
			Parameters = new Dictionary<ModuleKey, Dictionary<int, object>>();
		}

		public void SetParameters(ModuleId moduleId, Type moduleType, Dictionary<int, object> parameters)
		{
			Parameters[new ModuleKey(moduleId, moduleType)] = parameters;
		}

		public void SetParameter(ModuleId moduleId, Type moduleType, int parameter, object value)
		{
			var para = GetParameters(moduleId, moduleType);
			if (para == null)
				Parameters[new ModuleKey(moduleId, moduleType)] = new Dictionary<int, object>();

			para[parameter] = value;
		}

		public Dictionary<int, object> GetParameters(ModuleId moduleId, Type moduleType)
		{
			var key = new ModuleKey(moduleId, moduleType);
			Dictionary<int, object> output;
			bool ok = Parameters.TryGetValue(key, out output);
			return ok ? output : null;
		}

		public object GetParameter(ModuleId moduleId, Type moduleType, int parameter)
		{
			var para = GetParameters(moduleId, moduleType);
			if (para == null)
				return null;

			object output;
			para.TryGetValue(parameter, out output);
			return output;
		}

		public List<ParameterMapEntry> GetAllParameters()
		{
			var list = new List<ParameterMapEntry>();

			foreach(var module in Parameters)
			{
				foreach (var para in module.Value)
				{
					list.Add(new ParameterMapEntry
					{
						Key = para.Key,
						Module = module.Key.ModuleId,
						ModuleType = module.Key.ModuleType,
						Value = para.Value
					});
				}
			}

			return list;
		}
	}
}
