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

			Ambush.CreateBrambleAt( randTile.tileX, randTile.tileY );

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


		public static void CreateBrambleAt( int tileX, int tileY ) {
			int thick = AmbushesMod.Config.BrambleThickness;
			float dense = AmbushesMod.Config.BrambleDensity;
			int brambleType = ModContent.TileType<CursedBrambleTile>();
			
			for( int i=thick/-2; i<thick/2; i++ ) {
				for( int j = thick / -2; j < thick / 2; j++ ) {
					if( TmlHelpers.SafelyGetRand().NextFloat() > dense ) {
						continue;
					}

					int x = tileX + i;
					int y = tileX + j;

					if( TileHelpers.IsAir( Framing.GetTileSafely( x, y ) ) ) {
						TileHelpers.PlaceTile( x, y, brambleType );
					}
				}
			}
		}
	}
}
