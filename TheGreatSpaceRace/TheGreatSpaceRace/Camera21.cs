using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
//using Vector3 = BEPUutilities.Vector3;
//using Matrix = BEPUutilities.Vector3;

namespace TheGreatSpaceRace
{
    internal class Camera2
    {
        //Cameras
        Vector3 camTarget;
        public Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        bool hasRotated = false;
        bool fixedPos = false;

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
                hasRotated = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                float deltaX = camTarget.X - camPosition.X;
                float deltaZ = camTarget.Z - camPosition.Z;
                float distance = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
                float angle = (float)Math.Atan2(deltaZ, deltaX) - rotationAngle;
                camTarget.X = camPosition.X + distance * (float)Math.Cos(angle);
                camTarget.Z = camPosition.Z + distance * (float)Math.Sin(angle);
                hasRotated = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Vector3 direction = camTarget - camPosition;
                float distance = direction.Length();
                direction.Normalize();
                Vector3 right = Vector3.Cross(direction, Vector3.Up);
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(right, rotationAngle));
                camTarget = camPosition + direction * distance;
                hasRotated = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector3 direction = camTarget - camPosition;
                float distance = direction.Length();
                direction.Normalize();
                Vector3 right = Vector3.Cross(direction, Vector3.Up);
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(right, -rotationAngle));
                camTarget = camPosition + direction * distance;
                hasRotated = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Vector3 direction = Vector3.Normalize(camTarget - camPosition);
                camPosition += direction * 1f;
                camTarget += direction * 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Vector3 direction = Vector3.Normalize(camTarget - camPosition);
                camPosition -= direction * 1f;
                camTarget -= direction * 1f;
            }


            if (hasRotated && fixedPos)
            {
                fixedPos = false;
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
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
                    effect.World = worldMatrix * Matrix.CreateScale(10, 10, 10) * Matrix.CreateTranslation(camPosition.X, camPosition.Y, camPosition.Z);
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