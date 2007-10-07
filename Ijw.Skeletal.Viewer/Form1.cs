﻿using System;
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
using IjwFramework.Types;

namespace Ijw.Skeletal.Viewer
{
	using Math = System.Math;

	public class Form1 : Form
	{
		GraphicsDevice device;
		Shader shader;
		Shader wireframe;
		Shader points;

		CoreSkeleton coreSkeleton = new CoreSkeleton("../../../res/skeleton.xsf");
		Cache<string, CoreMesh> meshes;
		Cache<string, CoreAnimation> animations;
		Cache<string, Texture> textures;

		FvfVertexBuffer<Vertex> vertices;
		FvfVertexBuffer<Vector3> haxVerts;
		IndexBuffer indices;

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
			wireframe = new Shader(device, File.OpenRead("../../../res/wire.fx"));
			points = new Shader(device, File.OpenRead("../../../res/point.fx"));

			meshes = new Cache<string,CoreMesh>(
				x => new CoreMesh("../../../res/" + x + ".xmf", coreSkeleton ));

			animations = new Cache<string, CoreAnimation>(
				x => new CoreAnimation("../../../res/" + x + ".xaf", coreSkeleton));

			vertices = new FvfVertexBuffer<Vertex>(device, 1024,
				VertexFormat.Position | VertexFormat.Normal | VertexFormat.Texture);

			haxVerts = new FvfVertexBuffer<Vector3>(device, 1024,
				VertexFormat.Position);

			indices = new IndexBuffer(device, 1024);

			textures = new Cache<string, Texture>(
				x => Texture.Create(File.OpenRead("../../../res/" + x), device));

			skeleton = new Skeleton(coreSkeleton);

			mixer.Play(animations["walk"]).Looping().WithWeight(0.3f);
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
			device.Clear(Color.Blue.ToArgb(), Surfaces.Color | Surfaces.Depth);

			var view = Matrix.LookAt( 
				new Vector3( 6 * (float)Math.Sin(t), 6, 6 * (float)Math.Cos(t) ), 
				new Vector3( 0, 3, 0 ), 
				Vector3.UnitY );
			var proj = Matrix.Perspective( (float)Math.PI / 2, ClientSize.Width / ClientSize.Height,
				0.01f, 1000.0f );

			shader.SetValue("viewProjMatrix", view * proj);
			shader.SetValue("worldMatrix", Matrix.Scale(-1,1,1) * Matrix.RotationX(-(float)Math.PI / 2));

			var mm = new Pair<string,string>[]
			{
				new Pair<string,string>( "box01", "hax.tga" ),
				new Pair<string,string>( "p90", "p90-template.tga" )
			};

			foreach (var m in mm.Select(x => 
				new 
				{ 
					Mesh = meshes[x.First], 
					Texture = textures[x.Second] 
				} ))
			{
				var v = m.Mesh.GetTransformedVertices(skeleton);
				var i = m.Mesh.GetIndices();

				shader.SetValue("diffuseTexture", m.Texture);

				vertices.SetData(v);
				indices.SetData(i);

				vertices.Bind(0);
				indices.Bind();

				shader.Render(() =>
					device.DrawIndexedPrimitives(PrimitiveType.TriangleList, v.Length, i.Length / 3));
			}

			var verts = skeleton.GetBoneLines().ToArray();
			haxVerts.SetData(verts);
			haxVerts.Bind(0);

			wireframe.Render(() =>
				device.DrawPrimitives(PrimitiveType.LineList, verts.Length / 2));

			verts = skeleton.GetBonePoints().ToArray();
			haxVerts.SetData(verts);
			haxVerts.Bind(0);

			points.Render(() =>
				device.DrawPrimitives(PrimitiveType.PointList, verts.Length));

			device.End();
			device.Present();
		}
	}
}
