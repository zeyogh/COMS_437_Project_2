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

        //Geometric info
        Model model;

        //Orbit
        bool orbit = false;

        public Camera2(GraphicsDeviceManager graphics, Model model, Game game) : base(game)
        {
            this.model = model;
            camPositionPhysics = new BEPUutilities.Vector3(0f, 0f, -5);
            ship = new Sphere(camPositionPhysics, 5, 1);
            ship.Gravity = new BEPUutilities.Vector3(0, 0, 0); //black holes
            ship.LinearDamping = 0.6f;
            ship.AngularDamping = 0.6f;
            ship.BecomeDynamic(1);
            ((Space)Game.Services.GetService(typeof(Space))).Add(ship);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(convertVector3ToXNA(ship.Position), convertVector3ToXNA(ship.WorldTransform.Forward),
                         new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(convertVector3ToXNA(ship.WorldTransform.Forward), Vector3.
                          Forward, Vector3.Up);
        }


        public void Update(GameTime gameTime)
        {
            BEPUutilities.Matrix world = ship.WorldTransform;
            float rotationAngle = MathHelper.ToRadians(1.0f);
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
                ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                BEPUutilities.Vector3 v = ship.WorldTransform.Right; //Up
                ship.ApplyAngularImpulse(ref v);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ship.ApplyImpulse(ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                BEPUutilities.Vector3 v = ship.WorldTransform.Left; //Down
                ship.ApplyAngularImpulse(ref v); //move
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

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Microsoft.Xna.Framework.Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix * Matrix.CreateScale(10, 10, 10) * convertMatrixToXNA(ship.WorldTransform);
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


            g.DepthStencilState = originalDepthStencilState;
        }
    }
}