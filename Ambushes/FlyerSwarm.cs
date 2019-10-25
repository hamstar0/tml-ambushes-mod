using HamstarHelpers.Classes.DataStructures;
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
		public static readonly ISet<int> PreHardModeFlyers;
		public static readonly ISet<int> HardModeFlyers;
		private static readonly ISet<int> AllFlyers;

		////

		static FlyerSwarmAmbush() {
			FlyerSwarmAmbush.PreHardModeFlyers = new ReadOnlySet<int>( new HashSet<int> {
				NPCID.Slimer,
				NPCID.CaveBat,
				NPCID.GiantBat,
				NPCID.JungleBat,
				NPCID.IceBat,
				NPCID.Hellbat
			} );
			FlyerSwarmAmbush.HardModeFlyers = new ReadOnlySet<int>( new HashSet<int> {
				NPCID.GiantBat,
				NPCID.IlluminantBat,
				NPCID.GiantFlyingFox,
				NPCID.Lavabat
			} );

			FlyerSwarmAmbush.AllFlyers = new HashSet<int>( FlyerSwarmAmbush.PreHardModeFlyers );
			FlyerSwarmAmbush.AllFlyers.UnionWith( FlyerSwarmAmbush.HardModeFlyers );
		}



		////////////////

		public override float WorldGenWeight => AmbushesMod.Config.FlyerSwarmAmbushPriorityWeight;



		////////////////

		private FlyerSwarmAmbush() : base( 0, 0, false ) {
		}

		protected FlyerSwarmAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = AmbushesMod.Config.AmbushEntrapmentOdds <= 0
				? false
				: TmlHelpers.SafelyGetRand().Next( AmbushesMod.Config.AmbushEntrapmentOdds ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2( tileX << 4, tileY << 4 ) );

			return new FlyerSwarmAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}

		////

		protected override bool RunUntil() {
			return base.RunUntil();
		}


		////////////////

		public override int GetNPCSpawnsDuration() {
			return this.GetBrambleDuration();
		}

		public override float GetNPCSpawnWeight() {
			return base.GetNPCSpawnWeight() * AmbushesMod.Config.FlyerswarmAmbushSpawnWeight;
		}

		public override void ShowMessage() {
			Main.NewText( "A rush of wings can be heard...", Color.DarkOrange );
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


		////

		protected override bool PreClaimNPC( NPC npc ) {
			return FlyerSwarmAmbush.AllFlyers.Contains( npc.type );
		}
	}
}
