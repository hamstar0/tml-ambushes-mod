﻿using Ambushes.Tiles;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes.Ambushes {
	abstract class BrambleAmbush : Ambush {
		protected BrambleAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}


		////////////////

		protected override bool RunUntil() {
			bool cleanupComplete = false;
			int duration = this.GetBrambleDuration();

			if( this.ElapsedTicks > duration ) {
				if( AmbushesMod.Config.DebugModeInfoBrambles ) {
					if( this.ElapsedTicks == (duration + 1) ) {
						Main.NewText( "Begun bramble cleanup at " + this.TileX + "," + this.TileY );
					}
				}

				if( this.ElapsedTicks % 10 == 0 ) {
					this.RunErode();
				}

				if( this.ElapsedTicks % 60 == 0 ) {
					this.RunCleanup( ref cleanupComplete );
				}
			}

			return cleanupComplete;
		}


		////

		private void RunErode() {
			if( Main.netMode == 1 ) {
				return;
			}

			int rad = AmbushesMod.Config.AmbushEntrapmentRadius + AmbushesMod.Config.BrambleThickness + 1;
			for( int i=0; i<120; i++ ) {
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, rad );
			}
		}

		private void RunCleanup( ref bool cleanupComplete ) {
			int tileX = this.TileX;
			int tileY = this.TileY;
			int rad = AmbushesMod.Config.AmbushEntrapmentRadius + AmbushesMod.Config.BrambleThickness + 1;
			IList<(int, int)> brambles = CursedBrambleTile.FindBrambles( tileX - rad, tileY - rad, tileX + rad, tileY + rad );

			if( brambles.Count > 0 ) {
				if( brambles.Count < 48 ) {
					if( Main.netMode != 1 ) {
						foreach( (int x, int y) in brambles ) {
							CursedBrambleTile.RemoveBrambleAt( x, y );
						}
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


		////////////////

		public abstract int GetBrambleDuration();
	}
}
