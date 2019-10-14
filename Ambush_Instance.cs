using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class Ambush {
		private IDictionary<int, ISet<int>> EdgeTiles;
		
		////

		public int TileX { get; }
		public int TileY { get; }



		////////////////

		public Ambush( int tileX, int tileY, IDictionary<int, ISet<int>> edgeTiles ) {
			this.TileX = tileX;
			this.TileY = tileY;
			this.EdgeTiles = edgeTiles;
		}


		////////////////

		public void Trigger( Player player ) {

		}
	}
}
