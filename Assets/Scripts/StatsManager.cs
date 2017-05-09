// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;

namespace Chromatap
{
	/// <summary>
	/// Handles updating/saving all player stats.
	/// </summary>

	public class StatsManager : MonoBehaviour
	{
		#region Unity and Events

		private void Awake ()
		{
			PlayerStats.Load();

			SetupListeners();
		}



		private void OnDestroy ()
		{
			PlayerStats.Save();             // Just to be safe.

			RemoveListeners();
		}



		private void SetupListeners ()
		{
			Events.onCircleCompleted += OnCircleCompleted;
			Events.onDontTapCompleted += OnDontTapCompleted;
			Events.onCircleFailed += OnCircleFailed;
			Events.onGameplayStart += OnGameplayStart;
			Events.onSavedByHeart += OnSavedByHeart;
			Events.onHeartSizeUpdate += OnHeartSizeUpdate;
			Events.onWatchAdCompleted += OnWatchAdCompleted;
		}



		private void RemoveListeners ()
		{
			Events.onCircleCompleted -= OnCircleCompleted;
			Events.onDontTapCompleted -= OnDontTapCompleted;
			Events.onCircleFailed -= OnCircleFailed;
			Events.onGameplayStart -= OnGameplayStart;
			Events.onSavedByHeart -= OnSavedByHeart;
			Events.onHeartSizeUpdate -= OnHeartSizeUpdate;
			Events.onWatchAdCompleted -= OnWatchAdCompleted;
		}



		private void OnCircleCompleted (float score)
		{
			PlayerStats.CirclesCompleted++;
			PlayerStats.TotalPointsGained += score;

			PlayerStats.Save();
		}



		private void OnDontTapCompleted ()
		{
			PlayerStats.DontTapCompleted++;

			PlayerStats.Save();
		}



		private void OnCircleFailed (FailReason reason)
		{
			switch (reason)
			{
				case FailReason.DontTap:
					PlayerStats.CirclesFailed_DontTap++;
					break;
				case FailReason.ExpandedTooFar:
					PlayerStats.CirclesFailed_ExpandedTooFar++;
					break;
				case FailReason.ExpandedTooLittle:
					PlayerStats.CirclesFailed_ExpandedTooLittle++;
					break;
				case FailReason.TimeRanOut:
					PlayerStats.CirclesFailed_TimeRanOut++;
					break;
			}

			PlayerStats.Save();
		}



		private void OnGameplayStart (bool revived)
		{
			if (revived) return;    // Don't count revives, since this stat is equal to the "VideoAdsWatched" stat.

			PlayerStats.TotalGames++;

			PlayerStats.Save();
		}



		private void OnSavedByHeart ()
		{
			PlayerStats.TimesSavedByHeart++;

			PlayerStats.Save();
		}



		private void OnHeartSizeUpdate (float size, float lastValue)
		{
			if (size == 1f && lastValue != 1f)      // Heart was just filled.
			{
				PlayerStats.HeartsFilled++;

				PlayerStats.Save();
			}
		}



		private void OnWatchAdCompleted ()
		{
			PlayerStats.VideoAdsWatched++;

			PlayerStats.Save();
		}

		#endregion
	}
}