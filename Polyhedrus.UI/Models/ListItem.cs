using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.UI.Models
{
	public class ListItem<T>
	{
		public T Value;
		public string Text;

		public ListItem(T value, string text)
		{
			Value = value;
			Text = text;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
