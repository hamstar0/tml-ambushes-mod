using Ambushes.Tiles;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes.Ambushes {
	class BrambleWallAmbush : BrambleAmbush {
		protected bool IsHorizontal;


		////////////////

		public override float SpawnWeight => 2f;

		public override bool PlaysMusic => false;



		////////////////

		private BrambleWallAmbush() : base( 0, 0 ) {
			this.IsHorizontal = TmlHelpers.SafelyGetRand().NextBool();
		}

		protected BrambleWallAmbush( int tileX, int tileY, bool isHorizontal ) : base( tileX, tileY ) {
			this.IsHorizontal = isHorizontal;
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isHorizontal = TmlHelpers.SafelyGetRand().NextBool();
			return new BrambleWallAmbush( tileX, tileY, isHorizontal );
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "Look out!", Color.Red );

			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileWall( clearTileX, clearTileY, radius, this.IsHorizontal );

			CursedBrambleTile.CreateBramblesAt( tileTrace );

			return true;
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override int GetBrambleDuration() {
			return AmbushesMod.Config.BrambleWallTickDurationUntilErosionBegin;
		}
	}
}
