using Ambushes.Tiles;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes.Ambushes {
	abstract class BrambleEnclosureAmbush : BrambleAmbush {
		public bool IsEntrapping { get; private set; }



		////////////////

		protected BrambleEnclosureAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY ) {
			this.IsEntrapping = isEntrapping;
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			this.CreateBrambleEnclosure();

			return true;
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override int GetBrambleDuration() {
			return AmbushesMod.Config.BrambleEnclosureTickDurationUntilErosionBegin;
		}

		////////////////

		protected override bool RunUntil() {
			bool isBramblesDone = base.RunUntil();
			return isBramblesDone || !this.IsEntrapping;
		}


		////////////////

		protected void CreateBrambleEnclosure() {
			if( this.IsEntrapping ) {
				int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
				IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileEnclosure( this.TileX, this.TileY, radius );

				CursedBrambleTile.CreateBramblesAt( tileTrace );
			}
		}
	}
}
