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

        public List<TgcMesh> CreateColumn(TgcPlane columnPlaneX, TgcPlane columnPlaneZ, TGCVector3 offset)
        {
            List<TgcMesh> columna = new List<TgcMesh>();

            var baseWall = columnPlaneX;

            var wallMesh = baseWall.toMesh("Column1V1");
            wallMesh.Position = new TGCVector3(Position.X + 5 + offset.X, Position.Y, Position.Z + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            wallMesh = baseWall.toMesh("Column1V2");
            wallMesh.Position = new TGCVector3(Position.X + offset.X , Position.Y, Position.Z + offset.Z);
            wallMesh.UpdateMeshTransform();
            columna.Add(wallMesh);

            baseWall = columnPlaneZ;

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
