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
    class InferiorLeft : Parcela
    {
        //TODO: manejar todos los mesh de cada parcela en una una coleccion
        public InferiorLeft(TGCVector3 Position, string grassTexture, string wallTexture, string columnTexture, string topTexture, TgcMesh plantModel)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            floor = new TgcPlane(Position, new TGCVector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4).toMesh("floora");

            //Variable temporal que contiene el modelo
            var basePlant = plantModel;
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 45);
            basePlant.Scale = new TGCVector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda2");
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda4");
            basePlant.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo0");
            basePlant.Position = new TGCVector3(Position.X + 40, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo2");
            basePlant.Position = new TGCVector3(Position.X + 20, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo4");
            basePlant.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.RotateY(ran);
            basePlant.UpdateMeshTransform();
            plants.Add(basePlant);

            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 50), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 5, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);
            wallMesh = baseWall.toMesh("WallB");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 5);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            //Columnas
            var colTexture = TgcTexture.createTexture(columnTexture);

            var column = new Column
            {
                Position = this.Position
            };

            columns.AddRange(column.CreateColumn(colTexture, TGCVector3.Empty));

            columns.AddRange(column.CreateColumn(colTexture, new TGCVector3(0, 0, 45)));

            columns.AddRange(column.CreateColumn(colTexture, new TGCVector3(45, 0, 0)));

            columns.AddRange(column.CreateColumn(colTexture, new TGCVector3(45, 0, 45)));

            //Tapas de columnas
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 0, 5), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(topTexture), 1, 1);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y + 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y + 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);
        }
    }
}
