// Project:			Fireshore Tools
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections;
using System;

namespace Fireshore
{
	/// <summary>
	/// Often-used utility tools.
	/// </summary>

	public static class FireTools
	{
		#region Public Methods

		/// <summary>
		/// Given a bool, turns it into an int. True = 1, false = 0.
		/// </summary>
		public static int BoolToInt (bool b)
		{
			if (b) return 1;
			else return 0;
		}



		/// <summary>
		/// Given an int, turns it into a bool. True if higher than 0, false otherwise.
		/// </summary>
		public static bool IntToBool (int i)
		{
			if (i > 0) return true;
			else return false;
		}



		/// <summary>
		/// Formats time in seconds into HH:MM:SS format.
		/// </summary>
		public static string TimeToString (int seconds)
		{
			TimeSpan time = TimeSpan.FromSeconds(seconds);

			string _hours = "" + time.Hours;
			if (_hours.Length < 2)
				_hours = "0" + _hours;

			string _min = "" + time.Minutes;
			if (_min.Length < 2)
				_min = "0" + _min;

			string _sec = "" + time.Seconds;
			if (_sec.Length < 2)
				_sec = "0" + _sec;

			return _hours + ":" + _min + ":" + _sec;
		}



		/// <summary>
		/// Clamps an angle between two others, taking 0=360 into account.
		/// </summary>
		public static float ClampAngle (float angle, float from, float to)
		{
			if (angle > 180) angle = 360 - angle;
			angle = Mathf.Clamp(angle, from, to);
			if (angle < 0) angle = 360 + angle;

			return angle;
		}



		// http://wiki.unity3d.com/index.php?title=MD5
		/// <summary>
		/// Md5Sum hash.
		/// </summary>
		public static string Md5Sum (string strToHash)
		{
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
			byte[] bytes = ue.GetBytes(strToHash);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++)
			{
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}

		#endregion
	}

}