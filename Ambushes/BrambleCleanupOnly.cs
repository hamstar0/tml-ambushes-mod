using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class BrambleCleanupOnlyAmbush : BrambleAmbush {
		public override float SpawnWeight => 0f;



		////////////////

		public BrambleCleanupOnlyAmbush() : base( 0, 0 ) {
		}

		public BrambleCleanupOnlyAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}

		////////////////

		protected override Ambush Clone( int tileX, int tileY ) {
			return new BrambleCleanupOnlyAmbush( tileX, tileY );
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			return true;
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override int GetBrambleDuration() {
			return 0;
		}

		////////////////

		public override bool Run() {
			return base.Run();
		}


		////////////////

		public override void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
