using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpatialHashTable.CullingSystem
{
    public interface ICullSystemManager
    {
        void SetCenter(Vector3 position);
    }
    public sealed class CullSystemManager : SpatialHashManagerBase<CullableObjectTag>,ICullSystemManager
    {
        public float cullDistance;

        private SpatialIndex catchCullCenterIndex;
        readonly List<SpatialIndex> previousCullIndices = new List<SpatialIndex>();
        readonly List<SpatialIndex> currentCullIndices = new List<SpatialIndex>();
        readonly List<SpatialIndex> subtractedIndices = new List<SpatialIndex>();

        public void SetCenter(Vector3 position)
        {
            var centerIndex = Vector3ToSpatialIndex(position);
            if (centerIndex == catchCullCenterIndex) return;
            catchCullCenterIndex = centerIndex;
            GetAllIndexesNearbyNoneAloc(currentCullIndices, cullDistance, position);
            subtractedIndices.Clear();

            subtractedIndices.AddRange(previousCullIndices.Except(currentCullIndices));
            foreach (var index in subtractedIndices)
            {
                if (!objectsTable.ContainsKey(index))
                {
                    continue; //previous cache is really out of date
                }


                foreach (var cullableObjectTag in objectsTable[index])
                {
                    cullableObjectTag.CullChildrenTags();
                }
            }

            foreach (var index in currentCullIndices)
            {
                foreach (var cullObjetTag in objectsTable[index])
                {
                    cullObjetTag.UnCullChildrenTags();
                }
            }

            previousCullIndices.Clear();
            previousCullIndices.AddRange(currentCullIndices);
        }

        public override void AddObject(CullableObjectTag Obj)
        {
            base.AddObject(Obj);

            if (previousCullIndices.Contains(Obj.SpatialIndex))
            {
                Obj.UnCullChildrenTags();
            }
            else if (IsIndextInRange(catchCullCenterIndex, Obj.SpatialIndex, cullDistance))
            {
                Obj.UnCullChildrenTags();
                previousCullIndices.Add(Obj.SpatialIndex);
            }
            else
            {
                Obj.CullChildrenTags();
            }
        }

        public override void RemoveObject(CullableObjectTag Obj)
        {
            base.RemoveObject(Obj);
            Obj.UnCullChildrenTags();
        }


    }
}