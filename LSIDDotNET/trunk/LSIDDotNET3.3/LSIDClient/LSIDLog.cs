using System;

namespace LSIDClient
{
	/// <summary>
	/// Summary description for LSIDLog.
	/// </summary>
	public class LSIDLog
	{
		public static void LogMessage(string msg)
		{
			try
			{
				msg = DateTime.Now.ToString() + " : " + msg;
				String f = Global.BinDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["LogFile"];
				System.IO.StreamWriter wr = System.IO.File.AppendText(f);
				wr.WriteLine(msg);
				wr.Close();
			}
			catch(Exception ex)
			{
				try
				{
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, ex.Message	); 
					System.Diagnostics.EventLog.WriteEntry(LSIDException.EXCEPTION_EVENT_SOURCE, msg); 
				}
				catch(Exception)				
				{
				}
			}
		}
	}
}
