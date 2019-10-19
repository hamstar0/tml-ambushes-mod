using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class FlyerSwarmAmbush : BrambleEnclosureAmbush {
		public override float SpawnWeight => 0.5f;



		////////////////

		private FlyerSwarmAmbush() : base( 0, 0, false ) {
		}

		protected FlyerSwarmAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		protected override Ambush Clone( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			return new FlyerSwarmAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "The air beats wildly with the flapping of wings...", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			pool.Clear();

			if( spawnInfo.player.ZoneCorrupt ) {
				int npcid = Main.hardMode ? NPCID.GiantBat : NPCID.CaveBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneCrimson ) {
				int npcid = Main.hardMode ? NPCID.GiantBat : NPCID.CaveBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneHoly ) {
				int npcid = Main.hardMode ? NPCID.IlluminantBat : NPCID.GiantBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneJungle ) {
				int npcid = Main.hardMode ? NPCID.GiantFlyingFox : NPCID.JungleBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneSnow ) {
				int npcid = Main.hardMode ? NPCID.GiantBat : NPCID.IceBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneUndergroundDesert ) {
				int npcid = Main.hardMode ? NPCID.GiantBat : NPCID.CaveBat;
				pool[npcid] = 5f;
			} else if( spawnInfo.player.ZoneUnderworldHeight ) {
				int npcid = Main.hardMode ? NPCID.Lavabat : NPCID.Hellbat;
				pool[npcid] = 5f;
			} else {
				int npcid = Main.hardMode ? NPCID.GiantBat : NPCID.CaveBat;
				pool[npcid] = 5f;
			}
		}
	}
}
