using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheGreatSpaceRace
{
    internal class Ring
    {

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

        private int outerSize = 4;
        private int innerSize = 3;

        private int state = 0;

        private int order;

        public Ring(GraphicsDevice gd, BasicEffect basic, int order, int offsetX, int offsetY, int offsetZ)
        {
            this.order = order;

            verticesTop = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Red)
            };

            verticesBottom = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Green)
            };

            verticesLeft = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Blue)
            };

            verticesRight = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Yellow)
            };

            verticesTopBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Red),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Red)
            };

            verticesBottomBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Green),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Green)
            };

            verticesLeftBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Blue),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Blue)
            };

            verticesRightBack = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Yellow),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Yellow)
            };

            verticesTopEdgeOuter = new VertexPositionColor[4] //must be clockwise
{
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.White)
};

            verticesBottomEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.White)
            };

            verticesLeftEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Black),
            };

            verticesRightEdgeOuter = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, outerSize+offsetY, -1+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-outerSize+offsetX, -outerSize+offsetY, -1+offsetZ), Color.Black),
            };

            verticesTopEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.White)
            };

            verticesBottomEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.White),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.White)
            };

            verticesLeftEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Black),
            };

            verticesRightEdgeInner = new VertexPositionColor[4] //must be clockwise
            {
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, innerSize+offsetY, -1+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, 0+offsetZ), Color.Black),
                new VertexPositionColor(new Vector3(-innerSize+offsetX, -innerSize+offsetY, -1+offsetZ), Color.Black),
            };

            bufferTop = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTop.SetData(verticesTop);

            bufferBottom = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottom.SetData(verticesBottom);

            bufferLeft = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeft.SetData(verticesLeft);

            bufferRight = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRight.SetData(verticesRight);

            bufferTopBack = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopBack.SetData(verticesTopBack);

            bufferBottomBack = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomBack.SetData(verticesBottomBack);

            bufferLeftBack = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftBack.SetData(verticesLeftBack);

            bufferRightBack = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightBack.SetData(verticesRightBack);

            bufferTopEdgeOuter = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopEdgeOuter.SetData(verticesTopEdgeOuter);

            bufferBottomEdgeOuter = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomEdgeOuter.SetData(verticesBottomEdgeOuter);

            bufferLeftEdgeOuter = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftEdgeOuter.SetData(verticesLeftEdgeOuter);

            bufferRightEdgeOuter = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightEdgeOuter.SetData(verticesRightEdgeOuter);

            bufferTopEdgeInner = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferTopEdgeInner.SetData(verticesTopEdgeInner);

            bufferBottomEdgeInner = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferBottomEdgeInner.SetData(verticesBottomEdgeInner);

            bufferLeftEdgeInner = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferLeftEdgeInner.SetData(verticesLeftEdgeInner);

            bufferRightEdgeInner = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bufferRightEdgeInner.SetData(verticesRightEdgeInner);
        }

        public void Draw(GraphicsDevice gd, BasicEffect effect)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTop, 0, 2); //2 triangles for a square, no offset
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottom, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeft, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRight, 0, 2);

                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopBack, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomBack, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftBack, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightBack, 0, 2);

                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopEdgeOuter, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomEdgeOuter, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftEdgeOuter, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightEdgeOuter, 0, 2);

                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesTopEdgeInner, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesBottomEdgeInner, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesLeftEdgeInner, 0, 2);
                gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verticesRightEdgeInner, 0, 2);

            }
        }

        public int checkShipState(Vector3 shipPosition)
        {
            System.Diagnostics.Debug.WriteLine("Ship: " + shipPosition + " vs front: " 
                + verticesTopBack[0].Position.Z + " vs back: " + verticesTop[0].Position.Z);
            if (Math.Abs(verticesTopBack[0].Position.Z - shipPosition.Z) < 0.001f && state == 0) //path through entrance
            {
                state++;
                System.Diagnostics.Debug.WriteLine("State shift from 0 to 1");
            }
            else if (verticesTopBack[0].Position.Z < shipPosition.Z
                     && verticesTop[0].Position.Z > shipPosition.Z
                     && state == 1) //inside ring
            {
                state++;
                System.Diagnostics.Debug.WriteLine("State shift from 1 to 2");
            }
            else if (Math.Abs(verticesTop[0].Position.Z - shipPosition.Z) < 0.001f && state == 2) //leaving ring
            {
                state++;
                System.Diagnostics.Debug.WriteLine("State shift from 2 to 3");
            }

            return state;
        }

    }
}