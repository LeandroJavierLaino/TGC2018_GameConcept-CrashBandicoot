﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
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
        new protected Vector3 position;

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

        public Vector3 getPosition()
        {
            return position;
        }

        public bool isInParcela(Vector3 player)
        {
            return position.X < player.X &&
                position.Z < player.Z &&
                position.X + 50 > player.X &&
                position.Z + 50 > player.Z;
        }

        public bool isInPit(Vector3 player)
        {
            return position.X + 5 < player.X &&
                    position.Z + 5 < player.Z &&
                    position.X + 45 > player.X &&
                    position.Z + 45 > player.Z &&
                    player.Y < 0;
        }

        public void render()
        {
            floor.render();

            foreach(var plant in plants)
            {
                plant.render();
            }

            foreach (var column in columns)
            {
                column.render();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.render();
            }
        }

        public void dispose()
        {
            floor.dispose();

            foreach (var plant in plants)
            {
                plant.dispose();
            }

            foreach (var column in columns)
            {
                column.dispose();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.dispose();
            }
        }
    }
}
