using Ambushes.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes.Ambushes {
	abstract class BrambleAmbush : Ambush {
		protected int ElapsedTicks = 0;



		////////////////

		public BrambleAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}


		////////////////

		public abstract int GetRunDuration();


		////////////////

		public override bool Run() {
			bool cleanupComplete = false;
			int duration = this.GetRunDuration();

			if( this.ElapsedTicks > duration ) {
				this.RunErode();

				if( this.ElapsedTicks % 60 == 0 ) {
					this.RunCleanup( ref cleanupComplete );
				}
			}

			this.ElapsedTicks++;
			return cleanupComplete;
		}

		////

		private void RunErode() {
			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
			CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );
		}

		private void RunCleanup( ref bool cleanupComplete ) {
			int tileX = this.TileX;
			int tileY = this.TileY;
			int rad = AmbushesMod.Config.AmbushEntrapmentRadius + AmbushesMod.Config.BrambleThickness + 1;
			IList<(int, int)> brambles = CursedBrambleTile.FindBrambles( tileX - rad, tileY - rad, tileX + rad, tileY + rad );

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
	}
}
