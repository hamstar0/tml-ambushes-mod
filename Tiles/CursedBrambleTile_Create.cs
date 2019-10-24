using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public partial class CursedBrambleTile : ModTile {
		public static void CreateBramblesAt( IDictionary<int, ISet<int>> tilePositions ) {
			int totalCount = tilePositions.Count2D();
			var tilePositionList = new (int TileX, int TileY)[totalCount];

			int i = 0;
			foreach( (int tileX, ISet<int> tileYs) in tilePositions ) {
				foreach( int tileY in tileYs ) {
					tilePositionList[i++] = (tileX, tileY);
				}
			}

			for( i = totalCount - 1; i > 0; i-- ) {
				int rand = TmlHelpers.SafelyGetRand().Next( i );
				(int, int) tmp = tilePositionList[i];

				tilePositionList[i] = tilePositionList[rand];
				tilePositionList[rand] = tmp;
			}

			CursedBrambleTile.CreateBramblesAtIntervals( tilePositionList, tilePositionList.Length - 1 );
		}

		////

		private static void CreateBramblesAtIntervals( (int TileX, int TileY)[] randTilePositions, int lastIdx ) {
			(int tileX, int tileY) tilePos = randTilePositions[ lastIdx ];

			int bramblesPlaced = CursedBrambleTile.CreateBramblePatchAt( tilePos.tileX, tilePos.tileY );

			if( AmbushesMod.Config.DebugModeInfoBrambles ) {
				LogHelpers.Log( "Created " + bramblesPlaced + " brambles in patch " + lastIdx + " of " + randTilePositions.Length );
			}

			if( lastIdx > 0 ) {
				lastIdx--;

				string timerName = "AmbushesEntrapAsync_" + randTilePositions[lastIdx].TileX + "_" + randTilePositions[lastIdx].TileY;
				Timers.SetTimer( timerName, 2, () => {
					CursedBrambleTile.CreateBramblesAtIntervals( randTilePositions, lastIdx );
					return false;
				} );
			}
		}


		////

		public static int CreateBramblePatchAt( int tileX, int tileY ) {
			int brambleTileType = ModContent.TileType<CursedBrambleTile>();
			int thick = AmbushesMod.Config.BrambleThickness;
			float dense = AmbushesMod.Config.BrambleDensity;
			var rand = TmlHelpers.SafelyGetRand();

			Tile tileAt = Main.tile[tileX, tileY];
			if( tileAt != null && tileAt.active() && tileAt.type == brambleTileType ) {
				return 0;
			}

			int bramblesPlaced = 0;

			int max = thick / 2;
			int min = -max;
			for( int i = min; i < max; i++ ) {
				for( int j = min; j < max; j++ ) {
					if( (1f - rand.NextFloat()) > dense ) {
						continue;
					}

					int x = tileX + i;
					int y = tileY + j;
					Tile tile = Framing.GetTileSafely( x, y );

					if( tile == null || !tile.active() ) {
						if( TileHelpers.PlaceTile(x, y, brambleTileType) ) {
							Tile newTile = Main.tile[x, y];
							newTile.wire( tile.wire() );
							newTile.wire2( tile.wire2() );
							newTile.wire3( tile.wire3() );
							newTile.wire4( tile.wire4() );
							newTile.liquid = tile.liquid;
							newTile.liquidType( tile.liquidType() );
							NetMessage.SendData( MessageID.TileChange, -1, -1, null, 1, (float)x, (float)y, (float)brambleTileType, 0, 0, 0 );

							bramblesPlaced++;
						}
					}
				}
			}

			return bramblesPlaced;
		}
	}
}