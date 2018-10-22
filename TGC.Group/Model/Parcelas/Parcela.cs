using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Parcela
    {
        //Piso por el momento es simplemente un plano
        protected TgcMesh floor;
        
        //Algo de vegetacion
        protected List<TgcMesh> plants = new List<TgcMesh>();

        //Paredes
        protected List<TgcMesh> walls = new List<TgcMesh>();

        //Columnas
        protected List<TgcMesh> columns = new List<TgcMesh>();

        //El tope de las columnas
        protected List<TgcMesh> columnsTops = new List<TgcMesh>();

        //Posicion a partir de la cual se ubica todo
        new protected TGCVector3 Position;

        public List<TgcMesh> getWalls()
        {
            return walls;
        }

        public List<TgcMesh> getColumns()
        {
            return columns;
        }

        public List<TgcMesh> getAllMeshes()
        {
            List<TgcMesh> allMeshesList = new List<TgcMesh>();

            allMeshesList.Add(floor);

            foreach (var plant in plants)
            {
                allMeshesList.Add(plant);
            }

            foreach (var wall in walls)
            {
                allMeshesList.Add(wall);
            }

            foreach (var column in columns)
            {
                allMeshesList.Add(column);
            }

            foreach (var column in columnsTops)
            {
                allMeshesList.Add(column);
            }

            return allMeshesList;
        }

        public TGCVector3 getPosition()
        {
            return Position;
        }

        public bool isInParcela(TGCVector3 player)
        {
            return Position.X < player.X &&
                Position.Z < player.Z &&
                Position.X + 50 > player.X &&
                Position.Z + 50 > player.Z;
        }

        public bool isInPit(TGCVector3 player)
        {
            return Position.X + 5 < player.X &&
                    Position.Z + 5 < player.Z &&
                    Position.X + 45 > player.X &&
                    Position.Z + 45 > player.Z &&
                    player.Y < 0;
        }

        public void Render()
        {
            floor.Render();

            foreach(var plant in plants)
            {
                plant.Render();
            }

            foreach (var column in columns)
            {
                column.Render();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.Render();
            }
        }

        public void Dispose()
        {
            floor.Dispose();

            foreach (var plant in plants)
            {
                plant.Dispose();
            }

            foreach (var column in columns)
            {
                column.Dispose();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.Dispose();
            }
        }
    }
}
