﻿using System;
using Terraria;


namespace Ambushes.Ambushes {
	class BrambleCleanupOnlyAmbush : BrambleAmbush {
		public override float WorldGenWeight => 0f;

		public override bool PlaysMusic => false;



		////////////////

		public BrambleCleanupOnlyAmbush() : base( 0, 0 ) {
		}

		public BrambleCleanupOnlyAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			return new BrambleCleanupOnlyAmbush( tileX, tileY );
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			return true;
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override int GetBrambleDuration() {
			return 0;
		}
	}
}
