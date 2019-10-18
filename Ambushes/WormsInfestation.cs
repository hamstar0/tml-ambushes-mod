using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class WormsInfestationAmbush : BrambleEnclosureAmbush {
		public override float SpawnWeight => 0.5f;



		////////////////

		private WormsInfestationAmbush() : base( 0, 0, false ) {
		}

		protected WormsInfestationAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		protected override Ambush Clone( int tileX, int tileY ) {
			return new WormsInfestationAmbush( tileX, tileY, false );
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "The earth trembles...", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
