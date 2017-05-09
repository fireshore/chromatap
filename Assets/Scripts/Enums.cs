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
	/// Describes how the player should interact with the current circle.
	/// </summary>
	public enum TapType
	{
		TapAndHold,
		DontTap,
	}



	/// <summary>
	/// State machine enum for the game loop.
	/// </summary>
	public enum GameState
	{
		NotStarted,
		WaitingForTap,
		Expanding,
		Complete,
		Failed,
		Ended,
	}



	/// <summary>
	/// Reason for why the player failed a circle.
	/// </summary>
	public enum FailReason
	{
		ExpandedTooFar,
		DontTap,
		ExpandedTooLittle,
		TimeRanOut,
	}



	/// <summary>
	/// The type of hint to show to the player, explaning various game mechanics.
	/// </summary>
	public enum Hint
	{
		None,
		DontTouch,
		Warning,
		Points,
		Timer,
		Minimum,
		FillingHeart,
		FullHeart,
		NotTooFar,
	}



	/// <summary>
	/// Various types of sound effects.
	/// </summary>
	public enum Sound
	{
		Perfect,
		Good,
		Bad,
		Failed,
		Heartbroken,
		HeartFull,
	}
}
