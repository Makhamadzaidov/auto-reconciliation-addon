using System;
using System.IO;

namespace AutoReconcilationAddon.Services
{
	public class ReportService
	{
		public void ReportError(string cardCode, string linkedCardCode, Exception e, string xml)
		{
			if (linkedCardCode == "")
			{
				DateTime now = DateTime.Now;
				string contents = "Report on Error in AutoReconciliation Addon on " + now.ToString("MM/dd/yyyy HH/mm") + "\n\n" + "Business Partner CardCode -> [" + cardCode + "]\n\n" + "Error details:\n" + e.ToString() + "\n\n" + "XML of Open Transactions:\n" + xml;
				string[] strArray = new string[5]
				{
		  "Logs/Log",
		  null,
		  null,
		  null,
		  null
				};
				now = DateTime.Now;
				strArray[1] = now.ToString("MM/dd/yyyy HH/mm");
				strArray[2] = "_";
				strArray[3] = cardCode;
				strArray[4] = ".txt";
				File.WriteAllText(string.Concat(strArray), contents);
			}
			else
			{
				DateTime now = DateTime.Now;
				string contents = "Report on Error (Linked Account) in AutoReconciliation Addon on " + now.ToString("MM/dd/yyyy HH/mm") + "\n\n" + "Business Partner CardCode -> [" + cardCode + "]\n" + "Linked Business Partner CardCode -> [" + linkedCardCode + "]\n\n" + "Error details:\n" + e.ToString() + "\n\n" + "XML of Open Transactions:\n" + xml;
				string[] strArray = new string[5]
				{
		  "Logs/Log",
		  null,
		  null,
		  null,
		  null
				};
				now = DateTime.Now;
				strArray[1] = now.ToString("MM/dd/yyyy HH/mm");
				strArray[2] = "_";
				strArray[3] = cardCode;
				strArray[4] = ".txt";
				File.WriteAllText(string.Concat(strArray), contents);
			}
		}
	}
}
