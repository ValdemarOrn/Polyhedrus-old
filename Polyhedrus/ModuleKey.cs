using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public struct ModuleKey
	{
		public ModuleId ModuleId;
		public Type ModuleType;

		public int Key
		{
			get
			{
				return (int)ModuleId | ModuleType.GetHashCode() << 2 ;
			}
		}

		public ModuleKey(ModuleId moduleId, Type moduleType)
		{
			ModuleId = moduleId;
			ModuleType = moduleType;
		}

		public override int GetHashCode()
		{
			return Key;
		}

		public override bool Equals(object obj)
		{
			return (obj is ModuleKey) && Key == ((ModuleKey)obj).Key;
		}

		public override string ToString()
		{
			return ModuleId.ToString() + " - " + ModuleType.Name;
		}
	}
}
