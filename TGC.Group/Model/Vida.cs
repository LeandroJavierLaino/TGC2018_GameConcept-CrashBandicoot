using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    class Vida
    {
        private TgcMesh tgcBotHead { get; set; }
        private Vector3 position { get; set; }
        public int liveQuantity { get; set; }
        private bool LiveTaked { get; set; }

        public Vida(string modelPath, Vector3 newPosition)
        {
            tgcBotHead = new TgcSceneLoader().loadSceneFromFile(modelPath).Meshes[0];
            tgcBotHead.Position = newPosition;
            tgcBotHead.Scale = new Vector3(0.15f, 0.15f, 0.15f);
            tgcBotHead.UpdateMeshTransform();

            liveQuantity = 0;
            LiveTaked = false;
        }

        /// <summary>
        ///     Verifica si el jugador tomo o no la vida y modifica el atributo LiveTaked
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        public void takeLive(Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            if (isColliding(characterBoundingbox) && liveQuantity == 0)
            {
                liveQuantity = 1;
                LiveTaked = true;
            }
        }

        /// <summary>
        /// Verifica si hay colision entre el personaje y el modelo de vida
        /// </summary>
        /// <param name="characterBoundingbox"></param>
        /// <returns></returns>
        public bool isColliding(Core.BoundingVolumes.TgcBoundingAxisAlignBox characterBoundingbox)
        {
            return Core.Collision.TgcCollisionUtils.testAABBAABB(characterBoundingbox, tgcBotHead.BoundingBox);
        }

        public void applyShader(Effect shader)
        {
            tgcBotHead.Effect = shader;
            tgcBotHead.Technique = "HEAD_DIFFUSE_MAP";
        }

        public void render()
        {
           if(!LiveTaked) tgcBotHead.render();
        }

        public void dispose()
        {
            tgcBotHead.dispose();
        }
    }
}
