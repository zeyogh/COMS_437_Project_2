namespace TheGreatSpaceRace
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

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

            public Camera2(GraphicsDeviceManager graphics)
            {
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
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    camPosition.X -= 0.1f;
                    camTarget.X -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    camPosition.X += 0.1f;
                    camTarget.X += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    camPosition.Y -= 0.1f;
                    camTarget.Y -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    camPosition.Y += 0.1f;
                    camTarget.Y += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                {
                    camPosition.Z += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                {
                    camPosition.Z -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    orbit = !orbit;
                }

                if (orbit)
                {
                    Matrix rotationMatrix = Matrix.CreateRotationY(
                                            MathHelper.ToRadians(1f));
                    camPosition = Vector3.Transform(camPosition,
                                  rotationMatrix);
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
}