using Ambushes.Tiles;
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

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileEnclosure( clearTileX, clearTileY, radius );

			CursedBrambleTile.CreateBramblesAt( tileTrace );

			return true;
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override int GetRunDuration() {
			return AmbushesMod.Config.BrambleEnclosureTickDurationUntilErosionBegin;
		}

		////////////////

		public override bool Run() {
			if( !this.IsEntrapping ) {
				return true;
			}

			return base.Run();
		}
	}
}
