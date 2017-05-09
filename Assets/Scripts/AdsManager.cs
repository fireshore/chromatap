// Project:			Fireshore Tools
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using UnityEngine.Advertisements;

namespace Chromatap
{
	/// <summary>
	/// Handles showing ads for the player.
	/// </summary>

	public class AdsManager : MonoBehaviour
	{
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
			Events.onWatchAdRequest += OnWatchAdRequest;
		}



		private void RemoveListeners ()
		{
			Events.onWatchAdRequest -= OnWatchAdRequest;
		}



		private void OnWatchAdRequest ()
		{
			if (Advertisement.IsReady("rewardedVideo"))
			{
				// Show ad, pass the result to method below.
				var options = new ShowOptions { resultCallback = VideoAdHandler };
				Advertisement.Show("rewardedVideo", options);
			}
		}

		#endregion



		#region Private Methods

		/// <summary>
		/// Handles the result of the displayed rewarded video ad.
		/// </summary>
		private void VideoAdHandler (ShowResult result)
		{
			switch (result)
			{
				case ShowResult.Finished:
					Events.WatchAdCompleted(); // Call the event to revive the player and restart gameplay from previous score
					Debug.Log("Video ad was watched sucessfully.");
					break;
				case ShowResult.Skipped:
					Debug.Log("Video ad was skipped.");
					break;
				case ShowResult.Failed:
					Debug.Log("Video ad failed to play.");
					break;
			}
		}

		#endregion
	}

}