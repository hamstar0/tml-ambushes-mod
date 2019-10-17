using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.TModLoader;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public partial class CursedBrambleTile : ModTile {
		public static bool ErodeRandomBrambleWithinRadius( int tileX, int tileY, int radius ) {
			int randX = TmlHelpers.SafelyGetRand().Next( radius * 2 );
			int randY = TmlHelpers.SafelyGetRand().Next( radius * 2 );
			int randTileX = tileX + ( randX - radius );
			int randTileY = tileY + ( randY - radius );

			return CursedBrambleTile.RemoveBrambleAt( randTileX, randTileY );
		}


		public static bool RemoveBrambleAt( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );

			if( tile.active() && tile.type == ModContent.TileType<CursedBrambleTile>() ) {
				TileHelpers.KillTile( tileX, tileY, false, false );
				return true;
			}
			return false;
		}
	}
}
