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
		public static IDictionary<int, ISet<int>> CreateBrambleEnclosure( int tileX, int tileY, int radius ) {
			var tilePositionOffets = new Dictionary<int, ISet<int>>();
			var tilePositions = new Dictionary<int, ISet<int>>();

			// Trace outer rectangle
			for( int i = 0; i < radius; i++ ) {
				tilePositionOffets.Set2D( -i, -radius );
				tilePositionOffets.Set2D( -i, radius );
				tilePositionOffets.Set2D( i, -radius );
				tilePositionOffets.Set2D( i, radius );
			}
			for( int i = 0; i < radius; i++ ) {
				tilePositionOffets.Set2D( -radius, -i );
				tilePositionOffets.Set2D( -radius, i );
				tilePositionOffets.Set2D( radius, -i );
				tilePositionOffets.Set2D( radius, i );
			}

			// Compress rectangle
			foreach( (int offTileX, ISet<int> offTileYs) in tilePositionOffets ) {
				foreach( int offTileY in offTileYs ) {
					int distX = offTileX;
					int distY = offTileY;
					double dist = Math.Sqrt( (distX * distX) + (distY * distY) );

					double lerp = (dist - (double)radius ) / (double)radius;
					lerp *= lerp;

					float lerpX = MathHelper.Lerp( distX, (distX >= 0 ? radius : -radius), (float)lerp );
					float lerpY = MathHelper.Lerp( distY, (distY >= 0 ? radius : -radius), (float)lerp );
					lerpX *= lerpX;
					lerpY *= lerpY;

					tilePositions.Set2D( tileX + (int)lerpX, tileY + (int)lerpY );
				}
			}

			return tilePositions;
		}


		////////////////

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

			CursedBrambleTile.CreateBramblesAtAsync( tilePositionList, tilePositionList.Length - 1 );
		}

		////

		private static void CreateBramblesAtAsync( (int TileX, int TileY)[] randTilePositions, int lastIdx ) {
			(int tileX, int tileY) randEdgeTile = randTilePositions[ lastIdx ];

			CursedBrambleTile.CreateBramblePatchAt( randEdgeTile.tileX, randEdgeTile.tileY );

			if( lastIdx > 0 ) {
				lastIdx--;
				Timers.SetTimer(
					"AmbushesEntrapAsync_" + randTilePositions[lastIdx].TileX + "_" + randTilePositions[lastIdx].TileY,
					2,
					() => {
						CursedBrambleTile.CreateBramblesAtAsync( randTilePositions, lastIdx );
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