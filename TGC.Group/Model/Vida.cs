using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Vida
    {
        private TgcMesh tgcBotHead { get; set; }
        private Vector3 position { get; set; }

        public Vida(string modelPath, Vector3 newPosition)
        {
            tgcBotHead = new TgcSceneLoader().loadSceneFromFile(modelPath).Meshes[0];
            tgcBotHead.Position = newPosition;
            tgcBotHead.UpdateMeshTransform();
        }
    }
}
