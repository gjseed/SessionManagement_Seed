using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace SessionManagement
{
	public class Coder : IHttpModule
	{
		//a dictionary to help out
		Dictionary<string, string> vdt = new Dictionary<string, string>();
		public void Dispose()
		{
			// nothing to dispose 
		}

		public void OnBeginRequest(object sender, EventArgs e)
		{
			var context = ((HttpApplication)sender).Context;
			var URL = context.Request.Path;
			string userID = context.Request.QueryString["userid"];
			string Active = context.Request.QueryString["active"];
			string eUserid = string.Empty;
			string eactive = string.Empty;
			//so nothing will happen on default
			if (userID != null)
			{
				//checks if the current encrypted value exists
				if (!vdt.ContainsKey(eUserid))
				{
					eUserid = Encrypt(userID);
					eactive = Encrypt(Active);
					vdt.Add(eUserid, userID);
					vdt.Add(eactive, Active);
				}
			}
			//second check for other things
			if (!string.IsNullOrEmpty(userID))
			{
				//to be able to go to the third page
				if (URL.Contains("NewPage") && vdt.Count > 4)
				{
					URL = "MembersMain.aspx";
				}
				//putting the encrypted string into url 
				context.RewritePath(URL, string.Empty, "userid=" + eUserid + "&active=" + Active, true);
				//this is to check if the web page has changed and/or the browser url
				if (vdt.ContainsKey(userID) && vdt.ContainsValue(userID))
				{
					context.Response.Write(vdt[userID]);
					context.Response.Write(vdt[eUserid]);

					context.RewritePath(URL, string.Empty, "userid=" + vdt[userID] + "&active=" + vdt[Active], true);
				}
				//this is the basic change of the url to the encrypted value
				else if (vdt.ContainsValue(userID))
				{
					context.Response.Redirect(context.Request.Url.ToString());
				}
			}

		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(OnBeginRequest);

		}
		//actuall encrytion with salt of random number
		public string Encrypt(string info)
		{
			Random r = new Random(DateTime.Now.Millisecond);
			Thread.Sleep(1);
			string salt = r.Next(100000, 999999).ToString();
			int a;
			byte[] dataArray;
			byte[] result;
			dataArray = ASCIIEncoding.ASCII.GetBytes(info);
			SHA256 myresult = SHA256.Create();
			result = myresult.ComputeHash(dataArray);
			StringBuilder output = new StringBuilder(result.Length);
			for (a = 0; a < result.Length; a++)
			{
				output.Append(result[a].ToString("X2"));
			}
			return output.ToString().Substring(0, 6) + salt + output.ToString().Substring(6);
		}
	}
}
