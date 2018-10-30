using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace TGC.Group.Model.Parcelas
{
    class Column
    {
        public TGCVector3 Position { get; set; }

        public List<TgcMesh> CreateColumn(TgcTexture columnTexture, TGCVector3 offset)
        {
            List<TgcMesh> columna = new List<TgcMesh>();

            var baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(0, 20, 5), TgcPlane.Orientations.YZplane, columnTexture, 1, 1);

            var wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(Position.X + 5 + offset.X, Position.Y, Position.Z + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            wallMesh = baseWall.toMesh("Column1V2");
            wallMesh.Position = new TGCVector3(Position.X + offset.X , Position.Y, Position.Z + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            baseWall = new TgcPlane(new TGCVector3(), new TGCVector3(5, 20, 0), TgcPlane.Orientations.XYplane, columnTexture, 1, 1);

            wallMesh = baseWall.toMesh("Column1H1");
            wallMesh.Position = new TGCVector3(Position.X + offset.X, Position.Y, Position.Z + 5 + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            wallMesh = baseWall.toMesh("Column1H2");
            wallMesh.Position = new TGCVector3(Position.X + offset.X, Position.Y, Position.Z + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            return columna;
        }
    }
}
