using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Parcela
    {
        //guardo todos los meshes de la parcela (paredes, cajas, vegetacion, etc.)
        protected List<TgcMesh> meshes = new List<TgcMesh>();

        //Posicion a partir de la cual se ubica todo
        protected TGCVector3 Position;

        public List<TgcMesh> GetAllMeshes()
        {
            return meshes;
        }

        public TGCVector3 GetPosition()
        {
            return Position;
        }

        public bool IsInParcela(TGCVector3 player)
        {
            return Position.X < player.X &&
                Position.Z < player.Z &&
                Position.X + 50 > player.X &&
                Position.Z + 50 > player.Z;
        }

        public bool IsInPit(TGCVector3 player)
        {
            return Position.X + 5 < player.X &&
                    Position.Z + 5 < player.Z &&
                    Position.X + 45 > player.X &&
                    Position.Z + 45 > player.Z &&
                    player.Y < 0;
        }

        public void Render()
        {
            foreach(var mesh in meshes)
            {
                mesh.Render();
            }
        }

        public void Dispose()
        {
            foreach (var mesh in meshes)
            {
                mesh.Dispose();
            }
        }
    }
}
