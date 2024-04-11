using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities;
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
    internal class Ring2 : GameComponent
    {
        public Model ring;

        public Vector3 pos;

        public int state;

        Entity allHits;

        Sphere[] hitsBoxes = new Sphere[20];

        int numSpheres = 20;
        float circleRadius = 5f; // Adjust as needed
        float angleIncrement = MathHelper.TwoPi / 20;

        List<CompoundShapeEntry> shapeEntries = new List<CompoundShapeEntry>();

        //Entity compoundBody = //list of all physicsbodies (compound shape entries) //list of 20 or so spheres basically in radius of ring (pos is center)
        //make function that builds ring of sphere, compound all those together
        //Math.PI * 2 // num sphere +-ijjidi cos/sin functions for unit circle
        //after building the compoundbody, use separate Trigger Entity to make event happen upon collision, probably use cylinder on side instead of box

        public Ring2(Vector3 pos, Game game) : base(game)
        {
            this.pos = pos;
            state = 0;
            for (int i = 0; i < numSpheres; i++)
            {
                float angle = i * angleIncrement;
                float x = circleRadius * (float)Math.Cos(angle);
                float y = circleRadius * (float)Math.Sin(angle);

                BEPUutilities.Vector3 posBepu = new BEPUutilities.Vector3(pos.X, pos.Y, pos.Z);
                Sphere s = new(new BEPUutilities.Vector3(pos.X, pos.Y, pos.Z), 10, 1);
                hitsBoxes[i] = s;

                // Create the compound shape entry for the sphere
                CompoundShapeEntry shapeEntry = new CompoundShapeEntry(new SphereShape(circleRadius), posBepu, 1f);

                // Add the shape entry to the list
                shapeEntries.Add(shapeEntry);
            }
            foreach (Sphere hitbox in hitsBoxes) {
                //((Space)Game.Services.GetService(typeof(Space))).Add(hitbox);
                //hitbox.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                //hitbox.CollisionInformation.Events.ContactCreated += CollisionHappened;
            }

            // Create the compound body using the list of shape entries
            CompoundBody compoundBody = new CompoundBody(shapeEntries);
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