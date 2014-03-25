using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public static class ModuleType
	{
		static ModuleType()
		{
			// finds all classes with a ModuleName attribute
			// registers their name and type

			var classes = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetExportedTypes())
				.Where(x => x.GetCustomAttributes(typeof (ModuleNameAttribute), true).Length > 0);

			var insTypes = new List<Type>();
			var oscTypes = new List<Type>();

			foreach (var cls in classes)
			{
				var nameAttribute = cls
					.GetCustomAttributes(typeof(ModuleNameAttribute), true)
					.FirstOrDefault() as ModuleNameAttribute;

				var name = nameAttribute == null ? cls.Name : nameAttribute.Name;
				moduleNames[name] = cls;

				if (typeof (IInsEffect).IsAssignableFrom(cls))
					insTypes.Add(cls);

				if (typeof(IOscillator).IsAssignableFrom(cls))
					oscTypes.Add(cls);
			}

			InsertEffectTypes = insTypes.ToArray();
			OscillatorTypes = oscTypes.ToArray();
		}

		private static Dictionary<string, Type> moduleNames = new Dictionary<string, Type>();

		public static Type[] InsertEffectTypes { get; private set; }
		public static Type[] OscillatorTypes { get; private set; }

		public static T CreateNew<T>(Type moduleType, double samplerate, int bufferSize) where T : class
		{
			var constructor = moduleType.GetConstructor(new[] { typeof(double), typeof(int) });
			if (constructor == null)
				return default(T);

			var instance = constructor.Invoke(new object[] { samplerate, bufferSize });
			return instance as T;
		}

		public static string GetName(Type moduleType)
		{
			return moduleNames.Any(x => x.Value == moduleType)
				? moduleNames.First(x => x.Value == moduleType).Key
				: null;
		}

		public static Type GetType(string moduleName)
		{
			Type t;
			moduleNames.TryGetValue(moduleName, out t);
			return t;
		}
	}

	public class ModuleNameAttribute : Attribute
	{
		public string Name { get; private set; }

		public ModuleNameAttribute(string name)
		{
			Name = name;
		}
	}
}
