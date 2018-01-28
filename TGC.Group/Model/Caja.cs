using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Shaders;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Caja
    {
        private TgcBox box { get; set; }
        private float OriginalPosYBox { get; set; }
        public bool BoxTaked { get; set; }
        public int boxQuantity = 0;

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

        public void applyEffect(Effect effect)
        {
            box.Effect = effect;
            box.Technique = "BOX_DIFFUSE_MAP";
        }

        /// <summary>
        ///     Verifica si el jugador tomo o no la caja y modifica el atributo BoxTaked
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        public void takeBox(TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            if (isColliding(characterBoundingbox) && boxQuantity == 0)
            {
                boxQuantity = 1;
                BoxTaked = true;
            }
        }

        /// <summary>
        /// Verifica si hay colision entre el personaje y la caja
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        /// <returns></returns>
        public bool isColliding(TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            return TGC.Core.Collision.TgcCollisionUtils.testAABBAABB(characterBoundingbox, box.BoundingBox);
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
