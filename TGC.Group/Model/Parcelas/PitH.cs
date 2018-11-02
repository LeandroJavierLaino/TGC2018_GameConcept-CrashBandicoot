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
        public PitH(TGCVector3 Position, TgcPlane grassPlane, TgcPlane wallPlaneX, TgcPlane wallPlaneZ, TgcPlane wallPlaneVertZ, TgcPlane columnPlaneX, TgcPlane columnPlaneZ, TgcPlane topPlane)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            var grassMesh = grassPlane.toMesh("floor");
            grassMesh.Position = new TGCVector3(this.Position.X, this.Position.Y - 20, this.Position.Z);
            grassMesh.Transform = TGCMatrix.Translation(grassMesh.Position);
            meshes.Add(grassMesh);

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseTriangleWallH = wallPlaneZ; 

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

            var baseWall = wallPlaneX;

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
                        
            baseWall = wallPlaneVertZ;
       
            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z+50);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            //Columnas 
            var column = new Column
            {
                Position = new TGCVector3(this.Position.X, this.Position.Y - 20, this.Position.Z)
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
