using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Services.Timers;
using Terraria.ModLoader;
using Ambushes.Tiles;


namespace Ambushes {
	partial class Ambush {
		public static void CreateEntrapment( IDictionary<int, ISet<int>> edgeTiles ) {
			int totalCount = edgeTiles.Count2D();
			var edgeCloneList = new (int TileX, int TileY)[ totalCount ];

			int i = 0;
			foreach( (int tileX, ISet<int> tileYs) in edgeTiles ) {
				foreach( int tileY in tileYs ) {
					int rand = TmlHelpers.SafelyGetRand().Next( totalCount );
					(int, int) tmp = (tileX, tileY);

					edgeCloneList[ i ] = edgeCloneList[ rand ];
					edgeCloneList[ rand ] = tmp;
					i++;
				}
			}

			Ambush.CreateEntrapmentAsync( edgeCloneList, edgeCloneList.Length - 1 );
		}


		////

		private static void CreateEntrapmentAsync( (int TileX, int TileY)[] edgeRandTiles, int lastIdx ) {
			(int tileX, int tileY) randTile = edgeRandTiles[ lastIdx ];

			if( TileHelpers.IsAir( Framing.GetTileSafely(randTile.tileX, randTile.tileY) ) ) {
				int brambleType = ModContent.TileType<CursedBrambleTile>();

				TileHelpers.PlaceTile( randTile.tileX, randTile.tileY, brambleType );
			}

			if( lastIdx > 0 ) {
				lastIdx--;
				Timers.SetTimer(
					"AmbushesEntrapAsync_" + edgeRandTiles[lastIdx].TileX + "_" + edgeRandTiles[lastIdx].TileY,
					2,
					() => {
						Ambush.CreateEntrapmentAsync( edgeRandTiles, lastIdx );
						return false;
					}
				);
			}
		}
	}
}
