using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using System;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using BEPUphysics;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace TheGreatSpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont dfont;

        Ring[] rings = new Ring[10];
        bool[] ringStatus = new bool[10];
        int ringCurrent = 0;
        int ringsMissed = 0;
        int lap = 0;
        //Ring ring;

        //Skybox skybox;
        Model skysphere;
        Model spaceship;
        Matrix world = Matrix.Identity;
        Matrix view = Matrix.CreateLookAt(new Vector3(20, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
        Vector3 cameraPosition;
        float angle = 0;
        float distance = 20;

        Camera camera;

        BasicEffect effect; //lighting and shading

        Vector3 position;
        float rotationY;

        Space space;

        bool ranIntoCy1 = false;
        bool ranIntoCy2 = false;
        String states = "";
        int finalScore;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            effect = new BasicEffect(GraphicsDevice);

            position = new Vector3(0, 0, 8); //position of shape

#if WINDOWS
            Mouse.SetPosition(200, 200); //This helps the camera stay on track even if the mouse is offset during startup.
#endif

            base.Initialize();

            GraphicsDevice.RasterizerState = RasterizerState.CullNone; //have to be careful with vertices, declare vertices in clockwise fashion

            skysphere = Content.Load<Model>("skysphere");
            spaceship = Content.Load<Model>("spaceship");

            Model sphere = Content.Load<Model>("collision_test_sphere");
            space = new Space();
            Services.AddService(space); //now can get from anywhere that sees Game class
            Random rand = new Random();
            for (int i = 0; i < rings.Length; i++)
            {
                rings[i] = new Ring(new Vector3(rand.Next(-100, 100), rand.Next(-5, 5), rand.Next(-100, 100)), sphere, this);
                rings[i].ring = Content.Load<Model>("ring");
            }
            rings[0].ring = Content.Load<Model>("ringNext");
            camera = new Camera(_graphics, skysphere, spaceship, this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            dfont = Content.Load<SpriteFont>("posdisplayfont");

        }

        protected override void Update(GameTime gameTime)
        {
            space.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            camera.Update(gameTime);

            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            rotationY += deltatime;

            //angle += 0.002f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.UnitY);

            foreach (Ring ring in rings)
            {
                for (int i = 0; i < ring.hitBoxes.Length; i++)
                {
                    if (camera.ship.CollisionInformation.BoundingBox.Intersects(ring.hitBoxes[i].CollisionInformation.BoundingBox))
                    {
                        camera.ship.ApplyImpulse(camera.ship.WorldTransform.Translation, new BEPUutilities.Vector3(0, 0, 0.0001f)); //wake up
                        BEPUutilities.Vector3 v = camera.ship.WorldTransform.Backward;
                        camera.ship.ApplyLinearImpulse(ref v);
                        //System.Diagnostics.Debug.WriteLine("Ran into hitbox " + i +" at " + ring.hitBoxes[i].Position + " owned by ring at " + ring.pos);
                    }
                }

                //now for the cylinders
                if (camera.ship.CollisionInformation.BoundingBox.Intersects(ring.holeBounds[0].CollisionInformation.BoundingBox) && //2
                    camera.ship.CollisionInformation.BoundingBox.Intersects(ring.holeBounds[1].CollisionInformation.BoundingBox))
                {
                    //System.Diagnostics.Debug.WriteLine("Ran into both cys owned by ring at " + ring.pos);
                    ranIntoCy1 = true;
                    ranIntoCy2 = true;
                    manageStates(ring, '2', gameTime);
                }
                else if (camera.ship.CollisionInformation.BoundingBox.Intersects(ring.holeBounds[0].CollisionInformation.BoundingBox)) //1
                {
                    //System.Diagnostics.Debug.WriteLine("Ran into cy 0 at " + ring.holeBounds[0].Position + " owned by ring at " + ring.pos);
                    ranIntoCy1 = true;
                    ranIntoCy2 = false;
                    manageStates(ring, '1', gameTime);

                }
                else if (camera.ship.CollisionInformation.BoundingBox.Intersects(ring.holeBounds[1].CollisionInformation.BoundingBox)) //3
                {
                    //System.Diagnostics.Debug.WriteLine("Ran into cy 1 at " + ring.holeBounds[1].Position + " owned by ring at " + ring.pos);
                    ranIntoCy1 = false;
                    ranIntoCy2 = true;
                    manageStates(ring, '3', gameTime);
                }
                else //0
                {
                    ranIntoCy1 = false;
                    ranIntoCy2 = false;
                    //manageStates(ring, '0');
                }

            }

                base.Update(gameTime);
        }

        private void manageStates(Ring r, char state, GameTime gameTime)
        {
            if (states.Length < 3)
            {
                states += state;
                return;
            }
            else if (state == states[2]) //leave it alone if the state hasn't changed between updates
            {
                return;
            }

            states += state;
            if (states.Length > 3)
            {
                states = states.Substring(1, 3);
            }
            if (states.Equals("123") || states.Equals("321") && r == rings[ringCurrent])
            {
                ringStatus[ringCurrent] = true;
                rings[ringCurrent].state = 1;
                rings[ringCurrent].ring = Content.Load<Model>("ringComplete");
                ringCurrent++;
            }
            else if (states.Equals("123") || states.Equals("321") && r.state != 1) {
                int i;
                System.Diagnostics.Debug.WriteLine("Old ring current " + ringCurrent);
                for (i = ringCurrent; i < rings.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine("Skipped ring #" + i);
                    rings[i].ring = Content.Load<Model>("ringComplete");
                    rings[i].state = 1;
                    ringStatus[i] = true;
                    if (r == rings[i])
                    {
                        ringsMissed += i - ringCurrent;
                        ringCurrent = i;
                        break;
                    }

                }

                System.Diagnostics.Debug.WriteLine("New ring current " + ringCurrent);
            }

            if (ringCurrent >= rings.Length)
                lapFinished(gameTime);
            else
                rings[ringCurrent].ring = Content.Load<Model>("ringNext");
            System.Diagnostics.Debug.WriteLine(states);
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
                
            if (lap < 2)
            {

                _spriteBatch.DrawString(dfont, "Time Left:: " + (int)(500f - gameTime.TotalGameTime.TotalSeconds) + "s.", new System.Numerics.Vector2(50, 450), Color.White);
                _spriteBatch.DrawString(dfont, "Rings Missed:: " + ringsMissed, new System.Numerics.Vector2(200, 450), Color.White);
                _spriteBatch.DrawString(dfont, "Lap:: " + lap, new System.Numerics.Vector2(350, 450), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(dfont, "Rings Missed = -25 for every ring, missed " + ringsMissed + " rings", new System.Numerics.Vector2(100, 200), Color.White);
                _spriteBatch.DrawString(dfont, "Time Left:: " + (int)(500f - gameTime.TotalGameTime.TotalSeconds) + "s.", new System.Numerics.Vector2(100, 225), Color.White);
                
                _spriteBatch.DrawString(dfont, "Final Score:: " + finalScore, new System.Numerics.Vector2(100, 250), Color.White);
                _spriteBatch.DrawString(dfont, "Press escape to quit", new System.Numerics.Vector2(100, 275), Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void lapFinished(GameTime gameTime)
        {
            lap++;
            System.Diagnostics.Debug.WriteLine("Entered lap " + lap);
            for (int i = 0; i < ringStatus.Length; i++)
            {
                ringStatus[i] = false;
                rings[i].ring = Content.Load<Model>("ring");
                rings[i].state = 0;
            }
            ringCurrent = 0;
            rings[ringCurrent].ring = Content.Load<Model>("ringNext");
            finalScore = (int)(500f - gameTime.TotalGameTime.TotalSeconds) - (ringsMissed * 25);
        }

    }
}