using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	public class Ambush {
		public static bool CheckForAmbushElegibility( int tileX, int tileY, out IList<(int TileX, int TileY)> edgeTiles ) {
			edgeTiles = new List<(int TileX, int TileY)> { (tileX, tileY) };

			var edgeTileQueue = new List<(int TileX, int TileY)>();
			var chartedTiles = new Dictionary<int, ISet<int>>();

			int maxDistSqr = 60 * 60;
			int maxCount = 1600;
			int count = 0;

			do {
				for( int i = 0; i < edgeTiles.Count; i++ ) {
					int eTileX = edgeTiles[i].TileX;
					int eTileY = edgeTiles[i].TileY;

					if( ( eTileX * eTileX ) + ( eTileY * eTileY ) > maxDistSqr ) {
						continue;
					}

					Tile tile = Framing.GetTileSafely( eTileX, eTileY );

					if( TileHelpers.IsAir( tile, true, false ) ) {
						if( !chartedTiles.ContainsKey(eTileX) || !chartedTiles[eTileX].Contains(eTileY) ) {
							chartedTiles.Set2D( eTileX, eTileY );
							edgeTileQueue.Add( (eTileX, eTileY) );
							count++;
						}
					}

					if( count >= maxCount ) {
						break;
					}
				}

				edgeTiles.Clear();

				foreach( (int eTileX, int eTileY) in edgeTileQueue ) {
					Ambush.GetUnchartedNeighboringEdgeTiles( eTileX, eTileY, chartedTiles, edgeTiles );
				}

				edgeTileQueue.Clear();
			} while( edgeTiles.Count > 0 && count < maxCount );

			return count >= maxCount;
		}

		////

		private static void GetUnchartedNeighboringEdgeTiles( int eTileX, int eTileY,
					IDictionary<int, ISet<int>> chartedTiles,
					IList<(int TileX, int TileY)> newEdgeTiles ) {
			IEnumerable<(int, int)> neighbors() {
				yield return ( eTileX, eTileY - 1 );
				yield return ( eTileX - 1, eTileY );
				yield return ( eTileX + 1, eTileY );
				yield return ( eTileX, eTileY + 1 );
			};

			foreach( (int tileX, int tileY) in neighbors() ) {
				if( !chartedTiles.ContainsKey( tileX ) || !chartedTiles[tileX].Contains( tileY ) ) {
					Tile tile = Framing.GetTileSafely( tileX, tileY );

					if( TileHelpers.IsAir(tile, true, false) ) {
						newEdgeTiles.Add( (tileX, tileY) );
					}
				}
			}
		}

		////////////////

		public static void AdjustAmbushTileCenter( int tileX, ref int tileY ) {
			var mymod = AmbushesMod.Instance;
			Tile tile;
			int y = 0;

			do {
				tile = Framing.GetTileSafely( tileX, tileY + y );
				y++;
			} while( TileHelpers.IsAir(tile, true, false) );

			tileY = Math.Max( tileY, (tileY + y) - (mymod.Config.AmbushTriggerRadiusTiles / 2) );
			;
		}



		////////////////

		private IList<(int TileX, int TileY)> EdgeTiles;

		////////////////

		public int TileX { get; }
		public int TileY { get; }



		////////////////

		public Ambush( int tileX, int tileY, IList<(int TileX, int TileY)> edgeTiles ) {
			this.TileX = tileX;
			this.TileY = tileY;
			this.EdgeTiles = edgeTiles;
		}
	}
}
