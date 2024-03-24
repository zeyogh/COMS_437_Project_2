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
            float rotationAngle = MathHelper.ToRadians(1.0f);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                float deltaX = camTarget.X - camPosition.X;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaX) + rotationAngle;
                camTarget.X = camPosition.X + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                float deltaX = camTarget.X - camPosition.X;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaX) - rotationAngle;
                camTarget.X = camPosition.X + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
            }
            Vector3 forward = Vector3.Normalize(camTarget - camPosition);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition += forward * 0.1f;
                System.Diagnostics.Debug.WriteLine(camPosition);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition -= forward * 0.1f;
                System.Diagnostics.Debug.WriteLine(camPosition);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                float deltaY = camTarget.Y - camPosition.Y;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaY * deltaY + deltaZ * deltaZ);
                float angle = Math.Max(-MathHelper.PiOver2 + 0.01f, Math.Abs((float)Math.Atan2(deltaZ, deltaY) - rotationAngle)); // The Math.Max helps avoid stuttering
                angle = MathHelper.Clamp(angle, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 * 2 - 0.01f); // Keep the angle within -π/2 to +π/2 range
                camTarget.Y = camPosition.Y + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
                System.Diagnostics.Debug.WriteLine(MathHelper.ToDegrees(angle));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                float deltaY = camTarget.Y - camPosition.Y;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaY * deltaY + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaY) + rotationAngle; // Calculate the new angle
                angle = MathHelper.Clamp(angle, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 * 2 - 0.01f); // Keep the angle within -π/2 to +π/2 range
                camTarget.Y = camPosition.Y + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
                System.Diagnostics.Debug.WriteLine(MathHelper.ToDegrees(angle));
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix * Matrix.CreateScale(10, 10, 10) * Matrix.CreateTranslation(camPosition.X, camPosition.Y, camPosition.Z);
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}