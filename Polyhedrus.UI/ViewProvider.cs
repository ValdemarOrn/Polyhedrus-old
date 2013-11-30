using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.UI
{
	public static class ViewProvider
	{
		private static Dictionary<Type, Type> Views;

		private static void FindAll()
		{
			Views = new Dictionary<Type, Type>();

			var views = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(SynthModuleView).IsAssignableFrom(x))
				.Where(x => x.GetCustomAttributes(typeof(ViewProviderForAttribute), true).Any())
				.ToList();

			foreach(var view in views)
			{
				var types = view.GetCustomAttributes(typeof(ViewProviderForAttribute), true).Cast<ViewProviderForAttribute>().Select(x => x.Type).ToArray();
				foreach (var type in types)
					Views[type] = view;
			}
		}

		public static SynthModuleView GetView(Type type, SynthController ctrl, ModuleId moduleId)
		{
			if (Views == null)
				FindAll();

			Type viewType;
			Views.TryGetValue(type, out viewType);

			if (viewType == null)
				return null;

			var constructur = viewType.GetConstructor(new[] { typeof(SynthController), typeof(ModuleId) });
			var view = constructur.Invoke(new object[] { ctrl, moduleId });
			return view as SynthModuleView;
		}


	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ViewProviderForAttribute : Attribute
	{
		public Type Type { get; set; }

		public ViewProviderForAttribute(Type type)
		{
			Type = type;
		}
	}
}
