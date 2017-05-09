// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

namespace Chromatap
{
	/// <summary>
	/// Handles connection to Unity Analytics.
	/// </summary>

	public class AnalyticsManager : MonoBehaviour
	{
		#region Fields

		// Reused dictionary
		public static Dictionary<string, object> dict = new Dictionary<string, object>();

		#endregion



		#region Unity and Events

		private void Awake ()
		{
			SetupListeners();
		}



		private void OnDestroy ()
		{
			RemoveListeners();
		}



		private void SetupListeners ()
		{
			Events.onGameplayEnded += OnGameplayEnded;
		}



		private void RemoveListeners ()
		{
			Events.onGameplayEnded -= OnGameplayEnded;
		}



		private void OnGameplayEnded (float finalScore, bool newHighscore, FailReason reason)
		{
			dict = new Dictionary<string, object>();

			switch (reason)
			{
				case FailReason.DontTap:
					dict["reason"] = "DontTap";
					break;
				case FailReason.ExpandedTooFar:
					dict["reason"] = "ExpandedTooFar";
					break;
				case FailReason.ExpandedTooLittle:
					dict["reason"] = "ExpandedTooLittle";
					break;
				case FailReason.TimeRanOut:
					dict["reason"] = "TimeRanOut";
					break;
			}

			dict["score"] = PlayerScore.Score;
			dict["newHighscore"] = newHighscore;
			dict["highscore"] = PlayerScore.Highscore;
			dict["totalGames"] = PlayerStats.TotalGames;
			dict["totalPointsGained"] = PlayerStats.TotalPointsGained;
			dict["videoAdsWatched"] = PlayerStats.VideoAdsWatched;

			Analytics.CustomEvent("GameplayEnded", dict);
		}

		#endregion
	}

}