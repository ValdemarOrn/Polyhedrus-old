using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Polyhedrus.UI
{
	public class SynthModuleView : UserControl, INotifyPropertyChanged
	{
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
