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

namespace TGC.Group.Model.Parcelas
{
    class SuperiorLeft : Parcela
    {
        public SuperiorLeft(Vector3 position, string grassTexture, string wallTexture, string plantModel)
        {
            //Se define el terrno de la parcela
            floor = new TgcPlane(position, new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4);

            //Variable temporal que contiene el modelo
            var basePlant = new TgcSceneLoader().loadSceneFromFile(plantModel).Meshes[0];
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 45);
            basePlant.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 100);
            basePlant.rotateY(ran);
            basePlant.Enabled = true;

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda1");
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 35);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda2");
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda3");
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 15);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda4");
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            var baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20, 50), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = position;
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(50, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);
            wallMesh = baseWall.toMesh("WallB");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z+50);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);
        }
    }
}
