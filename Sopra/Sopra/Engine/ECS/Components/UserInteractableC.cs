using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sopra.Engine.ECS;

namespace Sopra.Engine.ECS.Components
{

    /// <summary>
    /// Marks an entity as user interactable.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    sealed class UserInteractableC : IComponent
    {
        public Entity InteractingChar { get; set; }
        public int Type { get; set; }

        public UserInteractableC(int type)
        {
            Type = type;
        }
    }
}
