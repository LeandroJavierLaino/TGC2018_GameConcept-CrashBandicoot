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
        private List<TgcMesh> towerWalls = new List<TgcMesh>();

        public Tower(Vector3 newPosition, string wallTexture, string columnTexture)
        {
            this.position = newPosition;

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseWall = new TgcPlane(new Vector3(), new Vector3(40, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 4);

            var wallMesh = baseWall.toMesh("WallHA");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();

            towerWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();

            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 40), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 4);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();

            towerWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();

            towerWalls.Add(wallMesh);

            //Columnas
            //1
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            //2
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 50, position.Y, position.Z );
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            //3
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 50, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            //4
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 80, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 80, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 4);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            towerWalls.Add(wallMesh);
        }

        public void render()
        {
            foreach(var element in towerWalls)
            {
                element.render();
            }
        }
    }
}
