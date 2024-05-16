﻿using System.Collections.Generic;
using Sopra.ECS;

namespace Sopra.Logic.Render
{
    /// <summary>
    /// Implements an comparer, which can be used to sort entitys with
    /// a SpriteContainer Component from back to front
    /// </summary>
    /// <author>Felix Vogt</author>
    /// <inheritdoc cref="IComparer{T}"/>
    internal sealed class SortBackToFront : IComparer<Entity>
    {
        public int Compare(Entity e1, Entity e2)
        {
            if (e1 == null || e2 == null)
            {
                return 0;
            }

            var layer1 = e1.HasComponent(AnimationC.Type)
                ? e1.GetComponent<AnimationC>(AnimationC.Type).Layer
                : e1.GetComponent<SimpleSpriteC>(SimpleSpriteC.Type).Layer;

            var layer2 = e2.HasComponent(AnimationC.Type)
                ? e2.GetComponent<AnimationC>(AnimationC.Type).Layer
                : e2.GetComponent<SimpleSpriteC>(SimpleSpriteC.Type).Layer;

            if (layer1 < layer2)
            {
                return 1;
            }

            if (layer1 > layer2)
            {
                return -1;
            }

            return 0;
        }
    }
}
