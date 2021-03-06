﻿using Microsoft.DirectX;
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
    class Inicio : Parcela
    {
        public Inicio(TGCVector3 Position, TgcPlane grassPlane, TgcTexture wallTexture, TgcPlane columnPlaneX, TgcPlane columnPlaneZ, TgcPlane topPlane)
        {
            this.Position = Position;

            //Se define el terrno de la parcela
            var grassMesh = grassPlane.toMesh("floor");
            grassMesh.Position = this.Position;
            grassMesh.Transform = TGCMatrix.Translation(grassMesh.Position);
            meshes.Add(grassMesh);

            #region Paredes
            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20f, 50), TgcPlane.Orientations.YZplane, wallTexture, 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");
            wallMesh.Position = new TGCVector3(Position.X + 50, Position.Y, Position.Z);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            var baseTriangleWallH = new TgcPlane(new TGCVector3(), new TGCVector3(50, 20f, 0), TgcPlane.Orientations.XYplane, wallTexture, 2, 1);

            wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 12.5f);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("Wall45");
            wallMesh.RotateY(FastMath.ToRad(45));
            wallMesh.Position = new TGCVector3(Position.X, Position.Y, Position.Z + 25);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);

            wallMesh = wallMesh.clone("Wall135");
            wallMesh.RotateY(FastMath.ToRad(90));
            wallMesh.Position = new TGCVector3(Position.X + 50, Position.Y, Position.Z + 25);
            wallMesh.UpdateMeshTransform();
            meshes.Add(wallMesh);
            #endregion

            #region Columnas
            //Columnas 
            var column = new Column
            {
                Position = this.Position
            };

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, TGCVector3.Empty));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(0, 0, 45)));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(45, 0, 0)));

            meshes.AddRange(column.CreateColumn(columnPlaneX, columnPlaneZ, new TGCVector3(45, 0, 45)));
            #endregion

            //Tapas de columnas
            baseWall = topPlane;

            wallMesh = baseWall.toMesh("TopColumn3");
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
