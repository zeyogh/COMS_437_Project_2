using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using System;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using BEPUphysics;

namespace TheGreatSpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont dfont;

        //Ring[] rings = new Ring[1];
        Ring2[] rings = new Ring2[7];
        bool[] ringStatus = new bool[7];
        int ringCurrent = 0;
        int lap = 0;
        //Ring ring;

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

        bool quit = false;

        Space space;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //camera = new Camera(BEPUutilities.Vector3.Zero, 0, 0, BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, Graphics.PreferredBackBufferWidth / (float)Graphics.PreferredBackBufferHeight, .1f, 10000));

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            effect = new BasicEffect(GraphicsDevice);

            //ring = new Ring(GraphicsDevice, effect);
            /*for (int i = 0; i < rings.Length; i++)
            {
                rings[i] = new Ring(GraphicsDevice, effect, i, 0, 0, i + 10 + (i * 4));
            }*/

            position = new Vector3(0, 0, 8); //position of shape

#if WINDOWS
            Mouse.SetPosition(200, 200); //This helps the camera stay on track even if the mouse is offset during startup.
#endif

            base.Initialize();

            GraphicsDevice.RasterizerState = RasterizerState.CullNone; //have to be careful with vertices, declare vertices in clockwise fashion

            skysphere = Content.Load<Model>("skysphere");
            Random rand = new Random();
            for (int i = 0; i < rings.Length; i++)
            {
                rings[i] = new Ring2(new Vector3(rand.Next(-100, 100), rand.Next(-100, 100), rand.Next(-100, 100)));
                rings[i].ring = Content.Load<Model>("ring");
            }
            rings[0].ring = Content.Load<Model>("ringNext");
            space = new Space();
            Services.AddService(space); //now can get from anywhere that sees Game class
            camera = new Camera2(_graphics, skysphere, this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            dfont = Content.Load<SpriteFont>("posdisplayfont");

            //skybox = new Skybox("SunInSpace", Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            space.Update();
            lapFinished();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || quit)
                Exit();

            checkRings();

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

            camera.Draw(gameTime, rings, GraphicsDevice, effect, rotationY);

            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5), Vector3.Forward, Vector3.Up); //where camera is looking, its "up"

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f);
            effect.World = Matrix.Identity * Matrix.CreateRotationY(rotationY) * Matrix.CreateTranslation(position); //how the object should be drawn out, local -> world transform

            effect.VertexColorEnabled = true;

            _spriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
                
            if (lap < 3)
            {

                _spriteBatch.DrawString(dfont, "Time Left:: " + (int)(500f - gameTime.TotalGameTime.TotalSeconds) + "s.", new System.Numerics.Vector2(50, 450), Color.White);
                _spriteBatch.DrawString(dfont, "Rings Missed:: " + (int)(500f - gameTime.TotalGameTime.TotalSeconds) + "s.", new System.Numerics.Vector2(50, 450), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(dfont, "Time Left:: " + (int)(500f - gameTime.TotalGameTime.TotalSeconds) + "s.", new System.Numerics.Vector2(50, 450), Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool lapFinished()
        {
            if (rings[rings.Length - 1].checkShipState(Camera2.convertVector3ToXNA(camera.ship.Position)) < 2)
            {
                return false;
            }
            lap++;
            for (int i = 0; i < ringStatus.Length; i++)
            {
                ringStatus[i] = false;
                rings[i].state = 0;
            }
            rings[0].ring = Content.Load<Model>("ringNext");
            return true;
        }

        public void checkRings()
        {
            foreach (Ring2 ring in rings)
            {
                int state = ring.checkShipState(Camera2.convertVector3ToXNA(camera.ship.Position));
                if (ring == rings[ringCurrent] && state >= 2)
                {
                    rings[ringCurrent].ring = Content.Load<Model>("ringComplete");
                    ringCurrent++;
                    rings[ringCurrent].ring = Content.Load<Model>("ringNext");
                }
            }
        }

    }
}