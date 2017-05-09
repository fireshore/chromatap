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
	/// Handles playing sound effets at correct times.
	/// </summary>

	public class SFXManager : MonoBehaviour
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
			Events.onCircleCompleted += OnCircleCompleted;
			Events.onCircleFailed += OnCircleFailed;
			Events.onHeartSizeUpdate += OnHeartSizeUpdate;
		}



		private void RemoveListeners ()
		{
			Events.onCircleCompleted -= OnCircleCompleted;
			Events.onCircleFailed -= OnCircleFailed;
			Events.onHeartSizeUpdate -= OnHeartSizeUpdate;
		}



		private void OnCircleCompleted (float score)
		{
			// Play sound depending on score.
			if (score < Values.ScoreThresholdMiddle)
				SoundPlayer.Play(Sound.Bad);
			else if (score < Values.ScoreThresholdHighest)
				SoundPlayer.Play(Sound.Good);
			else
				SoundPlayer.Play(Sound.Perfect);
		}



		private void OnCircleFailed (FailReason reason)
		{
			SoundPlayer.Play(Sound.Failed);
		}



		private void OnHeartSizeUpdate (float size, float lastValue)
		{
			// If heart breaks.
			if (size <= 0f && lastValue >= 1f)
			{
				SoundPlayer.Play(Sound.Heartbroken);
			}
			// If heart is filled.
			else if (size >= 1f)
			{
				SoundPlayer.Play(Sound.HeartFull);
			}
		}

		#endregion
	}

}