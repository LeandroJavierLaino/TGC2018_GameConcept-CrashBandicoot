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
    class Fin : Parcela
    {
        public Fin(TGCVector3 Position, string grassTexture, string wallTexture, string columnTexture, string topTexture)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            floor = new TgcPlane(Position, new TGCVector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4).toMesh("floor");

            #region Paredes
            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20f, 50), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");
            wallMesh.Position = new TGCVector3(Position.X + 50, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            var baseTriangleWallH = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 37.5f);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("Wall45");
            wallMesh.RotateY(FastMath.ToRad(45));
            wallMesh.Position = new TGCVector3(Position.X + 25, Position.Y, Position.Z + 50);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("Wall135");
            wallMesh.RotateY(FastMath.ToRad(90));
            wallMesh.Position = new TGCVector3(Position.X + 25, Position.Y, Position.Z + 50);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);
            #endregion

            #region Columnas
            //Columnas 
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(Position.X+5, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z+5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z );
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);
            //Este queda
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2V1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z );
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2H1");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column2H2");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y, Position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);
            #endregion

            //Tapas de columnas
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 0, 5), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(topTexture), 1, 1);

            wallMesh = baseWall.toMesh("TopColumn3");
            wallMesh.Position = new TGCVector3(Position.X + 45, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y + 20, Position.Z);
            wallMesh.UpdateMeshTransform();
            columnsTops.Add(wallMesh);
        }

    }
}
