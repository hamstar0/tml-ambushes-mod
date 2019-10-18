using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class Ambush {
		public static bool CheckForAmbushElegibility( int tileX, int tileY ) {
			IDictionary<int, ISet<int>> edgeTiles = new Dictionary<int, ISet<int>> {
				{ tileX, new HashSet<int> { tileY } }
			};

			var edgeTileQueue = new Dictionary<int, ISet<int>>();
			var chartedTiles = new Dictionary<int, ISet<int>>();

			TilePattern pattern = new TilePattern( new TilePatternBuilder {
				IsSolid = false,
				IsActuated = false
			} );
			int maxDistSqr = 50 * 50;
			int minNeededAirTiles = 32 * 32;
			int airTileCount = 0;

			do {
				foreach( (int eTileX, ISet<int> eTileYs) in edgeTiles ) {
					foreach( int eTileY in eTileYs ) {
						int distX = tileX - eTileX;
						int distY = tileY - eTileY;

						if( ((distX * distX) + (distY * distY)) > maxDistSqr ) {
							continue;
						}

						var eTile = Framing.GetTileSafely( eTileX, eTileY );

						if( pattern.Check(eTileX, eTileY) ) {
							if( !chartedTiles.ContainsKey(eTileX) || !chartedTiles[eTileX].Contains(eTileY) ) {
								chartedTiles.Set2D( eTileX, eTileY );
								edgeTileQueue.Set2D( eTileX, eTileY );
								airTileCount++;
							}
						}
						
						if( airTileCount >= minNeededAirTiles ) {
							break;
						}
					}
				}

				edgeTiles.Clear();

				foreach( (int eTileX, ISet<int> eTileYs) in edgeTileQueue ) {
					foreach( int eTileY in eTileYs ) {
						Ambush.GetUnchartedNeighboringEdgeTiles( eTileX, eTileY, chartedTiles, ref edgeTiles );
					}
				}

				edgeTileQueue.Clear();
			} while( edgeTiles.Count > 0 && airTileCount < minNeededAirTiles );
			
			return airTileCount >= minNeededAirTiles;
		}

		////

		private static void GetUnchartedNeighboringEdgeTiles(
					int eTileX, int eTileY,
					IDictionary<int, ISet<int>> chartedTiles,
					ref IDictionary<int, ISet<int>> newEdgeTiles ) {
			IEnumerable<(int, int)> neighbors() {
				yield return ( eTileX, eTileY - 1 );
				yield return ( eTileX - 1, eTileY );
				yield return ( eTileX + 1, eTileY );
				yield return ( eTileX, eTileY + 1 );
			};

			foreach( (int tileX, int tileY) in neighbors() ) {
				if( chartedTiles.ContainsKey( tileX ) && chartedTiles[tileX].Contains( tileY ) ) {
					continue;
				}

				Tile tile = Framing.GetTileSafely( tileX, tileY );

				if( TileHelpers.IsAir(tile, true, false) ) {
					newEdgeTiles.Set2D( tileX, tileY );
				}
			}
		}

		////////////////

		public static void AdjustAmbushTileCenter( int tileX, ref int tileY ) {
			Tile tile;
			int y = 0;

			do {
				tile = Framing.GetTileSafely( tileX, tileY + y );
				y++;
			} while( TileHelpers.IsAir(tile, true, false) );

			tileY = Math.Max( tileY, (tileY + y) - (AmbushesMod.Config.AmbushTriggerRadiusTiles / 2) );
		}
	}
}
