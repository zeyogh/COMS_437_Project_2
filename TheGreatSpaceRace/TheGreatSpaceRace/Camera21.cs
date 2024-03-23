using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TheGreatSpaceRace
{
    internal class Camera2
    {
        //Cameras
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //Geometric info
        Model model;

        //Orbit
        bool orbit = false;

        public Camera2(GraphicsDeviceManager graphics, Model model)
        {
            this.model = model;
            //setup camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -5);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                          Forward, Vector3.Up);
        }

        public void Update(GameTime gameTime)
        {
            float rotationSpeed = 0.02f;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                float deltaX = camTarget.X - camPosition.X;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaX) + rotationSpeed;
                camTarget.X = camPosition.X + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                float deltaX = camTarget.X - camPosition.X;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaX) - rotationSpeed;
                camTarget.X = camPosition.X + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Z += 0.1f;
                //camPosition.Y -= 0.1f;
                //camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Z -= 0.1f;
                //camPosition.Y += 0.1f;
                //camTarget.Y += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                float deltaY = camTarget.Y - camPosition.Y;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaY * deltaY + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaY) + rotationSpeed;
                camTarget.Y = camPosition.Y + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                float deltaY = camTarget.Y - camPosition.Y;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaY * deltaY + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaY) - rotationSpeed;
                camTarget.Y = camPosition.Y + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            /*if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(
                                        MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition,
                              rotationMatrix);
            }*/
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}