using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ijw.DirectX;
using System.IO;
using IjwFramework.Collections;
using Ijw.Math;

namespace Ijw.Skeletal.Viewer
{
	using Math = System.Math;

	public class Form1 : Form
	{
		
		GraphicsDevice device;
		Shader shader;

		CoreSkeleton coreSkeleton = new CoreSkeleton("../../../res/skeleton.xsf");
		Cache<string, CoreMesh> meshes;
		Cache<string, CoreAnimation> animations;

		FvfVertexBuffer<Vertex> vertices;
		IndexBuffer indices;
		Texture texture;

		Mixer mixer = new Mixer();
		Skeleton skeleton;

		public Form1()
		{
			ClientSize = new Size(640, 480);
			Text = "Ijw.Skeletal.Viewer";
			Visible = true;

			device = GraphicsDevice.Create(this, ClientSize.Width, ClientSize.Height, true, true,
				Surfaces.Color | Surfaces.Depth);

			shader = new Shader(device, File.OpenRead("../../../res/shader.fx"));

			meshes = new Cache<string,CoreMesh>(
				x => new CoreMesh("../../../res/" + x + ".xmf", coreSkeleton ));

			animations = new Cache<string, CoreAnimation>(
				x => new CoreAnimation("../../../res/" + x + ".xaf", coreSkeleton));

			vertices = new FvfVertexBuffer<Vertex>(device, 1024,
				VertexFormat.Position | VertexFormat.Normal | VertexFormat.Texture);
			indices = new IndexBuffer(device, 1024);

			texture = Texture.Create(File.OpenRead("../../../res/hax.tga"), device);

			skeleton = new Skeleton(coreSkeleton);

			mixer.Play(animations["aim"]).Looping();
		}

		public void Run()
		{
			while (Created && Visible)
			{
				Application.DoEvents();
				Frame();
			}
		}

		float t = 0;

		void Frame()
		{
			float dt = 0.005f;
			t += dt;

			mixer.Update(dt);
			skeleton.Animate(mixer);

			device.Begin();
			device.Clear(Color.Red.ToArgb(), Surfaces.Color | Surfaces.Depth);

			var view = Matrix.LookAt( 
				new Vector3( 10 * (float)Math.Sin(t), 10, 10 * (float)Math.Cos(t) ), 
				new Vector3( 0, 3, 0 ), 
				Vector3.UnitY );
			var proj = Matrix.Perspective( (float)Math.PI / 2, ClientSize.Width / ClientSize.Height,
				0.01f, 1000.0f );

			shader.SetValue("viewProjMatrix", view * proj);
			shader.SetValue("worldMatrix", Matrix.Scale(-1,1,1) * Matrix.RotationX(-(float)Math.PI / 2));
			shader.SetValue("diffuseTexture", texture);

			string[] mm = { "box01", "p90" };

			foreach (var m in mm.Select(x => meshes[x]))
			{

				var v = m.GetTransformedVertices(skeleton);
				var i = m.GetIndices();

				vertices.SetData(v);
				indices.SetData(i);

				vertices.Bind(0);
				indices.Bind();

				shader.Render(() =>
					device.DrawIndexedPrimitives(PrimitiveType.TriangleList, v.Length, i.Length / 3));
			}

			device.End();
			device.Present();
		}
	}
}
