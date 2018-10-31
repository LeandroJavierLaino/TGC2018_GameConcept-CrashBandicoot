using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model.Parcelas
{
    class SuperiorRight : Parcela
    {
        //TODO: manejar todos los mesh de cada parcela en una una coleccion
        public SuperiorRight(TGCVector3 Position, TgcPlane grassPlane, TgcTexture wallTexture, TgcPlane columnPlaneX, TgcPlane columnPlaneZ, TgcPlane topPlane, TgcMesh plantModel)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            var grassMesh = grassPlane.toMesh("floor");
            grassMesh.Position = this.Position;
            grassMesh.Transform = TGCMatrix.Translation(grassMesh.Position);
            meshes.Add(grassMesh);

            //Variable temporal que contiene el modelo
            var basePlant = plantModel;
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 45);
            basePlant.Scale = new TGCVector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda2");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda4");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba1");
            basePlant.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 45);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba3");
            basePlant.Position = new TGCVector3(Position.X + 25, Position.Y, Position.Z + 45);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            meshes.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba5");
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 45);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            meshes.Add(basePlant);

            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 50), TgcPlane.Orientations.YZplane, wallTexture, 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20.62f, 0), TgcPlane.Orientations.XYplane, wallTexture, 2, 1);
            wallMesh = baseWall.toMesh("WallB");
            wallMesh.RotateX(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            //Columnas 
            var column = new Column
            {
                Position = this.Position
            };

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, TGCVector3.Empty));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(0, 0, 45)));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(45, 0, 0)));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(45, 0, 45)));

            //Tapas de columnas
            baseWall = topPlane; 

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

