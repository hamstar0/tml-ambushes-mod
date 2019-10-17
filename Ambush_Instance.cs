using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class Ambush {
		public int TileX { get; }
		public int TileY { get; }

		public bool IsEntrapping { get; private set; }



		////////////////

		public Ambush( int tileX, int tileY, bool isEntrapping ) {
			this.TileX = tileX;
			this.TileY = tileY;
			this.IsEntrapping = isEntrapping;
		}


		////////////////

		public void Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return;
			}
			int tileX = point.Value.x;
			int tileY = point.Value.y;

			if( this.IsEntrapping ) {
				IDictionary<int, ISet<int>> edgeTiles = Ambush.CreateEntrapmentEdges( tileX, tileY );
				Ambush.CreateEntrapmentUponEdges( edgeTiles );
			}

			//TODO
		}
	}
}
