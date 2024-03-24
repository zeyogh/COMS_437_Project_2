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
            // Up
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Calculate the direction vector from position to target
                Vector3 direction = camTarget - camPosition;
                // Calculate the distance between position and target
                float distance = direction.Length();
                // Normalize the direction vector
                direction.Normalize();

                // Calculate the right vector perpendicular to direction and the world up vector
                Vector3 right = Vector3.Cross(direction, Vector3.Up);
                // Rotate the direction vector around the right vector
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(right, rotationAngle));

                // Update the target position based on the rotated direction
                camTarget = camPosition + direction * distance;
            }

            // Down
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Calculate the direction vector from position to target
                Vector3 direction = camTarget - camPosition;
                // Calculate the distance between position and target
                float distance = direction.Length();
                // Normalize the direction vector
                direction.Normalize();

                // Calculate the right vector perpendicular to direction and the world up vector
                Vector3 right = Vector3.Cross(direction, Vector3.Up);
                // Rotate the direction vector around the right vector in the opposite direction
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(right, -rotationAngle));

                // Update the target position based on the rotated direction
                camTarget = camPosition + direction * distance;
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
        }

        public void Draw(GameTime gameTime, Ring ring, GraphicsDevice g, BasicEffect ringEffect, float rotationY)
        {

            DepthStencilState originalDepthStencilState = g.DepthStencilState;
            g.DepthStencilState = DepthStencilState.DepthRead;

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

            g.DepthStencilState = originalDepthStencilState;

            ringEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5), Vector3.Forward, Vector3.Up);
            ringEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, g.Viewport.AspectRatio, 0.001f, 1000f);
            Vector3 position = camPosition;
            ringEffect.World = Matrix.Identity * Matrix.CreateTranslation(new Vector3(0, 0, 8)) * Matrix.CreateTranslation(position);
            ringEffect.VertexColorEnabled = true;

            ring.Draw(g, ringEffect);
        }
    }
}