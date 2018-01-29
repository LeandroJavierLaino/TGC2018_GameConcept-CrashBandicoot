using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model.Enviroment_Objects
{
    class Tower
    {
        //Posicion de la torre
        private Vector3 position;

        //Paredes de la torre, tambien pueden ser columnas
        private List<TgcMesh> templeWalls = new List<TgcMesh>();

        public Tower(string wallTexture, Vector3 newPosition)
        {
            this.position = newPosition;

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseTriangleWallH = new TgcPlane(new Vector3(), new Vector3(50, 40f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);
        }
    }
}
