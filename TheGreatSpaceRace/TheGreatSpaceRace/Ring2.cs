using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TheGreatSpaceRace
{
    /// <summary>
    /// Handles all of the aspects of working with a skybox.
    /// </summary>
    internal class Ring2
    {
        /// <summary>
        /// The skybox model, which will just be a cube
        /// </summary>
        public Model ring;

        public Vector3 pos;

        public Ring2(Vector3 pos)
        {
            this.pos = pos;
            //ring = Content.Load<Model>("skybox");
            //skyBoxTexture = Content.Load<TextureCube>(skyboxTexture);
            //skyBoxEffect = Content.Load<Effect>("SkyboxFX");
        }



        public int checkShipState(Vector3 cameraPosition)
        {
            return -1;
        }
    }
}