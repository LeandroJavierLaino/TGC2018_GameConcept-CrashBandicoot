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

namespace TGC.Group.Model.Enviroment_Objects
{
    class Temple
    {
        //Paredes del Templo, tambien pueden ser columnas
        private List<TgcMesh> templeWalls = new List<TgcMesh>();

        public Temple(Vector3 position, string wallTexture, string columnTexture, string topWall, string topTexture, string floorTexture)
        {

            #region 1er Piso
            //1er Piso
            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 100), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X , position.Y, position.Z);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 100, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(100, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z + 100);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(90, 0, 90), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(floorTexture), 2, 2);

            wallMesh = baseWall.toMesh("Floor");
            wallMesh.Position = new TGCVector3(position.X + 5,position.Y + 20,position.Z + 5);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);
            #endregion

            #region 2do Piso
            //2Piso
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 80), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 90, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(80, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 90);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(70, 0, 70), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(floorTexture), 2, 2);

            wallMesh = baseWall.toMesh("Floor");
            wallMesh.Position = new TGCVector3(position.X + 15, position.Y + 40, position.Z + 15);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);
            #endregion

            #region 3er Piso
            //3Piso
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20.62f, 60), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.RotateZ(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.RotateZ(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 80, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(60, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("WallHA");
            wallMesh.RotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 80);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(floorTexture), 2, 2);

            wallMesh = baseWall.toMesh("Floor");
            wallMesh.Position = new TGCVector3(position.X + 25, position.Y + 60, position.Z + 25);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);
#endregion

            #region Tope
            //Tope
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 45);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 65);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 45);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(10, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 65);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 45, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 45, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 65, position.Y + 60, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 65, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 10), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 55, position.Y + 60, position.Z + 55);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(30, 0, 30), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("Floor");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 80, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(30, 5, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(topWall), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 80, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(30, 5, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(topWall), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 80, position.Z + 65);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 5, 30), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(topWall), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 80, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 5, 30), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(topWall), 2, 1);

            wallMesh = baseWall.toMesh("TopWall");
            wallMesh.Position = new TGCVector3(position.X + 65, position.Y + 80, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(30, 0, 30), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("Floor");
            wallMesh.Position = new TGCVector3(position.X + 35, position.Y + 85, position.Z + 35);
            wallMesh.UpdateMeshTransform();

            templeWalls.Add(wallMesh);
            #endregion

            #region Columnas
            //Columnas
            //1er Piso
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 5, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 5, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X, position.Y, position.Z + 100);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y, position.Z + 100);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            //2do Piso
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 15, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100 - 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 15, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100 - 10, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 5 + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 20, position.Z + 100 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 5 + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 20, position.Z + 100 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            //3er Piso
            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 5 + 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100 - 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 5 + 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 100 - 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 1);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20 , position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 5 + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 40, position.Z + 100 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 5 + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("Column1V1");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 40, position.Z + 100 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            //Tapas

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 0, 5), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(topTexture), 1, 1);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new TGCVector3(position.X, position.Y + 20, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y + 20, position.Z);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(position.X + 95, position.Y + 20, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(position.X, position.Y + 20, position.Z + 95);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 40, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 40, position.Z + 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(position.X + 95 - 10, position.Y + 40, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(position.X + 10, position.Y + 40, position.Z + 95 - 10);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = baseWall.toMesh("TopColumn1");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 60, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn2");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 60, position.Z + 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn3");
            wallMesh.Position = new TGCVector3(position.X + 95 - 20, position.Y + 60, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);

            wallMesh = wallMesh.clone("TopColumn4");
            wallMesh.Position = new TGCVector3(position.X + 20, position.Y + 60, position.Z + 95 - 20);
            wallMesh.UpdateMeshTransform();
            templeWalls.Add(wallMesh);
            #endregion
        }

        public void render()
        {
            foreach(var wall in templeWalls)
            {
                wall.Render();
            }
        }

        public void dispose()
        {
            foreach (var wall in templeWalls)
            {
                wall.Dispose();
            }
        }

        public List<TgcMesh> getWalls()
        {
            return templeWalls;
        }
    }
}
