using Polyhedrus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class ModuleNames
	{
		public static Dictionary<Type, string> Names = new Dictionary<Type, string>()
		{
			{ typeof(InsRedux), "Redux" },
			{ typeof(InsDistortion), "Saturation" }
		};

		public static string GetName(Type moduleType)
		{
			string name = string.Empty;
			Names.TryGetValue(moduleType, out name);
			return name;
		}

		public static Type GetType(string moduleName)
		{
			if (Names.ContainsValue(moduleName))
				return Names.FirstOrDefault(x => x.Value == moduleName).Key;
			else
				return null;
		}
	}
}
