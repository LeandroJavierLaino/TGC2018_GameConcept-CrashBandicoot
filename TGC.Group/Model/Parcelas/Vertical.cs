using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Core.Mathematica;

namespace TGC.Group.Model.Parcelas
{
    class Vertical : Parcela
    {
        //TODO: manejar todos los mesh de cada parcela en una una coleccion
        public Vertical(TGCVector3 Position, TgcTexture grassTexture, TgcTexture wallTexture, TgcTexture columnTexture, TgcTexture topTexture, TgcMesh plantModel)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            var floor = new TgcPlane(Position, new TGCVector3(50, 0, 50), TgcPlane.Orientations.XZplane, grassTexture, 4, 4).toMesh("floor");
            meshes.Add(floor);

            #region Plantas
            //Variable temporal que contiene el modelo
            var basePlant = plantModel;
            basePlant.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 45);
            basePlant.Scale = new TGCVector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda2");
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda4");
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha1");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 45);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha3");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha5");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            #endregion

            #region Paredes
            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 50), TgcPlane.Orientations.YZplane, wallTexture, 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();

            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(-FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();

            meshes.Add(wallMesh);
            #endregion

            #region Columnas
            //Columnas 

            var column = new Column
            {
                Position = this.Position
            };

            meshes.AddRange(column.CreateColumn(columnTexture, TGCVector3.Empty));

            meshes.AddRange(column.CreateColumn(columnTexture, new TGCVector3(0, 0, 45)));

            meshes.AddRange(column.CreateColumn(columnTexture, new TGCVector3(45, 0, 0)));

            meshes.AddRange(column.CreateColumn(columnTexture, new TGCVector3(45, 0, 45)));
             #endregion

            //Tapas de columnas
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 0, 5), TgcPlane.Orientations.XZplane, topTexture, 1, 1);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y + 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y + 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

        }
        
    }
}
