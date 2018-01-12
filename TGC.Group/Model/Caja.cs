using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Caja
    {
        private TgcBox box { get; set; }
        private float OriginalPosYBox { get; set; }
        private bool BoxTaked { get; set; }
        
        /// <summary>
        ///     Creacion de la caja a partir de una posicion y la ruta de una textura
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texturePath"></param>
        public Caja(Vector3 position, string texturePath)
        {
            box = new TgcBox();
            box.setTexture(TgcTexture.createTexture(texturePath));
            box.Size = new Vector3(3, 3, 3);
            box.AutoTransformEnable = true;
            OriginalPosYBox = position.Y;
            box.Position = position;
            BoxTaked = false;
            box.updateValues();
        }

        /// <summary>
        ///     Animacion de oscilacion vertical y rotacion sobre el eje Y 
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="acumTime"></param>
        public void animateBox(float elapsedTime, float acumTime)
        {
            box.rotateY(elapsedTime);
            box.Position = new Vector3(box.Position.X, OriginalPosYBox + 3 * FastMath.Sin(acumTime), box.Position.Z);
            box.updateValues();
        }

        /// <summary>
        ///     Verifica si el jugador tomo o no la caja y modifica el atributo BoxTaked
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        public void takeBox(TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            if (TGC.Core.Collision.TgcCollisionUtils.testAABBAABB(characterBoundingbox, box.BoundingBox)) BoxTaked = true;
        }

        /// <summary>
        ///     Renderiza la caja 
        /// </summary>
        public void render()
        {
            if(!BoxTaked) box.render();
        }

        /// <summary>
        ///     Libera los recursos asociados a Caja
        /// </summary>
        public void dispose()
        {
            box.dispose();
        }
    }
}
