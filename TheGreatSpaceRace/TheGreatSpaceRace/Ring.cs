using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionShapes;
using System.Collections.Generic;
using BEPUphysics.CollisionShapes.ConvexShapes;
using System;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionTests;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionRuleManagement;

namespace TheGreatSpaceRace
{
    internal class Ring : GameComponent
    {
        public Model ring;

        public Model sphere;

        public Vector3 pos;

        public int state;

        public Sphere[] hitBoxes = new Sphere[10]; //more than 10 can get laggy

        public Cylinder[] holeBounds = new Cylinder[2];

        int numSpheres = 10;
        float circleRadius = 13f;
        float angleIncrement = MathHelper.TwoPi / 10;

        List<CompoundShapeEntry> shapeEntries = new List<CompoundShapeEntry>();

        public CompoundBody compoundBody;

        public Ring(Vector3 pos, Model sphere, Game game) : base(game)
        {
            this.pos = pos;
            state = 0;
            this.sphere = sphere;
            for (int i = 0; i < numSpheres; i++)
            {
                float angle = i * angleIncrement;
                float x = circleRadius * (float)Math.Cos(angle);
                float y = circleRadius * (float)Math.Sin(angle);

                BEPUutilities.Vector3 posBepu = new BEPUutilities.Vector3(pos.X + x, pos.Y + y, pos.Z);
                Sphere s = new(posBepu, 1, 1);
                hitBoxes[i] = s;

                CompoundShapeEntry shapeEntry = new CompoundShapeEntry(new SphereShape(circleRadius), posBepu, 1f);

                shapeEntries.Add(shapeEntry);
            }
            foreach (Sphere hitbox in hitBoxes) {
                ((Space)Game.Services.GetService(typeof(Space))).Add(hitbox);
                hitbox.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                hitbox.CollisionInformation.Events.ContactCreated += CollisionHappened;
            }

            compoundBody = new CompoundBody(shapeEntries);


       
                BEPUutilities.Vector3 posBepu2 = new BEPUutilities.Vector3(pos.X, pos.Y + 2, pos.Z + 2);
                holeBounds[0] = new Cylinder(posBepu2, 20, 15);

                ((Space)Game.Services.GetService(typeof(Space))).Add(holeBounds[0]);
                holeBounds[0].CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                holeBounds[0].CollisionInformation.Events.ContactCreated += CollisionHappened;

                posBepu2 = new BEPUutilities.Vector3(pos.X, pos.Y + 2, pos.Z - 2);
                holeBounds[1] = new Cylinder(posBepu2, 20, 15); ;

                ((Space)Game.Services.GetService(typeof(Space))).Add(holeBounds[1]);
                holeBounds[1].CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                holeBounds[1].CollisionInformation.Events.ContactCreated += CollisionHappened;


            compoundBody = new CompoundBody(shapeEntries);

        }

        void CollisionHappened(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            Console.WriteLine("Collision detected.");
        }

        public int checkShipState(Vector3 cameraPosition)
        {
            return -1;
        }
    }
}