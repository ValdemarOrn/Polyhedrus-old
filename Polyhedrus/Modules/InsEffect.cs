using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public static class InsEffect
	{
		public static Type[] Types
		{
			get { return new[] { typeof(InsDistortion), typeof(InsRedux) }; }
		}
	}
}
