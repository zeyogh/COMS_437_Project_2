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

        VertexPositionColor[] vertices; //color of a vertex
        BasicEffect effect; //lighting and shading
        VertexBuffer buffer; //take vertices and pass them to GPU for more efficient read

        Vector3 position;
        float rotationY;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            vertices = new VertexPositionColor[3]
            {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(-1, -1, 0), Color.Green),
                new VertexPositionColor(new Vector3(1, -1, 0), Color.Blue)
            };

            effect = new BasicEffect(GraphicsDevice);

            buffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 3, BufferUsage.WriteOnly);

            buffer.SetData(vertices);

            position = new Vector3(0, 0, 5); //position of shape

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

            effect.VertexColorEnabled = true;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1); //1 triangle, no offset


            }

            base.Draw(gameTime);
        }
    }
}