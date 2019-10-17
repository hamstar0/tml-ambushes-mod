using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public partial class CursedBrambleTile : ModTile {
		public static IDictionary<int, ISet<int>> CreateBrambleEnclosure( int tileX, int tileY ) {
			int max = 32;
			var outerEdgeTiles = new Dictionary<int, ISet<int>>();
			var edgeTiles = new Dictionary<int, ISet<int>>();

			// Trace outer rectangle
			for( int i = 0; i < max; i++ ) {
				outerEdgeTiles.Set2D( tileX - i, tileY - max );
				outerEdgeTiles.Set2D( tileX - i, tileY + max );
				outerEdgeTiles.Set2D( tileX + i, tileY - max );
				outerEdgeTiles.Set2D( tileX + i, tileY + max );
			}
			for( int i = 0; i < max; i++ ) {
				outerEdgeTiles.Set2D( tileX - max, tileY - i );
				outerEdgeTiles.Set2D( tileX + max, tileY - i );
				outerEdgeTiles.Set2D( tileX - max, tileY + i );
				outerEdgeTiles.Set2D( tileX + max, tileY + i );
			}

			// Compress rectangle
			foreach( (int eTileX, ISet<int> eTileYs) in outerEdgeTiles ) {
				foreach( int eTileY in eTileYs ) {
					int distX = eTileX - tileX;
					int distY = eTileY - tileY;
					double dist = Math.Sqrt( ( distX * distX ) + ( distY * distY ) );

					double lerp = ( dist - (double)max ) / (double)max;
					lerp *= lerp;

					distX = (int)MathHelper.Lerp( distX, ( distX >= 0 ? max : -max ), (float)lerp );
					distY = (int)MathHelper.Lerp( distY, ( distY >= 0 ? max : -max ), (float)lerp );


					float lerpX = Math.Abs( (float)max / (float)distX );
					lerpX *= lerpX;
					int newX = (int)MathHelper.Lerp( (float)distX, ( distX >= 0 ? max : -max ), lerpX );

					float lerpY = (float)distY / max;
					lerpX *= lerpX;
					int newY = (int)MathHelper.Lerp( (float)distX, ( distX >= 0 ? max : -max ), lerpX );

					edgeTiles.Set2D( newX, newY );
				}
			}

			return edgeTiles;
		}


		////////////////

		public static void CreateBramblesAt( IDictionary<int, ISet<int>> edgeTiles ) {
			int totalCount = edgeTiles.Count2D();
			var edgeCloneList = new (int TileX, int TileY)[totalCount];

			int i = 0;
			foreach( (int tileX, ISet<int> tileYs) in edgeTiles ) {
				foreach( int tileY in tileYs ) {
					edgeCloneList[i++] = (tileX, tileY);
				}
			}

			for( i = totalCount - 1; i > 0; i-- ) {
				int rand = TmlHelpers.SafelyGetRand().Next( i );
				(int, int) tmp = edgeCloneList[i];

				edgeCloneList[i] = edgeCloneList[rand];
				edgeCloneList[rand] = tmp;
			}

			CursedBrambleTile.CreateBramblesAtAsync( edgeCloneList, edgeCloneList.Length - 1 );
		}

		////

		private static void CreateBramblesAtAsync( (int TileX, int TileY)[] edgeRandTiles, int lastIdx ) {
			(int tileX, int tileY) randEdgeTile = edgeRandTiles[lastIdx];

			CursedBrambleTile.CreateBramblePatchAt( randEdgeTile.tileX, randEdgeTile.tileY );

			if( lastIdx > 0 ) {
				lastIdx--;
				Timers.SetTimer(
					"AmbushesEntrapAsync_" + edgeRandTiles[lastIdx].TileX + "_" + edgeRandTiles[lastIdx].TileY,
					2,
					() => {
						CursedBrambleTile.CreateBramblesAtAsync( edgeRandTiles, lastIdx );
						return false;
					}
				);
			}
		}


		////

		public static bool CreateBramblePatchAt( int tileX, int tileY ) {
			int brambleTileType = ModContent.TileType<CursedBrambleTile>();
			int thick = AmbushesMod.Config.BrambleThickness;
			float dense = AmbushesMod.Config.BrambleDensity;
			var rand = TmlHelpers.SafelyGetRand();

			Tile tileAt = Main.tile[tileX, tileY];
			if( tileAt != null && tileAt.active() && tileAt.type == brambleTileType ) {
				return false;
			}

			bool bramblePlaced = false;

			int max = thick / 2;
			int min = -max;
			for( int i = min; i < max; i++ ) {
				for( int j = min; j < max; j++ ) {
					if( rand.NextFloat() > dense ) {
						continue;
					}

					int x = tileX + i;
					int y = tileY + j;

					if( TileHelpers.IsAir( Framing.GetTileSafely( x, y ), false, true ) ) {
						TileHelpers.PlaceTile( x, y, brambleTileType );
						bramblePlaced = true;
					}
				}
			}

			return bramblePlaced;
		}
	}
}