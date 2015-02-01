using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;

namespace $safeprojectname$
{
	public class Plugin:Application.Plugins.Plugin
	{
		 public Plugin(string assemblyPath)
			: base(assemblyPath)
		{
			//Add constructor logic here
		}
	}
}
