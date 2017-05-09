// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:								Yes, using PlayerPrefs is dumb for production, but works well for prototyping
//

using UnityEngine;

namespace Chromatap
{
	/// <summary>
	/// Storage for player score.
	/// </summary>

	public static class PlayerScore
	{
		#region Fields

		// Private:
		private static float _score = 0f;
		private static float _highscore = 0f;



		// Properties:
		public static float Score
		{
			get { return _score; }
			set { if (value >= 0) _score = value; }
		}
		public static float Highscore
		{
			get { return _highscore; }
			set { if (value >= 0) _highscore = value; }
		}

		#endregion



		#region Public Methods
		
		public static void Save ()
		{
			PlayerPrefs.SetFloat("Highscore", _highscore);
			PlayerPrefs.Save();
		}



		public static void Load ()
		{
			_highscore = PlayerPrefs.GetFloat("Highscore", 0);
		}

		#endregion
	}

}