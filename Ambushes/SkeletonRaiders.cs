using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class SkeletonRaidersAmbush : BrambleEnclosureAmbush {
		public override float SpawnWeight => 0.5f;



		////////////////

		private SkeletonRaidersAmbush() : base( 0, 0, false ) {
		}

		protected SkeletonRaidersAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////////////////

		protected override Ambush Clone( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			return new SkeletonRaidersAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "Death from above!", Color.DarkOrange );

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
