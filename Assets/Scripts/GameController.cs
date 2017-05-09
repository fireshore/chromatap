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
	/// Handles the main gameplay loop as well as player interaction.
	/// </summary>

	public class GameController : MonoBehaviour
	{
		#region Fields

		// Singleton
		private static GameController instance;



		// Exposed:
		[Header("Settings:")]
		[SerializeField, Tooltip("The amount of 3rd best (defined in Values.cs) or higher value taps in a row before a Heart is awarded."), Range(2, 30)]
		private int goodTapsForHeart = 10;
		[SerializeField, Tooltip("The start expansion speed, increased by the Time Curve.")]
		private float expansionSpeed = 1.5f;
		[SerializeField, Tooltip("Amount of circles to complete before the Expansion Speed and Minimum Size reaches the maximum of difficulty, defined by the difficulty curves.")]
		private int circlesToMaxDif = 60;
		[SerializeField, Range(0, 0.5f), Tooltip("Start size of the minimum circle in relation to the big circle.")]
		private float initialMinimumSize = 0.5f;
		[SerializeField, Range(0.6f, 1), Tooltip("Max size of the minimum circle in relation to the big circle.")]
		private float maxMinimumSize = 0.9f;
		[SerializeField, Range(0, 1), Tooltip("Chance that a circle will be a Don't tap-type, range 0-1.")]
		private float dontTapChance = 0.15f;
		[SerializeField, Tooltip("Time to complete the fill, or time for the Don't tap-type circle to run out.")]
		private float timeToReact = 3.0f;

		[Header("Difficulty curves:")]
		[SerializeField, Tooltip("Increase function for how fast the expansion speed increases.")]
		private AnimationCurve speedCurve;
		[SerializeField, Tooltip("Increase function for how the minimum circle size expands.")]
		private AnimationCurve minimumCurve;



		// Private:
		private float bigMaxScale = 2.0f;				// Max scale of the bigger circle.
		private float bigMinScale = 1.0f;				// Minimum scale of the bigger circle.
		private float bigCurScale;						// Current scale of the bigger circle.
		private float smallCurScale;					// Current scale of the smaller (expanding) circle.
		private float minimumCurScale;					// Current size of the minimum circle, that the smaller circle must expand past.
		private TapType type;							// Current circle challenge type.
		private TapType nextType;						// Next circle challenge type.
		private GameState state;						// Current state of the game loop.
		private FailReason failReason;					// Reason for why the player failed a circle challenge.
		private int circlesCompleted;					// Circle challenges completed.
		private float timeStarted;						// Timestamp for when the player started the gameplay this round.
		private int goodTapsStreak = 0;					// Amount of "good" successful challenges in a row.



		// Properties:
		public static TapType TapType { get { return instance.type; } }
		public static TapType NextTapType { get { return instance.nextType; } }
		public static GameState State { get { return instance.state; } }
		public static float TimeLeft { get { return instance.timeToReact - (Time.timeSinceLevelLoad - instance.timeStarted); } }
		public static float TimeToReact { get { return instance.timeToReact; } }

		#endregion
		


		#region Unity and Events

		private void Awake ()
		{
			if (instance == null)
				instance = this;
			else Debug.LogError("Singleton Error: Multiple GameController components detected in scene.", this);

			SetupListeners();
		}



		private void Start ()
		{
			InitGameplay();
		}



		private void OnDestroy ()
		{
			RemoveListeners();
		}



		private void Update ()
		{
			// Main state machine
			switch (state)
			{
				case GameState.NotStarted:
					State_NotStarted();
					break;

				case GameState.WaitingForTap:
					State_WaitingForTap();
					CheckTime();
					break;

				case GameState.Expanding:
					State_Expanding();
					CheckTime();
					break;

				case GameState.Failed:
					State_Failed();
					break;

				case GameState.Ended:
					State_Ended();
					break;

				case GameState.Complete:
					State_Complete();
					break;
			}
		}



		private void SetupListeners ()
		{
			Events.onWatchAdCompleted += OnWatchAdCompleted;
			Events.onRestartRequest += OnRestartRequest;
		}



		private void RemoveListeners ()
		{
			Events.onWatchAdCompleted -= OnWatchAdCompleted;
			Events.onRestartRequest -= OnRestartRequest;
		}



		private void OnWatchAdCompleted ()
		{
			Revive();
		}



		private void OnRestartRequest ()
		{
			ResetGameplay();
		}

		#endregion



		#region State Methods

		private void State_NotStarted ()
		{
			// Gameplay doesn't begin before the player starts expanding the circle
			if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
			{
				timeStarted = Time.timeSinceLevelLoad;  // Reset time when we start expanding
				state = GameState.Expanding;
			}
		}



		private void State_WaitingForTap ()
		{
			// When the user starts expanding the circle, switch state
			if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
			{
				if (type == TapType.TapAndHold)
				{
					state = GameState.Expanding;
				}
				else if (type == TapType.DontTap)
				{
					failReason = FailReason.DontTap;
					state = GameState.Failed;
				}
			}
		}



		private void State_Expanding ()
		{
			// Update small circle size
			smallCurScale += (expansionSpeed + (expansionSpeed * speedCurve.Evaluate((float) circlesCompleted / (float) circlesToMaxDif))) * Time.deltaTime;
			Events.SmallCircleSizeUpdate(smallCurScale);

			// If the smaller circle grows bigger than the big circle
			if (smallCurScale > bigCurScale)
			{
				failReason = FailReason.ExpandedTooFar;
				state = GameState.Failed;
			}
			// If not failed yet, keep checking input
			else if (!Input.GetKey(KeyCode.Space) && Input.touchCount < 1)
			{
				// If the small circle is at least 50% of the big circle, completed
				if (smallCurScale >= minimumCurScale)
				{
					Events.CircleCompleted(smallCurScale / bigCurScale);
					state = GameState.Complete;

					UpdateHeartStreak();
				}
				// else, fail
				else
				{
					failReason = FailReason.ExpandedTooLittle;
					state = GameState.Failed;
				}
			}
		}



		private void State_Failed ()
		{
			// If the player has a full heart, we don't fail this time.
			if (goodTapsStreak >= goodTapsForHeart)
			{
				Events.SavedByHeart();
				state = GameState.Complete;
			}
			// Otherwise failed.
			else
			{
				Events.CircleFailed(failReason);
				state = GameState.Ended;
			}

			// Reset heart streak regardless.
			goodTapsStreak = 0;
			Events.HeartSizeUpdate(0f);
		}



		private void State_Ended ()
		{
			// Wait for the user UI input to change the state through events.
		}



		private void State_Complete ()
		{
			// TODO: Animation for completing a circle

			circlesCompleted++;
			NewCircle();
			state = GameState.WaitingForTap;
		}

		#endregion



		#region Private Methods

		/// <summary>
		/// Initialize (or reset) gameplay state.
		/// </summary>
		private void InitGameplay ()
		{
			nextType = TapType.TapAndHold;
			state = GameState.NotStarted;
			circlesCompleted = 0;
			goodTapsStreak = 0;

			NewCircle();

			Events.GameplayStart(false);
		}



		/// <summary>
		/// Restarts the gameplay from the beginning with 0 score.
		/// </summary>
		private void ResetGameplay ()
		{
			InitGameplay();
		}



		/// <summary>
		/// Used to resume gameplay when reviving (does not reset streaks and other tracking variables).
		/// </summary>
		private void Revive ()
		{
			nextType = TapType.TapAndHold;
			state = GameState.NotStarted;

			NewCircle();

			Events.GameplayStart(true);
		}



		/// <summary>
		/// Setup and create the next circle challenge.
		/// </summary>
		private void NewCircle ()
		{
			timeStarted = Time.timeSinceLevelLoad;

			type = nextType;

			// Decide randomly what the next tap type will be. No two DontTap in a row.
			if (Random.Range(0f, 1f) > dontTapChance || type == TapType.DontTap)
			{
				nextType = TapType.TapAndHold;
			}
			else
			{
				nextType = TapType.DontTap;
			}

			// DontTap-circles should not appear before 4 TapAndHold-circles have been completed.
			if (circlesCompleted < 4)
			{
				nextType = TapType.TapAndHold;
			}

			if (type == TapType.TapAndHold)
			{
				NewTapAndHoldCircle();
			}
			else
			{
				NewDontTapCircle();
			}

			// Set sizes
			ResetSmallCircle();
			SetMinimumCircle();
		}



		/// <summary>
		/// Give the big circle a new, random size within range and update the UI.
		/// </summary>
		private void NewTapAndHoldCircle ()
		{
			if (circlesCompleted == 0) // First one is always max size, so we can fit the Introduction graphic onto it perfectly
			{
				bigCurScale = bigMaxScale;
			}
			else
			{
				bigCurScale = Random.Range(bigMinScale, bigMaxScale);
			}

			Events.TapAndHoldStarted(timeToReact);
			Events.BigCircleSizeUpdate(bigCurScale);
		}



		/// <summary>
		/// Create a circle that should not be pressed.
		/// </summary>
		private void NewDontTapCircle ()
		{
			bigCurScale = Random.Range(bigMinScale, bigMaxScale);

			Events.DontTapStarted(timeToReact);
			Events.BigCircleSizeUpdate(bigCurScale);
		}



		/// <summary>
		/// Set the small circle scale to 0f and update the UI.
		/// </summary>
		private void ResetSmallCircle ()
		{
			smallCurScale = 0f;
			Events.SmallCircleSizeUpdate(smallCurScale);
		}



		/// <summary>
		/// Set the size of the minimum circle to the correct percentage size of the big circle.
		/// </summary>
		private void SetMinimumCircle ()
		{
			// Calculate value
			minimumCurScale = bigCurScale * (initialMinimumSize + ((1 - initialMinimumSize) * minimumCurve.Evaluate((float) circlesCompleted / (float) circlesToMaxDif)));

			// Roof
			if (minimumCurScale > bigCurScale * maxMinimumSize)
				minimumCurScale = bigCurScale * maxMinimumSize;

			Events.MinimumCircleSizeUpdate(minimumCurScale);
		}



		/// <summary>
		/// Update the "good taps" streak according to latest score.
		/// </summary>
		private void UpdateHeartStreak ()
		{
			// If good tap, increment streak counter.
			if (smallCurScale / bigCurScale >= Values.ScoreThresholdMiddle)
			{
				goodTapsStreak++;
				if (goodTapsStreak > goodTapsForHeart)
				{
					goodTapsStreak = goodTapsForHeart;
				}
				Events.HeartSizeUpdate((float) goodTapsStreak / goodTapsForHeart);
			}
			// Otherwise, reset the streak, if a full heart has not already been reached.
			else if (goodTapsStreak < goodTapsForHeart)
			{
				goodTapsStreak = 0;
				Events.HeartSizeUpdate(0f);
			}
		}



		/// <summary>
		/// Check if time has ran out.
		/// </summary>
		private void CheckTime ()
		{
			// Check if timer has been exceeded
			if (Time.timeSinceLevelLoad > timeStarted + timeToReact)
			{
				// Check the tap type
				if (type == TapType.TapAndHold)
				{
					// Time ran out, failed.
					failReason = FailReason.TimeRanOut;
					state = GameState.Failed;
				}
				else if (type == TapType.DontTap)
				{
					// Time ran out, success.
					Events.DontTapCompleted();
					state = GameState.Complete;
				}
			}
		}

		#endregion

	}
}