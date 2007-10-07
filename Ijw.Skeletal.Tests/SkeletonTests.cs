using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Ijw.Skeletal.Tests
{
	[TestFixture]
	public class SkeletonTests
	{
		const string skelPath = "../../../res/Skeleton.xsf";
		const string animPath = "../../../res/aim.xaf";
		const string meshPath = "../../../res/p90.xmf";

		[Test]
		public void Test2()
		{
			var skeleton = new CoreSkeleton(skelPath);
			var animation = new CoreAnimation(animPath, skeleton);

			Assert.AreEqual(0.0333333f, animation.Duration);
		}

		[Test]
		public void Test3()
		{
			var skeleton = new CoreSkeleton(skelPath);
			var animation = new CoreAnimation(animPath, skeleton);

			var skelInstance = new Skeleton(skeleton);
		}

		[Test]
		public void Test4()
		{
			// testing fluent interface

			var skeleton = new CoreSkeleton(skelPath);
			var animation = new CoreAnimation(animPath, skeleton);

			var mixer = new Mixer();

			bool callbackHappened = false;

			mixer.Play(animation).Once().WithWeight(1.0f).AndThen(() => callbackHappened = true);

			// now take a skeleton instance, and calculate its state.

			var skelInstance = new Skeleton(skeleton);
			skelInstance.Animate(mixer);

			mixer.Update(0.2f);
			Assert.IsTrue(callbackHappened);
		}

		[Test]
		public void Test5()
		{
			var skeleton = new CoreSkeleton(skelPath);
			var mesh = new CoreMesh(meshPath, skeleton);
			var anim = new CoreAnimation(animPath, skeleton);

			var skelInstance = new Skeleton(skeleton);
			var mixer = new Mixer();

			// play the animation
			mixer.Play(anim).Once();
			mixer.Update(0);

			skelInstance.Animate(mixer);

			var verts = mesh.GetTransformedVertices(skelInstance);
			var indices = mesh.GetIndices();
		}
	}
}
