// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:								Yes, using PlayerPrefs is dumb for production, but works well for prototyping
//

using UnityEngine;
using Fireshore;

namespace Chromatap
{
	/// <summary>
	/// Storage for player options.
	/// </summary>

	public static class Options
	{
		#region Fields

		// Public:
		public static bool Sound = true;
		public static bool Vibration = true;

		#endregion



		#region Public Methods

		public static void Save ()
		{
			PlayerPrefs.SetInt("Option_Sound", FireTools.BoolToInt(Sound));
			PlayerPrefs.SetInt("Option_Vibration", FireTools.BoolToInt(Vibration));
			PlayerPrefs.Save();
		}



		public static void Load ()
		{
			Sound = FireTools.IntToBool(PlayerPrefs.GetInt("Option_Sound", 1));
			Vibration = FireTools.IntToBool(PlayerPrefs.GetInt("Option_Vibration", 1));
		}

		#endregion
	}
}
