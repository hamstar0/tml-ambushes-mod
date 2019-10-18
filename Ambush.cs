using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using System;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		public int TileX { get; }
		public int TileY { get; }

		////

		public int TriggeringPlayer { get; private set; } = -1;



		////////////////

		public Ambush( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}


		////////////////

		public bool Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return false;
			}

			this.TriggeringPlayer = player.whoAmI;

			return this.OnActivate( point.Value.x, point.Value.y );
		}
	}
}
