// Copyright (c) 2012-2023 Wojciech Figat. All rights reserved.

#if USE_LARGE_WORLDS
using Real = System.Double;
#else
using Real = System.Single;
#endif

using FlaxEngine;

#if FLAX_EDITOR
using FlaxEditor.CustomEditors.Dedicated;
using FlaxEditor.CustomEditors;
#endif

namespace FlaxEditor.SceneGraph.Actors
{
#if FLAX_EDITOR
    /// <summary>
    /// Dedicated custom editor for BoxCollider objects.
    /// </summary>
    [CustomEditor(typeof(BoxCollider)), DefaultEditor]
    public class BoxColliderEditor : ActorEditor
    {
        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            base.Initialize(layout);
            layout.Space(20f);
            var autoResizeButton = layout.Button("Resize to Fit", "Resize the box collider to fit it's parent's bounds.");

            BoxCollider collider = Values[0] as BoxCollider;
            autoResizeButton.Button.Clicked += collider.AutoResize;
        }
    }
#endif

    /// <summary>
    /// Scene tree node for <see cref="BoxCollider"/> actor type.
    /// </summary>
    /// <seealso cref="ColliderNode" />
    [HideInEditor]
    public sealed class BoxColliderNode : ColliderNode
    {
        /// <inheritdoc />
        public BoxColliderNode(Actor actor)
        : base(actor)
        {
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref RayCastData ray, out Real distance, out Vector3 normal)
        {
            // Pick wires
            var actor = (BoxCollider)_actor;
            var box = actor.OrientedBox;
            if (Utilities.Utils.RayCastWire(ref box, ref ray.Ray, out distance, ref ray.View.Position))
            {
                normal = Vector3.Up;
                return true;
            }

            return base.RayCastSelf(ref ray, out distance, out normal);
        }

        /// <inheritdoc />
        public override void PostSpawn()
        {
            base.PostSpawn();
            BoxCollider boxCollider = Actor as BoxCollider;
            boxCollider.AutoResize();
        }
    }
}
