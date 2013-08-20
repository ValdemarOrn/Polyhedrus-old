using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.UI
{
	public static class Extensions
	{
		public static int IndexOf<T>(this IEnumerable<T> list, Func<T, bool> expr)
		{
			int i = 0;
			foreach(var elem in list)
			{
				if (expr(elem) == true)
					return i;

				i++;
			}

			return -1;
		}
	}
}
