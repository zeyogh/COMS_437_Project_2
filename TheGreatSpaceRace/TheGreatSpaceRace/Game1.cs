using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUutilities;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using System;
using BEPUphysicsDrawer.Models;

namespace TheGreatSpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Ring ring;

        //Skybox skybox;
        Model skysphere;
        Matrix world = Matrix.Identity;
        Matrix view = Matrix.CreateLookAt(new Vector3(20, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
        Vector3 cameraPosition;
        float angle = 0;
        float distance = 20;

        Camera2 camera;

        BasicEffect effect; //lighting and shading

        Vector3 position;
        float rotationY;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //camera = new Camera(BEPUutilities.Vector3.Zero, 0, 0, BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, Graphics.PreferredBackBufferWidth / (float)Graphics.PreferredBackBufferHeight, .1f, 10000));

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            effect = new BasicEffect(GraphicsDevice);

            ring = new Ring(GraphicsDevice, effect);

            position = new Vector3(0, 0, 8); //position of shape

#if WINDOWS
            Mouse.SetPosition(200, 200); //This helps the camera stay on track even if the mouse is offset during startup.
#endif

            base.Initialize();

            GraphicsDevice.RasterizerState = RasterizerState.CullNone; //have to be careful with vertices, declare vertices in clockwise fashion

            skysphere = Content.Load<Model>("skysphere");
            camera = new Camera2(_graphics, skysphere);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //skybox = new Skybox("SunInSpace", Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera.Update(gameTime);

            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            rotationY += deltatime;

            //angle += 0.002f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.UnitY);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            //View Matrix –> Camera Location | Projection Matrix –> Camera Lens | World Matrix –> Object Position/Orientation in 3D Scene
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.Draw(gameTime, ring, GraphicsDevice, effect, rotationY);

            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5), Vector3.Forward, Vector3.Up); //where camera is looking, its "up"

            /*foreach (ModelMesh mesh in skysphere.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
                    //effect.World = Matrix.CreateScale(0.3f * aspectRatio, 0.5f * aspectRatio, 0.5f) * Matrix.CreateRotationY(rotationY); //makes the skybox SPIN!
                    //effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -0.5f), Vector3.Forward, Vector3.Up); //makes a ring
                    effect.World = Matrix.CreateScale(0.3f * aspectRatio, 0.5f * aspectRatio, 0.5f) * Matrix.CreateTranslation(0, 0, 1);
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    //effect.World *= Matrix.CreateScale(0.99f); //this makes a cool effect
                }
                effect.World = Matrix.CreateScale(0.5f);
                mesh.Draw();
            }*/

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f);
            effect.World = Matrix.Identity * Matrix.CreateRotationY(rotationY) * Matrix.CreateTranslation(position); //how the object should be drawn out, local -> world transform                                                                                                //effect.World = Matrix.Identity;
            //effect.World = Matrix.Identity;

            effect.VertexColorEnabled = true;

            //ring.Draw(GraphicsDevice, effect);

            base.Draw(gameTime);
        }
    }
}