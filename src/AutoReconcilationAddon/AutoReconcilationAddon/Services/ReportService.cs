using System;
using System.IO;

namespace AutoReconcilationAddon.Services
{
	public class ReportService
	{
		public void ReportError(string cardCode, string linkedCardCode, Exception e, string xml)
		{
			string logDirectory = "Logs";

			if (!Directory.Exists(logDirectory))
			{
				Directory.CreateDirectory(logDirectory);
			}

			string fileName = linkedCardCode == ""
				? $"Log_{cardCode}_{DateTime.Now:MM.dd.yyyy HH.mm}.txt"
				: $"Log_{cardCode}_{linkedCardCode}_{DateTime.Now:MM.dd.yyyy HH.mm}.txt";

			string filePath = Path.Combine(logDirectory, fileName);

			string contents = linkedCardCode == ""
				? $"Report on Error in AutoReconciliation Addon on {DateTime.Now:MM/dd/yyyy HH/mm}\n\nBusiness Partner CardCode -> [{cardCode}]\n\nError details:\n{e}\n\nXML of Open Transactions:\n{xml}"
				: $"Report on Error (Linked Account) in AutoReconciliation Addon on {DateTime.Now:MM/dd/yyyy HH/mm}\n\nBusiness Partner CardCode -> [{cardCode}]\nLinked Business Partner CardCode -> [{linkedCardCode}]\n\nError details:\n{e}\n\nXML of Open Transactions:\n{xml}";

			File.WriteAllText(filePath, contents);
		}
	}
}
