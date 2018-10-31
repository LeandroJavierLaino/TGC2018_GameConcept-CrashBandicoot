using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model.Parcelas
{
    class PitH : Parcela
    {
        public PitH(TGCVector3 Position, TgcPlane grassPlane, string wallTexture, string columnTexture, string topTexture)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            var grassMesh = grassPlane.toMesh("floor");
            grassMesh.Position = new TGCVector3(this.Position.X, this.Position.Y - 20, this.Position.Z);
            grassMesh.Transform = TGCMatrix.Translation(grassMesh.Position);
            meshes.Add(grassMesh);

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseTriangleWallH = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z + 5);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallHB");
            wallMesh.RotateX(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 50), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 5, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(-FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z+50);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);
            
            //Columnas 
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(Position.X + 5, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z + 5);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2V1");
            wallMesh.Position = new TGCVector3(Position.X + 5, Position.Y - 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2H1");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2H2");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y - 20, Position.Z + 50);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3V1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3H1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z + 5);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3H2");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4V1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4H1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4H2");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y - 20, Position.Z + 50);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            //Tapas de columnas
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 0, 5), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(topTexture), 1, 1);

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
