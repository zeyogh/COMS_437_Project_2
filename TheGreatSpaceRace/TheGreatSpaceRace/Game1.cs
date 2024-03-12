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

        VertexPositionColor[] verticesTop; //color of a vertex
        VertexPositionColor[] verticesBottom;
        VertexPositionColor[] verticesLeft;
        VertexPositionColor[] verticesRight;
        VertexPositionColor[] verticesTopBack;
        VertexPositionColor[] verticesBottomBack;
        VertexPositionColor[] verticesLeftBack;
        VertexPositionColor[] verticesRightBack;
        VertexPositionColor[] verticesTopEdgeOuter;
        VertexPositionColor[] verticesBottomEdgeOuter;
        VertexPositionColor[] verticesLeftEdgeOuter;
        VertexPositionColor[] verticesRightEdgeOuter;
        VertexPositionColor[] verticesTopEdgeInner;
        VertexPositionColor[] verticesBottomEdgeInner;
        VertexPositionColor[] verticesLeftEdgeInner;
        VertexPositionColor[] verticesRightEdgeInner;


        BasicEffect effect; //lighting and shading

        VertexBuffer bufferTop; //take vertices and pass them to GPU for more efficient read
        VertexBuffer bufferBottom;
        VertexBuffer bufferLeft;
        VertexBuffer bufferRight;
        VertexBuffer bufferTopBack;
        VertexBuffer bufferBottomBack;
        VertexBuffer bufferLeftBack;
        VertexBuffer bufferRightBack;
        VertexBuffer bufferTopEdgeOuter;
        VertexBuffer bufferBottomEdgeOuter;
        VertexBuffer bufferLeftEdgeOuter;
        VertexBuffer bufferRightEdgeOuter;
        VertexBuffer bufferTopEdgeInner;
        VertexBuffer bufferBottomEdgeInner;
        VertexBuffer bufferLeftEdgeInner;
        VertexBuffer bufferRightEdgeInner;

        Vector3 position;
        float rotationY;

        int outerSize = 4;
        int innerSize = 3;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            verticesTop = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, outerSize, 0), Color.Red),
                new VertexPositionColor(new Vector3(outerSize, outerSize, 0), Color.Red),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, 0), Color.Red),
                new VertexPositionColor(new Vector3(innerSize, innerSize, 0), Color.Red)
            };

            verticesBottom = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, 0), Color.Green),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, 0), Color.Green),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, 0), Color.Green),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, 0), Color.Green)
            };

            verticesLeft = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize, outerSize, 0), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize, innerSize, 0), Color.Blue),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, 0), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, 0), Color.Blue)
            };

            verticesRight = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, outerSize, 0), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, 0), Color.Yellow),
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, 0), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, 0), Color.Yellow)
            };

            verticesTopBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, outerSize, -1), Color.Red),
                new VertexPositionColor(new Vector3(outerSize, outerSize, -1), Color.Red),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, -1), Color.Red),
                new VertexPositionColor(new Vector3(innerSize, innerSize, -1), Color.Red)
            };

            verticesBottomBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, -1), Color.Green),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, -1), Color.Green),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, -1), Color.Green),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, -1), Color.Green)
            };

            verticesLeftBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize, outerSize, -1), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize, innerSize, -1), Color.Blue),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, -1), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, -1), Color.Blue)
            };

            verticesRightBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, outerSize, -1), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, -1), Color.Yellow),
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, -1), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, -1), Color.Yellow)
            };

            verticesTopEdgeOuter = new VertexPositionColor[4] //must be clockwise
{
                new VertexPositionColor(new Vector3(-outerSize, outerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(-outerSize, outerSize, -1), Color.White),
                new VertexPositionColor(new Vector3(outerSize, outerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(outerSize, outerSize, -1), Color.White)
};

            verticesBottomEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, -1), Color.White),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, -1), Color.White)
            };

            verticesLeftEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize, outerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(outerSize, outerSize, -1), Color.Black),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(outerSize, -outerSize, -1), Color.Black),
            };

            verticesRightEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize, outerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize, outerSize, -1), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize, -outerSize, -1), Color.Black),
            };

            verticesTopEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize, innerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, -1), Color.White),
                new VertexPositionColor(new Vector3(innerSize, innerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(innerSize, innerSize, -1), Color.White)
            };

            verticesBottomEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, -1), Color.White),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, 0), Color.White),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, -1), Color.White)
            };

            verticesLeftEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(innerSize, innerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(innerSize, innerSize, -1), Color.Black),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(innerSize, -innerSize, -1), Color.Black),
            };

            verticesRightEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize, innerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize, innerSize, -1), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, 0), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize, -innerSize, -1), Color.Black),
            };

            effect = new BasicEffect(GraphicsDevice);

            bufferTop = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTop.SetData(verticesTop);

            bufferBottom = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottom.SetData(verticesBottom);

            bufferLeft = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeft.SetData(verticesLeft);

            bufferRight = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRight.SetData(verticesRight);

            bufferTopBack = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopBack.SetData(verticesTopBack);

            bufferBottomBack = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomBack.SetData(verticesBottomBack);

            bufferLeftBack = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftBack.SetData(verticesLeftBack);

            bufferRightBack = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightBack.SetData(verticesRightBack);

            bufferTopEdgeOuter = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopEdgeOuter.SetData(verticesTopEdgeOuter);

            bufferBottomEdgeOuter = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomEdgeOuter.SetData(verticesBottomEdgeOuter);

            bufferLeftEdgeOuter = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftEdgeOuter.SetData(verticesLeftEdgeOuter);

            bufferRightEdgeOuter = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightEdgeOuter.SetData(verticesRightEdgeOuter);

            bufferTopEdgeInner = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopEdgeInner.SetData(verticesTopEdgeInner);

            bufferBottomEdgeInner = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomEdgeInner.SetData(verticesBottomEdgeInner);

            bufferLeftEdgeInner = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftEdgeInner.SetData(verticesLeftEdgeInner);

            bufferRightEdgeInner = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightEdgeInner.SetData(verticesRightEdgeInner);

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

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTop, 0, 2); //2 triangles for a square, no offset
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottom, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeft, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRight, 0, 2);

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopBack, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomBack, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftBack, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightBack, 0, 2);

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopEdgeOuter, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomEdgeOuter, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftEdgeOuter, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightEdgeOuter, 0, 2);

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopEdgeInner, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomEdgeInner, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftEdgeInner, 0, 2);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightEdgeInner, 0, 2);

            }

            base.Draw(gameTime);
        }
    }
}