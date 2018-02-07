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

namespace TGC.Group.Model.Parcelas
{
    class Vertical : Parcela
    {
        public Vertical(Vector3 position, string grassTexture, string wallTexture, string columnTexture, string topTexture, string plantModel)
        {
            this.position = position;
            //Se define el terrno de la parcela
            floor = new TgcPlane(position, new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4).toMesh("floor");

            #region Plantas
            //Variable temporal que contiene el modelo
            var basePlant = new TgcSceneLoader().loadSceneFromFile(plantModel).Meshes[0];
            basePlant.Position = new Vector3(position.X, position.Y, position.Z + 45);
            basePlant.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 100);
            basePlant.rotateY(ran);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda1");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z + 35);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda2");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda3");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z + 15);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaizquierda4");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha1");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 45);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha2");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 35);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha3");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 25);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha4");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 15);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaderecha5");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 5);
            ran = random.Next(0, 100);
            basePlant.rotateY(ran);

            plants.Add(basePlant);
            #endregion

            #region Paredes
            var baseWall = new TgcPlane(new Vector3(),new Vector3(0, 20.62f, 50),TgcPlane.Orientations.YZplane,TgcTexture.createTexture(wallTexture),2,1);
            
            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.rotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.rotateZ(-FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new Vector3(position.X + 45,position.Y,position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);
            #endregion

            #region Columnas
            //Columnas 
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2H1");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2H2");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column3V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column3H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column3H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column4V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column4H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column4H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);
#endregion

            //Tapas de columnas
            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 0, 5), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(topTexture), 1, 1);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new Vector3(position.X, position.Y + 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y + 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new Vector3(position.X + 45, position.Y + 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new Vector3(position.X, position.Y + 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

        }
        
    }
}
