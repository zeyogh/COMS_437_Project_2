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

namespace TheGreatSpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Ring ring;


        BasicEffect effect; //lighting and shading


        Vector3 position;
        float rotationY;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            

            effect = new BasicEffect(GraphicsDevice);

            ring = new Ring(GraphicsDevice, effect);

            position = new Vector3(0, 0, 8); //position of shape

            base.Initialize();

            GraphicsDevice.RasterizerState = RasterizerState.CullNone; //have to be careful with vertices, declare vertices in clockwise fashion

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            rotationY += deltatime;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.001f, 1000f); //closest object can get to camera, farthest object can get to camera

            effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5), Vector3.Forward, Vector3.Up); //where camera is looking, its "up"

            effect.World = Matrix.Identity * Matrix.CreateRotationY(rotationY) * Matrix.CreateTranslation(position); //how the object should be drawn out, local -> world transform
            //effect.World = Matrix.Identity;

            effect.VertexColorEnabled = true;

            ring.Draw(GraphicsDevice, effect);

            base.Draw(gameTime);
        }
    }
}