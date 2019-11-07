using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		public static bool CheckForAmbushElegibility( int tileX, int tileY ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			int maxChartedTiles = 48 * 48;

			IList<(ushort, ushort)> unclosedTiles = new List<(ushort, ushort)>();
			IList<(ushort, ushort)> chartedTiles = TileFinderHelpers.GetAllContiguousMatchingTiles(
				pattern: myworld.AmbushMngr.ViableAmbushTilePattern,
				tileX: tileX,
				tileY: tileY,
				unclosedTiles: out unclosedTiles,
				maxRadius: 50,
				maxTiles: maxChartedTiles
			);

			return chartedTiles.Count >= maxChartedTiles;
		}

		////////////////

		public static void AdjustAmbushTileCenter( int tileX, ref int tileY ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			int y = 0;

			do {
				if( (tileY + y) >= Main.maxTilesY - 1 ) {
					//LogHelpers.WarnOnce( "Exceeded height check ("+tileX+", "+tileY+")" );
					y = 0;
					break;
				}

				y++;
			} while( myworld.AmbushMngr.ViableAmbushTilePattern.Check(tileX, tileY+y) );

			tileY = Math.Max( tileY, (tileY + y) - (AmbushesMod.Config.AmbushTriggerRadiusTiles / 2) );
		}
	}
}
