using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class FlyerSwarmAmbush : MobAmbush {
		public override float SpawnWeight => 0.5f;



		////////////////

		private FlyerSwarmAmbush() : base( 0, 0, false ) {
		}

		protected FlyerSwarmAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2(tileX<<4, tileY<<4) );

			return new FlyerSwarmAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override int GetSpawnsDuration() {
			return this.GetBrambleDuration();
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "The air beats wildly with the flapping of wings...", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			int npcid;

			pool.Clear();

			if( spawnInfo.player.ZoneCorrupt ) {
				npcid = NPCID.Slimer;
			} else if( spawnInfo.player.ZoneCrimson ) {
				npcid = NPCID.CaveBat;
			} else if( spawnInfo.player.ZoneHoly ) {
				npcid = NPCID.GiantBat;
			} else if( spawnInfo.player.ZoneJungle ) {
				npcid = NPCID.JungleBat;
			} else if( spawnInfo.player.ZoneSnow ) {
				npcid = NPCID.IceBat;
			} else if( spawnInfo.player.ZoneUndergroundDesert ) {
				npcid = NPCID.CaveBat;
			} else if( spawnInfo.player.ZoneUnderworldHeight ) {
				npcid = NPCID.Hellbat;
			} else {
				npcid = NPCID.CaveBat;
			}

			pool[npcid] = Main.hardMode ? 4f : 5f;

			if( Main.hardMode ) {
				if( spawnInfo.player.ZoneCorrupt ) {
					npcid = NPCID.GiantBat;
				} else if( spawnInfo.player.ZoneCrimson ) {
					npcid = NPCID.GiantBat;
				} else if( spawnInfo.player.ZoneHoly ) {
					npcid = NPCID.IlluminantBat;
				} else if( spawnInfo.player.ZoneJungle ) {
					npcid = NPCID.GiantFlyingFox;
				} else if( spawnInfo.player.ZoneSnow ) {
					npcid = NPCID.GiantBat;
				} else if( spawnInfo.player.ZoneUndergroundDesert ) {
					npcid = NPCID.GiantBat;
				} else if( spawnInfo.player.ZoneUnderworldHeight ) {
					npcid = NPCID.Lavabat;
				} else {
					npcid = NPCID.GiantBat;
				}

				pool[npcid] = 1f;
			}
		}

		public override void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
			spawnRate *= 10;
			maxSpawns *= 10;
		}
	}
}
