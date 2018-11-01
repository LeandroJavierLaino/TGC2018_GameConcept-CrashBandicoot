using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Caja
    {
        private TgcMesh Box { get; set; }
        private float OriginalPosYBox { get; set; }
        public bool BoxTaked { get; set; }
        public int boxQuantity = 0;

        /// <summary>
        ///     Creacion de la caja a partir de una posicion y la ruta de una textura
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texturePath"></param>
        public Caja(TGCVector3 position, TgcMesh boxMesh)
        {
            Box = boxMesh.clone("boxClone");
            Box.Position = position;
            Box.Transform = TGCMatrix.Translation(Box.Position);
        }

        public void ApplyEffect(Effect effect)
        {
            Box.Effect = effect;
            Box.Technique = "BOX_DIFFUSE_MAP";
        }

        /// <summary>
        ///     Verifica si el jugador tomo o no la caja y modifica el atributo BoxTaked
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        public void TakeBox(TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            if (IsColliding(characterBoundingbox) && boxQuantity == 0)
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
        public bool IsColliding(TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            return TGC.Core.Collision.TgcCollisionUtils.testAABBAABB(characterBoundingbox, Box.BoundingBox);
        }

        /// <summary>
        ///     Renderiza la caja 
        /// </summary>
        public void Render()
        {
            if(!BoxTaked) Box.Render();
        }

        /// <summary>
        ///     Libera los recursos asociados a Caja
        /// </summary>
        public void Dispose()
        {
            Box.Dispose();
        }
    }
}
