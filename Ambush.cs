using Ambushes.Tiles;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		public int TileX { get; }
		public int TileY { get; }

		////

		public int TriggerPlayer { get; private set; } = -1;


		////////////////

		private int ElapsedTicks = 0;



		////////////////

		public Ambush( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}


		////////////////

		internal void Run( out bool cleanupComplete ) {
			int duration = AmbushesMod.Config.BrambleTickDurationUntilErosionBegin;

			cleanupComplete = false;

			this.ElapsedTicks++;

			if( this.ElapsedTicks > duration ) {
				if( this.ElapsedTicks == duration ) {
					Main.NewText( "ambush ended" );
				}

				int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );

				if( this.ElapsedTicks % 60 == 0 ) {
					this.RunCleanup( ref cleanupComplete );
				}
			}
		}

		private void RunCleanup( ref bool cleanupComplete ) {
			int tileX = this.TileX;
			int tileY = this.TileY;
			int rad = AmbushesMod.Config.AmbushEntrapmentRadius + AmbushesMod.Config.BrambleThickness + 1;
			IList<(int, int)> brambles = CursedBrambleTile.FindBrambles( tileX-rad, tileY-rad, tileX+rad, tileY+rad );

			if( brambles.Count > 0 ) {
				if( brambles.Count < 48 ) {
					foreach( (int x, int y) in brambles ) {
						CursedBrambleTile.RemoveBrambleAt( x, y );
					}
					cleanupComplete = true;
				}
			} else {
				cleanupComplete = true;
			}

			if( cleanupComplete ) {
				if( AmbushesMod.Config.DebugModeInfoBrambles ) {
					Main.NewText( "Brambles at " + tileX + ", " + tileY + " cleaned up." );
				}
			}
		}

		////

		public void End() { }


		////////////////

		public bool Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return false;
			}

			this.TriggerPlayer = player.whoAmI;

			int tileX = point.Value.x;
			int tileY = point.Value.y;

			return this.OnActivate( point.Value.x, point.Value.y );
		}
	}
}
