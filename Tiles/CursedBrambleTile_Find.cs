using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public partial class CursedBrambleTile : ModTile {
		public static IList<(int tileX, int tileY)> FindBrambles( int leftTile, int topTile, int rightTile, int bottomTile ) {
			var tiles = new List<(int tileX, int tileY)>();

			for( int i=leftTile; i<rightTile; i++ ) {
				for( int j=topTile; j<bottomTile; j++ ) {
					Tile tile = Framing.GetTileSafely( i, j );
					if( !tile.active() || tile.type != ModContent.TileType<CursedBrambleTile>() ) {
						continue;
					}

					tiles.Add( (i, j) );
				}
			}

			return tiles;
		}
	}
}
