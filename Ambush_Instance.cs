using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class Ambush {
		private IDictionary<int, ISet<int>> EdgeTiles;
		
		////

		public int TileX { get; }
		public int TileY { get; }

		public bool IsEntrapping { get; private set; }



		////////////////

		public Ambush( int tileX, int tileY, IDictionary<int, ISet<int>> edgeTiles, bool isEntrapping ) {
			this.TileX = tileX;
			this.TileY = tileY;
			this.EdgeTiles = edgeTiles;
			this.IsEntrapping = isEntrapping;
		}


		////////////////

		public void Trigger( Player player ) {
			if( this.IsEntrapping ) {
				(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );

				if( point.HasValue ) {
					IDictionary<int, ISet<int>> edgeTiles;
					if( Ambush.CheckForAmbushElegibility( point.Value.x, point.Value.y, out edgeTiles ) ) {
						Ambush.CreateEntrapment( this.EdgeTiles );
					}
				}
			}
		}
	}
}
