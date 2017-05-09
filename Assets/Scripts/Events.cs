// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

namespace Chromatap
{
	/// <summary>
	/// Contains all delegate definitions and events for cross-script events in the game.
	/// </summary>

	public static class Events
	{

		// Gameplay is loaded and begins.
		public delegate void GameplayStartHandler (bool revived);
		public static event GameplayStartHandler onGameplayStart;
		public static void GameplayStart (bool revived) { if (onGameplayStart != null) onGameplayStart(revived); }



		// A new tap and hold circle was started.
		public delegate void TapAndHoldStartedHandler (float duration);
		public static event TapAndHoldStartedHandler onTapAndHoldStarted;
		public static void TapAndHoldStarted (float duration) { if (onTapAndHoldStarted != null) onTapAndHoldStarted(duration); }



		// A new tap and hold circle was started.
		public delegate void DontTapStartedHandler (float duration);
		public static event DontTapStartedHandler onDontTapStarted;
		public static void DontTapStarted (float duration) { if (onDontTapStarted != null) onDontTapStarted(duration); }



		// A circle was completed without failure.
		public delegate void CircleCompletedHandler (float score);
		public static event CircleCompletedHandler onCircleCompleted;
		public static void CircleCompleted (float score) { if (onCircleCompleted != null) onCircleCompleted(score); }



		// A Dont-Tap circle was completed.
		public delegate void DontTapCompletedHandler ();
		public static event DontTapCompletedHandler onDontTapCompleted;
		public static void DontTapCompleted () { if (onDontTapCompleted != null) onDontTapCompleted(); }



		// Small circle size was updated.
		public delegate void SmallCircleSizeUpdateHandler (float scale);
		public static event SmallCircleSizeUpdateHandler onSmallCircleSizeUpdate;
		public static void SmallCircleSizeUpdate (float value) { if (onSmallCircleSizeUpdate != null) onSmallCircleSizeUpdate(value); }



		// Big circle size was updated.
		public delegate void BigCircleSizeUpdateHandler (float scale);
		public static event BigCircleSizeUpdateHandler onBigCircleSizeUpdate;
		public static void BigCircleSizeUpdate (float value) { if (onBigCircleSizeUpdate != null) onBigCircleSizeUpdate(value); }



		// Minimum circle size was updated.
		public delegate void MinimumCircleSizeUpdateHandler (float scale);
		public static event MinimumCircleSizeUpdateHandler onMinimumCircleSizeUpdate;
		public static void MinimumCircleSizeUpdate (float value) { if (onMinimumCircleSizeUpdate != null) onMinimumCircleSizeUpdate(value); }



		// The player failed a circle.
		public delegate void CircleFailedHandler (FailReason reason);
		public static event CircleFailedHandler onCircleFailed;
		public static void CircleFailed (FailReason reason) { if (onCircleFailed != null) onCircleFailed(reason); }



		// The player failed the streak and gameplay ended.
		public delegate void GameplayEndedHandler (float finalScore, bool newHighscore, FailReason reason);
		public static event GameplayEndedHandler onGameplayEnded;
		public static void GameplayEnded (float finalScore, bool newHighscore, FailReason reason) { if (onGameplayEnded != null) onGameplayEnded(finalScore, newHighscore, reason); }



		// The player's current score was updated.
		public delegate void ScoreUpdateHandler (float score);
		public static event ScoreUpdateHandler onScoreUpdate;
		public static void ScoreUpdate (float score) { if (onScoreUpdate != null) onScoreUpdate(score); }



		// The player wants to restart the gameplay from the beginning.
		public delegate void RestartRequestHandler ();
		public static event RestartRequestHandler onRestartRequest;
		public static void RestartRequest () { if (onRestartRequest != null) onRestartRequest(); }



		// The player wants to revive by watching a rewarded ad video.
		public delegate void WatchAdRequestHandler ();
		public static event WatchAdRequestHandler onWatchAdRequest;
		public static void WatchAdRequest () { if (onWatchAdRequest != null) onWatchAdRequest(); }



		// The player successfully watched a rewarded ad video.
		public delegate void WatchAdCompletedHandler ();
		public static event WatchAdCompletedHandler onWatchAdCompleted;
		public static void WatchAdCompleted () { if (onWatchAdCompleted != null) onWatchAdCompleted(); }



		// Update the size of the heart indicator (0-1).
		public delegate void HeartSizeUpdateHandler (float size, float lastValue);
		public static event HeartSizeUpdateHandler onHeartSizeUpdate;
		public static float HeartSizeUpdate_LastValue = 0f;
		public static void HeartSizeUpdate (float size)
		{
			if (onHeartSizeUpdate != null && HeartSizeUpdate_LastValue != size)
			{
				onHeartSizeUpdate(size, HeartSizeUpdate_LastValue);
				HeartSizeUpdate_LastValue = size;
			}
		}



		// Player was saved from by a full heart.
		public delegate void SavedByHeartHandler ();
		public static event SavedByHeartHandler onSavedByHeart;
		public static void SavedByHeart () { if (onSavedByHeart != null) onSavedByHeart(); }



		// The player requests to return to the main menu.
		public delegate void MainMenuRequestHandler ();
		public static event MainMenuRequestHandler onMainMenuRequest;
		public static void MainMenuRequest () { if (onMainMenuRequest != null) onMainMenuRequest(); }



		// The player requests to start the gameplay scene.
		public delegate void GameplaySceneRequestHandler ();
		public static event GameplaySceneRequestHandler onGameplaySceneRequest;
		public static void GameplaySceneRequest () { if (onGameplaySceneRequest != null) onGameplaySceneRequest(); }
	}

}