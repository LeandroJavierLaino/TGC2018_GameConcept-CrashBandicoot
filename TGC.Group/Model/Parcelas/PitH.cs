using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model.Parcelas
{
    class PitH : Parcela
    {
        public PitH(Vector3 position, string grassTexture, string wallTexture, string columnTexture, string topTexture)
        {
            //Se define el terrno de la parcela
            floor = new TgcPlane(new Vector3(position.X, position.Y - 20, position.Z), new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4).toMesh("floor");

            //Armamos muros a partir de un Plano y lo convertimos a Mesh
            var baseTriangleWallH = new TgcPlane(new Vector3(), new Vector3(50, 20.62f, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            var wallMesh = baseTriangleWallH.toMesh("WallHA");
            wallMesh.rotateX(-FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z + 5);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallHB");
            wallMesh.rotateX(FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            var baseWall = new TgcPlane(new Vector3(), new Vector3(0, 20.62f, 50), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.rotateZ(FastMath.ToRad(2 * 7.125f));
            wallMesh.Position = new Vector3(position.X + 5, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            wallMesh = wallMesh.clone("WallVB");

            wallMesh.rotateZ(-FastMath.ToRad(4 * 7.125f));
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(50, 20, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(wallTexture), 2, 1);

            wallMesh = baseWall.toMesh("WallVA");
            wallMesh.Position = new Vector3(position.X, position.Y, position.Z+50);
            wallMesh.UpdateMeshTransform();

            walls.Add(wallMesh);
            
            //Columnas 
            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2V1");
            wallMesh.Position = new Vector3(position.X + 5, position.Y - 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2H1");
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column2H2");
            wallMesh.Position = new Vector3(position.X, position.Y - 20, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z + 5);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column3H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(0, 40, 5), TgcPlane.Orientations.YZplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4V1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4H1");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z + 45);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

            baseWall = new TgcPlane(new Vector3(), new Vector3(5, 40, 0), TgcPlane.Orientations.XYplane, TgcTexture.createTexture(columnTexture), 1, 2);

            wallMesh = baseWall.toMesh("Column4H2");
            wallMesh.Position = new Vector3(position.X + 45, position.Y - 20, position.Z + 50);
            wallMesh.UpdateMeshTransform();
            columns.Add(wallMesh);

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
