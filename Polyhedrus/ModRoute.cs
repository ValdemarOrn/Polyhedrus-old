using Polyhedrus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public struct ModRoute
	{
		public int Index;

		public ModSource Source;
		public ModDestination Destination;

		public ModSource Control;

		public double ControlAmount;
		public double Amount;

		public bool Enabled;
	}
}
