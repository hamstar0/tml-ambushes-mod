using Ambushes.Tiles;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class Ambush {
		public int TileX { get; }
		public int TileY { get; }

		public bool IsEntrapping { get; private set; }


		////////////////

		private int ElapsedTicks = 0;



		////////////////

		public Ambush( int tileX, int tileY, bool isEntrapping ) {
			this.TileX = tileX;
			this.TileY = tileY;
			this.IsEntrapping = isEntrapping;
		}


		////////////////

		internal void Run( out bool cleanupComplete ) {
			cleanupComplete = false;

			this.ElapsedTicks++;

			if( this.ElapsedTicks > 60 * 20 ) {
				if( this.ElapsedTicks == 60 * 20 ) {
					Main.NewText( "ambush ended" );
				}

				int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
				CursedBrambleTile.ErodeRandomBrambleWithinRadius( this.TileX, this.TileY, radius );

				if( this.ElapsedTicks % 60 == 0 ) {
					this.RunCleanup( ref cleanupComplete );
				}
			}
		}

		private void RunCleanup( ref bool cleanupComplete ) {
			int tileX = this.TileX;
			int tileY = this.TileY;
			int rad = AmbushesMod.Config.AmbushEntrapmentRadius;
			IList<(int, int)> brambles = CursedBrambleTile.FindBrambles( tileX-rad, tileY-rad, tileX+rad, tileY+rad );

			if( brambles.Count > 0 ) {
				if( brambles.Count < 24 ) {
					foreach( (int x, int y) in brambles ) {
						CursedBrambleTile.RemoveBrambleAt( x, y );
					}
					cleanupComplete = true;
				}
			} else {
				cleanupComplete = true;
			}
		}

		////

		public void End() { }


		////////////////

		public void Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return;
			}
			int tileX = point.Value.x;
			int tileY = point.Value.y;

			if( this.IsEntrapping ) {
Main.NewText("placing brambles at "+tileX+":"+tileY);
				int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
				IDictionary<int, ISet<int>> edgeTiles = CursedBrambleTile.CreateBrambleEnclosure( tileX, tileY, radius );

				CursedBrambleTile.CreateBramblesAt( edgeTiles );
			}
		}
	}
}
