// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Chromatap
{
	/// <summary>
	/// Updates, manages and receives input from the user interface.
	/// </summary>

	public class UIManager : MonoBehaviour
	{
		#region Fields

		// Exposed:
		[Header("Colors:")]
		[SerializeField, Tooltip("Color of the big circle.")]
		private Color bigColor;
		[SerializeField, Tooltip("Gradient that will be used to color the small, expanding circle.")]
		private Gradient smallGradient;
		[SerializeField, Tooltip("Warning gradient used on the big circle when the type is DontTap.")]
		private Gradient warningGradient;
		[SerializeField, Tooltip("The curve for how the color will be selected on the color gradients.")]
		private AnimationCurve colorChangeCurve;
		[SerializeField, Tooltip("Time it takes for the small circle to animate through an entire color iteration (flash rate).")]
		private float smallCircleAnimTime = 1f;
		[SerializeField, Tooltip("Time it takes for the big warning circle to animate through an entire color iteration (flash rate).")]
		private float warningAnimTime = 0.75f;

		[Header("Containers:")]
		[SerializeField, Tooltip("Container for hints and corresponding strings.")]
		private HintsContainer hintsContainer;

		[Header("UI References:")]
		[SerializeField, Tooltip("Contains references to elements in the gameplay interface.")]
		private GameplayContent gameplayUI;
		[SerializeField, Tooltip("Contains references to elements in the \"Failed\" menu screen.")]
		private FailedContent failedUI;

		

		// Private:
		private Vector3 tempVector = new Vector3(1, 1, 1);          // Used for temporary vector storage when setting UI sizes of circles.

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
			Events.onGameplayStart += OnGameplayStart;
			Events.onBigCircleSizeUpdate += OnBigCircleSizeUpdate;
			Events.onSmallCircleSizeUpdate += OnSmallCircleSizeUpdate;
			Events.onCircleCompleted += OnCircleCompleted;
			Events.onScoreUpdate += OnScoreUpdate;
			Events.onGameplayEnded += OnGameplayEnded;
			Events.onMinimumCircleSizeUpdate += OnMinimumCircleSizeUpdate;
			Events.onHeartSizeUpdate += OnHeartSizeUpdate;
		}



		private void RemoveListeners ()
		{
			Events.onGameplayStart -= OnGameplayStart;
			Events.onBigCircleSizeUpdate -= OnBigCircleSizeUpdate;
			Events.onSmallCircleSizeUpdate -= OnSmallCircleSizeUpdate;
			Events.onCircleCompleted -= OnCircleCompleted;
			Events.onScoreUpdate -= OnScoreUpdate;
			Events.onGameplayEnded -= OnGameplayEnded;
			Events.onMinimumCircleSizeUpdate -= OnMinimumCircleSizeUpdate;
			Events.onHeartSizeUpdate -= OnHeartSizeUpdate;
		}



		private void Update ()
		{
			// Animate the small circle color.
			gameplayUI.SmallCircleImage.color = smallGradient.Evaluate(colorChangeCurve.Evaluate(Time.timeSinceLevelLoad / smallCircleAnimTime));

			// If DontTap, animate the big circle with the warning colors.
			if (GameController.TapType == TapType.DontTap)
			{
				gameplayUI.BigCircleImage.color = warningGradient.Evaluate(colorChangeCurve.Evaluate(Time.timeSinceLevelLoad / warningAnimTime));
			}
			// Otherwise set flat color.
			else
			{
				gameplayUI.BigCircleImage.color = bigColor;
			}

			// If the gameplay is still going, expand the radial to reflect remaining time.
			if (GameController.State == GameState.Expanding || GameController.State == GameState.WaitingForTap)
			{
				gameplayUI.RadialCircleImage.fillAmount = 1 - GameController.TimeLeft / GameController.TimeToReact;
			}
			else
			{
				gameplayUI.RadialCircleImage.fillAmount = 0;
			}
		}



		private void OnGameplayStart (bool revived)
		{
			// Setup UI depending on whether the player just revived or not.
			if (!revived)
			{
#if UNITY_EDITOR || UNITY_STANDALONE
				gameplayUI.FeedbackText.text = "Hold space!";
#elif UNITY_ANDROID || UNITY_IOS
				gameplayUI.FeedbackText.text = "Tap and hold!";
#endif
				gameplayUI.TotalScoreText.text = "";
				gameplayUI.HeartImage.fillAmount = 0f;
				gameplayUI.IntroductionPanel.SetActive(true);
			}
			else
			{
				gameplayUI.FeedbackText.text = "Revived!";

				gameplayUI.IntroductionPanel.SetActive(false);
			}

			ShowGameplayUI();
		}



		private void OnBigCircleSizeUpdate (float scale)
		{
			// Set size of big circle.
			tempVector.x = scale;
			tempVector.y = scale;
			gameplayUI.BigCircleTransform.localScale = tempVector;

			// Show the warning images if the next circle is of the DontTap type.
			if (GameController.NextTapType == TapType.DontTap)
			{
				gameplayUI.WarningImage.SetActive(true);
				gameplayUI.WarningImage.transform.localScale = tempVector * 0.5f; // Stupid Unity and relative animations require this workaround.
				gameplayUI.WarningImageCross.SetActive(true);

				// Print "Warning!" on screen to explain the point of the warning image.
				CancelInvoke("HideFeedback");
				gameplayUI.FeedbackText.text = "Warning!";
			}
			// Otherwise, hide them.
			else
			{
				gameplayUI.WarningImage.SetActive(false);
				gameplayUI.WarningImageCross.SetActive(false);
			}
		}



		private void OnSmallCircleSizeUpdate (float scale)
		{
			// Set size of the smaller, expanding circle.
			tempVector.x = scale;
			tempVector.y = scale;

			gameplayUI.SmallCircleTransform.localScale = tempVector;
		}



		private void OnCircleCompleted (float score)
		{
			// Cancel any ongoing invokes of the feedback string reset.
			CancelInvoke("HideFeedback");

			// Show a new feedback string with appropriate text based on performance.
			gameplayUI.FeedbackText.text = GetScoreFeedbackString(score);

			// Schedule reset of feedback string.
			Invoke("HideFeedback", 1f);

			// Play appropriate particle effect, or none, based on performance.
			if (score >= Values.ScoreThresholdHighest)
			{
				gameplayUI.FeedbackPerfectParticles.Play();
			}
			else if (score >= Values.ScoreThresholdMiddle)
			{
				gameplayUI.FeedbackGoodParticles.Play();
			}

			// Hide the Introduction panel if it is active (so it disappears when the first circle is completed).
			if (gameplayUI.IntroductionPanel.activeInHierarchy)
				gameplayUI.IntroductionPanel.SetActive(false);
		}



		private void OnScoreUpdate (float score)
		{
			DisplayScore(score);

			// Bounce animation for score counter.
			gameplayUI.TotalScoreAnim.SetTrigger("Big");
		}



		private void OnGameplayEnded (float finalScore, bool newHighscore, FailReason reason)
		{
			// Show a specific hint to help the player if they failed early on.
			if (finalScore < 10)        
			{
				switch (reason)
				{
					case FailReason.ExpandedTooFar:
						ShowHint(Hint.NotTooFar);
						break;

					case FailReason.ExpandedTooLittle:
						ShowHint(Hint.Minimum);
						break;

					case FailReason.DontTap:
						ShowHint(Hint.DontTouch);
						break;

					case FailReason.TimeRanOut:
						ShowHint(Hint.Timer);
						break;
				}
			}
			// If the player is relatively experienced, show them a random hint to help them improve.
			else if (finalScore < 25)
			{
				ShowRandomHint();
			}
			// If the player is this good, we assume they don't need hints.
			else
			{
				ShowHint(Hint.None);
			}

			ShowFailedUI(finalScore, newHighscore);
		}



		private void OnMinimumCircleSizeUpdate (float scale)
		{
			// Set size of the minimum size indicator circle.
			tempVector.x = scale;
			tempVector.y = scale;

			gameplayUI.IndicatorCircleTransform.localScale = tempVector;
		}



		private void OnHeartSizeUpdate (float size, float lastValue)
		{
			gameplayUI.HeartImage.fillAmount = size;

			// If the heart is used up, play particles.
			if (size <= 0f && lastValue >= 1f)
			{
				gameplayUI.HeartbrokenParticles.Play();
			}
			// Otherwise if the heart is full after this update, play other particles.
			else if (size >= 1f)
			{
				gameplayUI.HeartFullParticles.Play();
			}
		}

#endregion



#region Private Methods

		/// <summary>
		/// Hides all the UI panels (use to clear screen, then draw new elements).
		/// </summary>
		private void HideAllUI ()
		{
			gameplayUI.Panel.SetActive(false);
			failedUI.Panel.SetActive(false);
		}



		/// <summary>
		/// Show the gameplay UI elements.
		/// </summary>
		private void ShowGameplayUI ()
		{
			HideAllUI();

			gameplayUI.Panel.SetActive(true);
			gameplayUI.WarningImage.SetActive(false);
			gameplayUI.WarningImageCross.SetActive(false);
			gameplayUI.BigCircleTransform.gameObject.SetActive(true);
			gameplayUI.IndicatorCircleTransform.gameObject.SetActive(true);
			gameplayUI.SmallCircleTransform.gameObject.SetActive(true);
			gameplayUI.FeedbackText.gameObject.SetActive(true);
			gameplayUI.TotalScoreText.gameObject.SetActive(true);
		}



		/// <summary>
		/// Show the "failed" menu UI elements.
		/// </summary>
		private void ShowFailedUI (float finalScore, bool newHighscore)
		{
			HideAllUI();

			failedUI.Panel.SetActive(true);
			failedUI.FailedButtonsAnim.SetTrigger("Move");
			failedUI.FinalScoreText.text = finalScore.ToString();

			if (newHighscore)
			{
				failedUI.NewHighscore.SetActive(true);
			}
			else
			{
				failedUI.NewHighscore.SetActive(false);
			}
		}



		/// <summary>
		/// Show a specific hint on the failed menu screen.
		/// </summary>
		private void ShowHint (Hint hint)
		{
			if (hint == Hint.None)
			{
				failedUI.HintsPanel.SetActive(false);
			}
			else
			{
				failedUI.HintText.text = hintsContainer.GetHintText(hint);

				failedUI.HintsPanel.SetActive(true);
			}
		}



		/// <summary>
		/// Show a random hint on the failed menu screen.
		/// </summary>
		private void ShowRandomHint ()
		{
			failedUI.HintText.text = hintsContainer.GetRandomHintText();

			failedUI.HintsPanel.SetActive(true);
		}



		/// <summary>
		/// Hide the feedback text string on the gameplay screen.
		/// </summary>
		private void HideFeedback ()
		{
			gameplayUI.FeedbackText.text = "";
		}



		/// <summary>
		/// Prints a score value onto the gameplay screen.
		/// </summary>
		private void DisplayScore (float score)
		{
			// Format to 2 decimal places if necessary; show new total score
			if (score.ToString().Contains("."))
			{
				string decimals = "" + score;
				decimals = decimals.Split('.')[1];
				if (decimals.Length > 2)
					decimals = decimals.Substring(0, 2);

				gameplayUI.TotalScoreText.text = score.ToString().Split('.')[0] + "." + decimals;
			}
			else
			{
				gameplayUI.TotalScoreText.text = score.ToString();
			}
		}



		/// <summary>
		/// Given a score 0.0-1.0, returns a text string describing how good that score is.
		/// </summary>
		private string GetScoreFeedbackString (float score)
		{
			if (score < Values.ScoreThresholdLowest)
				return "Terrible :(";
			else if (score < Values.ScoreThresholdLow)
				return "Poor...";
			else if (score < Values.ScoreThresholdMiddle)
				return "Not bad!";
			else if (score < Values.ScoreThresholdHigh)
				return "Great!";
			else if (score < Values.ScoreThresholdHighest)
				return "Excellent!";
			else
				return "PERFECT!";
		}

#endregion



#region UI Button Methods -- Do not call from code

		public void Button_Restart ()
		{
			Events.RestartRequest();
		}



		public void Button_Revive ()
		{
			Events.WatchAdRequest();
		}



		public void Button_BackToMainMenu ()
		{
			Events.MainMenuRequest();
		}

#endregion



#region Utility Classes

		[Serializable]
		private class GameplayContent
		{
			[Header("Objects:")]
			[Tooltip("The panel that is the parent to all other gameplay UI elements.")]
			public GameObject Panel;
			[Tooltip("Reference to the warning image object.")]
			public GameObject WarningImage;
			[Tooltip("Reference to the warning image cross object.")]
			public GameObject WarningImageCross;
			[Tooltip("Parent panel for the introduction graphic shown before the gameplay starts.")]
			public GameObject IntroductionPanel;

			[Header("Transforms:")]
			[Tooltip("Transform of the big circle.")]
			public RectTransform BigCircleTransform;
			[Tooltip("Transform of the indicator circle.")]
			public RectTransform IndicatorCircleTransform;
			[Tooltip("Transform of the small circle.")]
			public RectTransform SmallCircleTransform;

			[Header("Images:")]
			[Tooltip("Image component of the big circle.")]
			public Image BigCircleImage;
			[Tooltip("Image component of the small circle.")]
			public Image SmallCircleImage;
			[Tooltip("Image component of the radial circle.")]
			public Image RadialCircleImage;
			[Tooltip("Image component of the Heart indicator.")]
			public Image HeartImage;

			[Header("Texts:")]
			[Tooltip("The text component of the feedback text.")]
			public Text FeedbackText;
			[Tooltip("The text component of the total score text.")]
			public Text TotalScoreText;

			[Header("Animators:")]
			[Tooltip("Animator for the total score text.")]
			public Animator TotalScoreAnim;

			[Header("Particle Systems:")]
			[Tooltip("Particle System for the broken heart effect.")]
			public ParticleSystem HeartbrokenParticles;
			[Tooltip("Particle System for the full heart effect.")]
			public ParticleSystem HeartFullParticles;
			[Tooltip("Particle System for Perfect feedback.")]
			public ParticleSystem FeedbackPerfectParticles;
			[Tooltip("Particle System for Good feedback.")]
			public ParticleSystem FeedbackGoodParticles;

		}

		[Serializable]
		private class FailedContent
		{
			[Header("Objects:")]
			[Tooltip("The panel that is the parent to all other \"Failed\"-screen UI elements.")]
			public GameObject Panel;
			[Tooltip("The \"New Highscore\" text object.")]
			public GameObject NewHighscore;
			[Tooltip("The parent object that contains the Hints.")]
			public GameObject HintsPanel;

			[Header("Texts:")]
			[Tooltip("The text component of the Final Score text.")]
			public Text FinalScoreText;
			[Tooltip("The text component of the Hints text.")]
			public Text HintText;

			[Header("Animators:")]
			[Tooltip("Animator for panel containing the Retry and Revive buttons.")]
			public Animator FailedButtonsAnim;
		}

#endregion
	}

}