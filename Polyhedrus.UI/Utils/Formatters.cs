using AudioLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Polyhedrus.UI.Utils
{
	public class Formatters
	{
		public static string FormatEnvelope(double val)
		{
			var value = ValueTables.Get(val, ValueTables.Response4Dec) * 20000;

			if (value < 10)
				return String.Format(CultureInfo.InvariantCulture, "{0:0.00}ms", value);
			else if (value < 100)
				return String.Format(CultureInfo.InvariantCulture, "{0:0.0}ms", value);
			else if (value < 1000)
				return String.Format(CultureInfo.InvariantCulture, "{0:0}ms", value);
			else
				return String.Format(CultureInfo.InvariantCulture, "{0:0.00}s", value * 0.001);
		}
	}
}
