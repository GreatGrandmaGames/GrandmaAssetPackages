using System;
using UnityEngine;

namespace Grandma.Tiles
{
    public class Tile : GrandmaComponent
    {
        [NonSerialized]
        private TileData tileData;

        public GrandmaCollection TileConnections { get; private set; }

        public override GrandmaComponentData Data {

            get => base.Data;

            protected set {
                base.Data = value;

                tileData = Data as TileData;
            }
        }

        protected override void OnCreated()
        {
            base.OnCreated();

            if(tileData != null)
            {
                var newTileConnection = GrandmaObjectManager.Instance.CreateNewComponent<GrandmaCollection>(gameObject);

                tileData.tileConnectionID = newTileConnection.ComponentID;
            }
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            if(TileConnections?.ComponentID != ObjectID)
            {
                TileConnections = GrandmaObjectManager.Instance.GetComponentByID<GrandmaCollection>(ObjectID, tileData.tileConnectionID);
            }
        }
    }
}
