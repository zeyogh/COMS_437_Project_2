using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TheGreatSpaceRace
{
    internal class Ring2
    {
        public Model ring;

        public Vector3 pos;

        public int state;

        //Entity compoundBody = //list of all physicsbodies (compound shape entries) //list of 20 or so spheres basically in radius of ring (pos is center)
        //make function that builds ring of sphere, compound all those together
        //Math.PI * 2 // num sphere +-ijjidi cos/sin functions for unit circle
        //after building the compoundbody, use separate Trigger Entity to make event happen upon collision, probably use cylinder on side instead of box

        public Ring2(Vector3 pos)
        {
            this.pos = pos;
            state = 0;
        }

        public int checkShipState(Vector3 cameraPosition)
        {
            return -1;
        }
    }
}