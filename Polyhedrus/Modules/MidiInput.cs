using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public sealed class MidiInput
	{
		public bool Gate;
		public double Velocity, Note, PitchBend, ModWheel;
	}
}
