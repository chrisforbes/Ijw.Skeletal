using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	using Influence = Pair<float, Transform>;

	public class Mixer
	{
		List<Animation> animations = new List<Animation>();

		public Animation Play(CoreAnimation coreAnimation)
		{
			var anim = new Animation(coreAnimation);
			animations.RemoveAll(x => x.CoreAnimation == coreAnimation);
			animations.Add(anim);
			return anim;
		}

		public void Stop(CoreAnimation coreAnimation)
		{
			animations.RemoveAll(x => x.CoreAnimation == coreAnimation);
		}

		public Animation Animation(CoreAnimation coreAnimation)
		{
			return animations.Find(x => x.CoreAnimation == coreAnimation);
		}

		public void StopAll()
		{
			animations.Clear();
		}

		public IEnumerable<Influence> GetInfluencesFor(Bone b)
		{
			return animations.Select(x => x.TransformFor(b)).
				Where(x => x != null).
				Cast<Influence>();
		}

		public void Update(float dt)
		{
			foreach (var anim in animations)
				anim.Update(dt);
			animations.RemoveAll(x => x.Complete);
		}
	}
}
