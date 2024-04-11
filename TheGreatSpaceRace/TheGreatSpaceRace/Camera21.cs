using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionTests;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using BEPUphysics.Entities;
using BEPUphysics;
using System.Reflection.Metadata;
using System.Net.NetworkInformation;
//using Vector3 = BEPUutilities.Vector3;
//using Matrix = BEPUutilities.Vector3;

namespace TheGreatSpaceRace
{
    internal class Camera2 : GameComponent
    {
        //Cameras

        public Entity ship;
        BEPUutilities.Vector3 camPositionPhysics;



        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        bool hasRotated = false;
        bool fixedPos = false;

        Model sky;

        Model spaceship;

        public Camera2(GraphicsDeviceManager graphics, Model sky, Model spaceship, Game game) : base(game)
        {
            this.sky = sky;
            this.spaceship = spaceship;
            camPositionPhysics = new BEPUutilities.Vector3(0f, 0f, -5);
            ship = new Sphere(camPositionPhysics, 5, 1);
            ship.Gravity = new BEPUutilities.Vector3(0, 0, 0); //black holes
            ship.LinearDamping = 0.8f;
            ship.AngularDamping = 0.95f;
            ship.BecomeDynamic(1);
            ((Space)Game.Services.GetService(typeof(Space))).Add(ship);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, -5), new Vector3(0, 0, 0),
                         Vector3.Up);
            //viewMatrix = Matrix.CreateLookAt(convertVector3ToXNA(ship.Position), convertVector3ToXNA(ship.Position + ship.WorldTransform.Forward),
            //convertVector3ToXNA(ship.WorldTransform.Right));
            worldMatrix = Matrix.CreateWorld(convertVector3ToXNA(ship.Position),
                         convertVector3ToXNA(ship.WorldTransform.Forward), Vector3.Up);
        }


        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                BEPUutilities.Vector3 v = ship.WorldTransform.Down; //right
                ship.ApplyAngularImpulse(ref v);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                BEPUutilities.Vector3 v = ship.WorldTransform.Up; //left
                ship.ApplyAngularImpulse(ref v);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (ship.WorldTransform.Up.Y > 0.5f || ship.WorldTransform.Down.Z > 0) //can drift out of bounds
                {
                    ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                    System.Diagnostics.Debug.WriteLine("Up:" + ship.WorldTransform.Up);
                    BEPUutilities.Vector3 v = ship.WorldTransform.Right; //Up
                    ship.ApplyAngularImpulse(ref v);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Up Invalid:" + ship.WorldTransform.Up);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (ship.WorldTransform.Down.Y < -0.5f || ship.WorldTransform.Up.Z > 0)
                {
                    ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                    BEPUutilities.Vector3 v = ship.WorldTransform.Left; //Down
                    System.Diagnostics.Debug.WriteLine("Down:" + ship.WorldTransform.Down);
                    ship.ApplyAngularImpulse(ref v); //move
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Down Invalid:" + ship.WorldTransform.Down);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                BEPUutilities.Vector3 v = ship.WorldTransform.Forward; //Forward
                ship.ApplyLinearImpulse(ref v); //move
            }


            if (hasRotated && fixedPos)
            {
                fixedPos = false;
            }

            viewMatrix = Matrix.CreateLookAt(convertVector3ToXNA(ship.Position), convertVector3ToXNA(ship.Position)
                + convertVector3ToXNA(ship.WorldTransform.Forward), Vector3.Up);
        }

        public static Microsoft.Xna.Framework.Matrix convertMatrixToXNA(BEPUutilities.Matrix bepuMatrix)
        {
            Microsoft.Xna.Framework.Matrix xnaMatrix = new Microsoft.Xna.Framework.Matrix(
                bepuMatrix.M11, bepuMatrix.M12, bepuMatrix.M13, bepuMatrix.M14,
                bepuMatrix.M21, bepuMatrix.M22, bepuMatrix.M23, bepuMatrix.M24,
                bepuMatrix.M31, bepuMatrix.M32, bepuMatrix.M33, bepuMatrix.M34,
                bepuMatrix.M41, bepuMatrix.M42, bepuMatrix.M43, bepuMatrix.M44
            );

            return xnaMatrix;
        }

        public static Microsoft.Xna.Framework.Vector3 convertVector3ToXNA(BEPUutilities.Vector3 bepuVec)
        {
            Microsoft.Xna.Framework.Vector3 vec = new Microsoft.Xna.Framework.Vector3(
                bepuVec.X, bepuVec.Y, bepuVec.Z
            );

            return vec;
        }

        public void Draw(GameTime gameTime, Ring2[] rings, GraphicsDevice g, BasicEffect ringEffect, float rotationY)
        {

            DepthStencilState originalDepthStencilState = g.DepthStencilState;
            g.DepthStencilState = DepthStencilState.DepthRead;

            foreach (ModelMesh mesh in sky.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateScale(30, 30, 30) * convertMatrixToXNA(ship.WorldTransform);
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            foreach (Ring2 ring in rings)
            {

                foreach (ModelMesh mesh in ring.ring.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(1f, 0, 0);
                        effect.View = viewMatrix;
                        effect.World = worldMatrix * Matrix.CreateScale(10, 10, 10) * Matrix.CreateTranslation(ring.pos.X, ring.pos.Y, ring.pos.Z);
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }

            }

            foreach (ModelMesh mesh in spaceship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(1f, 0, 0);
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 5, -10), convertVector3ToXNA(ship.WorldTransform.Forward), Vector3.Up);
                    effect.World = worldMatrix; //Matrix.CreateTranslation(ship.Position.X, ship.Position.Y, ship.Position.Z - 5);
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            g.DepthStencilState = originalDepthStencilState;
        }
    }
}