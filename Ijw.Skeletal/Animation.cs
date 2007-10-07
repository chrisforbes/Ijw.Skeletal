﻿using System;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	using Influence = Pair<float, Transform>;

	public class Animation
	{
		readonly CoreAnimation coreAnimation;
		float time;
		float weight = 1;
		bool looping;
		Action completedCallbacks;

		public Animation(CoreAnimation coreAnimation)
		{
			this.coreAnimation = coreAnimation;
		}

		public float Duration { get { return coreAnimation.Duration; } }
		public float Time { get { return time; } }
		public bool Complete { get { return Time > Duration; } }
		public bool IsLooping { get { return looping; } }
		public float Weight { get { return weight; } }

		public CoreAnimation CoreAnimation { get { return coreAnimation; } }

		public void Update(float dt)
		{
			time += dt;
			if (time > Duration)
			{
				if (!looping && completedCallbacks != null)
					completedCallbacks();

				while (looping && Time > Duration)
					time -= Duration;
			}
		}

		internal Influence? TransformFor(Bone b)
		{
			var track = coreAnimation.GetTrack(b.CoreBone);

			return (track != null && weight > 0)
				? new Influence(weight, track.GetTransformAt(time))
				: default(Influence?);
		}

		public Animation Looping() { looping = true; return this; }
		public Animation Once() { looping = false; return this; }
		public Animation WithWeight(float w) { weight = w; return this; }
		public Animation AndThen(Action action) { completedCallbacks += action; return this; }
	}
}
