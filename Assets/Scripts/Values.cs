// Project:			Chromatap
// Copyright:       Copyright (C) 2017 Fireshore Entertainment
// Company:			Fireshore Entertainment
// Original Author: Mathias Alexander Ibsen
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections;

namespace Chromatap
{
	/// <summary>
	/// Globally defined constants.
	/// </summary>

	public static class Values
	{
		/// <summary>
		/// Lowest score threshold.
		/// </summary>
		public const float ScoreThresholdLowest = 0.6f;
		/// <summary>
		/// Second lowest score threshold.
		/// </summary>
		public const float ScoreThresholdLow = 0.7f;
		/// <summary>
		/// Middle score threshold.
		/// </summary>
		public const float ScoreThresholdMiddle = 0.8f;
		/// <summary>
		/// Second best score threshold.
		/// </summary>
		public const float ScoreThresholdHigh = 0.9f;
		/// <summary>
		/// Best score threshold.
		/// </summary>
		public const float ScoreThresholdHighest = 0.96f;
	}

}