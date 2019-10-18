using Ambushes.Tiles;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes.Ambushes {
	abstract class BrambleEnclosureAmbush : BrambleAmbush {
		public bool IsEntrapping { get; private set; }



		////////////////

		public BrambleEnclosureAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY ) {
			this.IsEntrapping = isEntrapping;
		}


		////////////////

		public override bool OnActivate( int openTileX, int openTileY ) {
			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileEnclosure( openTileX, openTileY, radius );

			CursedBrambleTile.CreateBramblesAt( tileTrace );

			return true;
		}

		public override void OnDeactivate() {
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
