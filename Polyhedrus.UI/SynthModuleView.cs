using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Polyhedrus.UI
{
	public class SynthModuleView : UserControl, INotifyPropertyChanged
	{
		private static Dictionary<DependencyObject, SynthModuleView> Parents = new Dictionary<DependencyObject, SynthModuleView>();

		public static SynthModuleView FindParentSynthModule(DependencyObject child)
		{
			if (Parents.ContainsKey(child))
				return Parents[child];

			DependencyObject current = child;

			while (current != null)
			{
				if (current is SynthModuleView)
				{
					Parents[child] = current as SynthModuleView;
					return current as SynthModuleView;
				}

				current = System.Windows.Media.VisualTreeHelper.GetParent(current);
			}

			return null;
		}

		public SynthController Ctrl { get; set; }
		public ModuleParams ModuleId { get; set; }

		#region Notify Change

		public event PropertyChangedEventHandler PropertyChanged;


		// I used this and GetPropertyName to avoid having to hard-code property names
		// into the NotifyChange events. This makes the application much easier to refactor
		// leter on, if needed.
		protected void NotifyChange<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			var name = GetPropertyName(exp);
			NotifyChange(name);
		}

		protected void NotifyChange(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
		}

		protected static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
		}

		#endregion
	}
}
