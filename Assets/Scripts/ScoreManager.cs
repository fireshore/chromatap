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
	/// Handles all updating and saving of the player score.
	/// </summary>

	public class ScoreManager : MonoBehaviour
	{
		#region Unity and Events

		private void Awake ()
		{
			SetupListeners();

			PlayerScore.Load();
		}



		private void OnDestroy ()
		{
			RemoveListeners();
		}



		private void SetupListeners ()
		{
			Events.onGameplayStart += OnGameplayStart;
			Events.onCircleCompleted += OnCircleCompleted;
			Events.onCircleFailed += OnCircleFailed;
		}



		private void RemoveListeners ()
		{
			Events.onGameplayStart -= OnGameplayStart;
			Events.onCircleCompleted -= OnCircleCompleted;
			Events.onCircleFailed -= OnCircleFailed;
		}



		private void OnGameplayStart (bool revived)
		{
			// Do not reset score if revived by watching an ad
			if (!revived)
			{
				PlayerScore.Score = 0f;
			}
		}



		private void OnCircleCompleted (float score)
		{
			float s = Mathf.Round(score * 100) / 100f;

			// Update score.
			PlayerScore.Score += s;
			Events.ScoreUpdate(PlayerScore.Score);
		}



		private void OnCircleFailed (FailReason reason)
		{
			Events.GameplayEnded(PlayerScore.Score, (PlayerScore.Score > PlayerScore.Highscore), reason);

			// Update highscore, save it.
			if (PlayerScore.Score > PlayerScore.Highscore)
			{
				PlayerScore.Highscore = PlayerScore.Score;
				PlayerScore.Save();
			}
		}

		#endregion

	}

}