using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
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

		public override void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			int npcid;

			pool.Clear();

			if( spawnInfo.player.ZoneCorrupt ) {
				npcid = NPCID.DevourerHead;
			} else if( spawnInfo.player.ZoneCrimson ) {
				npcid = NPCID.GiantWormHead;
			} else if( spawnInfo.player.ZoneHoly ) {
				npcid = NPCID.GiantWormHead;
			} else if( spawnInfo.player.ZoneJungle ) {
				npcid = NPCID.GiantWormHead;
			} else if( spawnInfo.player.ZoneSnow ) {
				npcid = NPCID.GiantWormHead;
			} else if( spawnInfo.player.ZoneUndergroundDesert ) {
				npcid = NPCID.TombCrawlerHead;
			} else if( spawnInfo.player.ZoneUnderworldHeight ) {
				npcid = NPCID.BoneSerpentHead;
			} else {
				npcid = NPCID.GiantWormHead;
			}

			pool[npcid] = Main.hardMode ? 4f : 5f;

			if( Main.hardMode ) {
				if( spawnInfo.player.ZoneCorrupt ) {
					npcid = NPCID.SeekerHead;
				} else if( spawnInfo.player.ZoneCrimson ) {
					npcid = NPCID.DiggerHead;
				} else if( spawnInfo.player.ZoneHoly ) {
					npcid = NPCID.DiggerHead;
				} else if( spawnInfo.player.ZoneJungle ) {
					npcid = NPCID.DiggerHead;
				} else if( spawnInfo.player.ZoneSnow ) {
					npcid = NPCID.DiggerHead;
				} else if( spawnInfo.player.ZoneUndergroundDesert ) {
					npcid = NPCID.DuneSplicerHead;
				} else if( spawnInfo.player.ZoneUnderworldHeight ) {
					npcid = NPCID.DiggerHead;
				} else {
					npcid = NPCID.DiggerHead;
				}

				pool[npcid] = 1f;
			}
		}
	}
}
