using BEPUphysics.Entities.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using BEPUutilities;

using Vector3 = BEPUutilities.Vector3;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionTests;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using TheGreatSpaceRace;

namespace Space_Race.Example
{
    internal class Trigger
    {
        Box Box;
        public Trigger(Game1 game)
        {
            //Create static box of size 1x1x1 at origin
            //Box = new(Vector3.Zero, 1, 1, 1);
            //Add Box to physics space
            //game.Space.Add(Box);
            //Disable solver to make box generate collision events but no affect physics (like a trigger in unity)
            //More about collision rules: https://github.com/bepu/bepuphysics1/blob/master/Documentation/CollisionRules.md
            Box.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
            //Add collision start listener
            //More about collision events: https://github.com/bepu/bepuphysics1/blob/master/Documentation/CollisionEvents.md
            Box.CollisionInformation.Events.ContactCreated += CollisionHappened;
        }
        //Handle collision events
        void CollisionHappened(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            Console.WriteLine("Collision detected.");
        }
    }
}
