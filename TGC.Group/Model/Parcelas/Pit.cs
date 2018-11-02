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
    public class Pit : Parcela
    {

        public Pit(TGCVector3 newPosition, TgcPlane grassPlane, TgcPlane wallPlaneX, TgcPlane wallPlaneZ, TgcPlane wallPlaneVertX, TgcPlane columnPlaneX, TgcPlane columnPlaneZ, TgcPlane topPlane)
        {
            this.Position = newPosition;

            //Se define el terrno de la parcela
            var grassMesh = grassPlane.toMesh("floor");
            grassMesh.Position = new TGCVector3(this.Position.X, this.Position.Y - 20, this.Position.Z);
            grassMesh.Transform = TGCMatrix.Translation(grassMesh.Position);
            meshes.Add(grassMesh);

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseTriangleWallH = wallPlaneZ;

            var wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(newPosition.X, newPosition.Y - 20, newPosition.Z + 5);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallHB");
            wallMesh.RotateX(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(newPosition.X, newPosition.Y - 20, newPosition.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            var baseWall = wallPlaneX;

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(newPosition.X + 5, newPosition.Y - 20, newPosition.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(-FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(newPosition.X + 45, newPosition.Y - 20, newPosition.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            baseWall = wallPlaneVertX;

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(newPosition.X, newPosition.Y, newPosition.Z);
            wallMesh.Transform = TGCMatrix.Translation(wallMesh.Position);
            meshes.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(newPosition.X + 50, newPosition.Y, newPosition.Z);
            wallMesh.Transform = TGCMatrix.Translation(wallMesh.Position);
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
            wallMesh.Position = new TGCVector3(newPosition.X, newPosition.Y + 20, newPosition.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(newPosition.X + 45, newPosition.Y + 20, newPosition.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(newPosition.X + 45, newPosition.Y + 20, newPosition.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(newPosition.X, newPosition.Y + 20, newPosition.Z + 45);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);
        }

       
    }
}
