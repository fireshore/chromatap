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
	/// Handles toggling device vibration.
	/// </summary>

	public class VibrationManager : MonoBehaviour
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
		}



		private void RemoveListeners ()
		{
			Events.onCircleCompleted -= OnCircleCompleted;
			Events.onCircleFailed -= OnCircleFailed;
		}



		private void OnCircleCompleted (float score)
		{
			// Vibrate for better than average scores only.
			if (Options.Vibration && score > Values.ScoreThresholdMiddle)
			{
				Handheld.Vibrate();
			}
		}



		private void OnCircleFailed (FailReason reason)
		{
			if (Options.Vibration)
			{
				Handheld.Vibrate();
			}
		}

		#endregion
	}

}