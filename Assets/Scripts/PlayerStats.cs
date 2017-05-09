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
	/// Storage for player stats.
	/// </summary>

	public static class PlayerStats
	{
		#region Fields

		// Tracked stats:
		public static int CirclesCompleted;
		public static int DontTapCompleted;
		public static int CirclesFailed_ExpandedTooFar;
		public static int CirclesFailed_DontTap;
		public static int CirclesFailed_ExpandedTooLittle;
		public static int CirclesFailed_TimeRanOut;
		public static float TotalPointsGained;
		public static int TotalGames;
		public static int TimesSavedByHeart;
		public static int HeartsFilled;
		public static int VideoAdsWatched;



		// Calculated stats:
		public static float AverageScore { get { return TotalPointsGained / TotalGames; } }

		#endregion



		#region Public Methods

		/// <summary>
		/// Save all stats to device memory.
		/// </summary>
		public static void Save ()
		{
			PlayerPrefs.SetInt("Stats_CirclesCompleted", CirclesCompleted);
			PlayerPrefs.SetInt("Stats_DontTapCompleted", DontTapCompleted);
			PlayerPrefs.SetInt("Stats_CirclesFailed_ExpandedTooFar", CirclesFailed_ExpandedTooFar);
			PlayerPrefs.SetInt("Stats_CirclesFailed_DontTap", CirclesFailed_DontTap);
			PlayerPrefs.SetInt("Stats_CirclesFailed_ExpandedTooLittle", CirclesFailed_ExpandedTooLittle);
			PlayerPrefs.SetInt("Stats_CirclesFailed_TimeRanOut", CirclesFailed_TimeRanOut);
			PlayerPrefs.SetFloat("Stats_TotalPointsGained", TotalPointsGained);
			PlayerPrefs.SetInt("Stats_TotalGames", TotalGames);
			PlayerPrefs.SetInt("Stats_TimesSavedByHeart", TimesSavedByHeart);
			PlayerPrefs.SetInt("Stats_HeartsFilled", HeartsFilled);
			PlayerPrefs.SetInt("Stats_VideoAdsWatched", VideoAdsWatched);
		}



		/// <summary>
		/// Load all previously saved stats from device memory, or initialize them to 0 if no saved values are found.
		/// </summary>
		public static void Load ()
		{
			CirclesCompleted = PlayerPrefs.GetInt("Stats_CirclesCompleted", 0);
			DontTapCompleted = PlayerPrefs.GetInt("Stats_DontTapCompleted", 0);
			CirclesFailed_ExpandedTooFar = PlayerPrefs.GetInt("Stats_CirclesFailed_ExpandedTooFar", 0);
			CirclesFailed_DontTap = PlayerPrefs.GetInt("Stats_CirclesFailed_DontTap", 0);
			CirclesFailed_ExpandedTooLittle = PlayerPrefs.GetInt("Stats_CirclesFailed_ExpandedTooLittle", 0);
			CirclesFailed_TimeRanOut = PlayerPrefs.GetInt("Stats_CirclesFailed_TimeRanOut", 0);
			TotalPointsGained = PlayerPrefs.GetFloat("Stats_TotalPointsGained");
			TotalGames = PlayerPrefs.GetInt("Stats_TotalGames", 0);
			TimesSavedByHeart = PlayerPrefs.GetInt("Stats_TimesSavedByHeart", 0);
			HeartsFilled = PlayerPrefs.GetInt("Stats_HeartsFilled", 0);
			VideoAdsWatched = PlayerPrefs.GetInt("Stats_VideoAdsWatched", 0);
		}

		#endregion
	}

}