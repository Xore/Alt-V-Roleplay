﻿using AltV.Net.EntitySync;
using AltV.Net.EntitySync.SpatialPartitions;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Altv_Roleplay.EntityStreamer
{
    public class GlobalEntity : SpatialPartition
    {
        private readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        public GlobalEntity()
        {
        }

        public override void Add(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void Remove(IEntity entity)
        {
            entities.Remove(entity);
        }

        public override void UpdateEntityPosition(IEntity entity, in Vector3 newPosition)
        {
        }

        public override void UpdateEntityRange(IEntity entity, uint range)
        {
        }

        public override void UpdateEntityDimension(IEntity entity, int dimension)
        {
        }

        private static bool CanSeeOtherDimension(int dimension, int otherDimension)
        {
            if (dimension > 0) return dimension == otherDimension || otherDimension == int.MinValue;
            if (dimension < 0)
                return otherDimension == 0 || dimension == otherDimension || otherDimension == int.MinValue;
            return otherDimension == 0 || otherDimension == int.MinValue;
        }

        public override IList<IEntity> Find(Vector3 position, int dimension)
        {
            return entities.Where(entity => CanSeeOtherDimension(dimension, entity.Dimension)).ToList();
        }
    }
}
