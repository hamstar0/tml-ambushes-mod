using HamstarHelpers.Classes.DataStructures;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class SkeletonRaidersAmbush : MobAmbush {
		public static readonly ISet<int> NormalPreHardModeSkeletons;
		public static readonly ISet<int> ExpertPreHardModeSkeletons;
		public static readonly ISet<int> HardModeSkeletons;

		public static readonly ISet<int> PreHardModeRareSkeletons;
		public static readonly ISet<int> HardModeRareSkeletons;

		public static readonly ISet<int> PreHardModeSnowSkeletons;
		public static readonly ISet<int> HardModeSnowSkeletons;

		private static readonly ISet<int> AllSkeletons;

		////

		static SkeletonRaidersAmbush() {
			SkeletonRaidersAmbush.NormalPreHardModeSkeletons = new ReadOnlySet<int>( new HashSet<int> { -46, -47, -48, -49, -50, -51, -52, -53, 21, 201, 202, 203 } );
			SkeletonRaidersAmbush.ExpertPreHardModeSkeletons = new ReadOnlySet<int>( new HashSet<int> { -46, -47, -48, -49, -50, -51, -52, -53, 449, 450, 451, 452 } );
			SkeletonRaidersAmbush.HardModeSkeletons = new ReadOnlySet<int>( new HashSet<int> { NPCID.HeavySkeleton, NPCID.ArmoredSkeleton, NPCID.SkeletonArcher } );
			SkeletonRaidersAmbush.PreHardModeRareSkeletons = new ReadOnlySet<int>( new HashSet<int> { NPCID.Tim } );
			SkeletonRaidersAmbush.HardModeRareSkeletons = new ReadOnlySet<int>( new HashSet<int> { NPCID.RuneWizard } );
			SkeletonRaidersAmbush.PreHardModeSnowSkeletons = new ReadOnlySet<int>( new HashSet<int> { NPCID.UndeadViking } );
			SkeletonRaidersAmbush.HardModeSnowSkeletons = new ReadOnlySet<int>( new HashSet<int> { NPCID.ArmoredViking } );

			SkeletonRaidersAmbush.AllSkeletons = new HashSet<int>( SkeletonRaidersAmbush.NormalPreHardModeSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.ExpertPreHardModeSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.HardModeSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.PreHardModeRareSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.HardModeRareSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.PreHardModeSnowSkeletons );
			SkeletonRaidersAmbush.AllSkeletons.UnionWith( SkeletonRaidersAmbush.HardModeSnowSkeletons );
		}



		////////////////

		public override float SpawnWeight => AmbushesMod.Config.SkeletonRaidersAmbushPriorityWeight;



		////////////////

		private SkeletonRaidersAmbush() : base( 0, 0, false ) {
		}

		protected SkeletonRaidersAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2(tileX<<4, tileY<<4) );

			return new SkeletonRaidersAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override int GetSpawnsDuration() {
			return this.GetBrambleDuration();
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "Death from above!", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			pool.Clear();

			if( !Main.expertMode ) {
				foreach( int npcid in SkeletonRaidersAmbush.NormalPreHardModeSkeletons ) {
					pool[npcid] = 3f;
				}
			} else {
				foreach( int npcid in SkeletonRaidersAmbush.ExpertPreHardModeSkeletons ) {
					pool[npcid] = 3f;
				}
			}

			foreach( int npcid in SkeletonRaidersAmbush.PreHardModeRareSkeletons ) {
				pool[npcid] = 0.15f;
			}

			if( Main.hardMode ) {
				foreach( int npcid in SkeletonRaidersAmbush.HardModeSkeletons ) {
					pool[npcid] = 3f;
				}
				foreach( int npcid in SkeletonRaidersAmbush.HardModeRareSkeletons ) {
					pool[npcid] = 0.15f;
				}
			}

			if( this.TriggeringPlayer >= 0 && (Main.player[this.TriggeringPlayer]?.ZoneSnow ?? false) ) {
				if( !Main.hardMode ) {
					foreach( int npcid in SkeletonRaidersAmbush.PreHardModeSnowSkeletons ) {
						pool[npcid] = 3f;
					}
				} else {
					foreach( int npcid in SkeletonRaidersAmbush.HardModeSnowSkeletons ) {
						pool[npcid] = 3f;
					}
				}
			}
		}

		public override void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
			spawnRate = (int)( (float)spawnRate / AmbushesMod.Config.SkeletonRaidersAmbushSpawnWeight );
			maxSpawns = (int)( (float)maxSpawns * AmbushesMod.Config.SkeletonRaidersAmbushSpawnWeight );
		}

		////

		protected override void OnClaimNPC( NPC npc ) {
			if( SkeletonRaidersAmbush.AllSkeletons.Contains( npc.type ) ) {
				if( !this.ValidateRaider( npc ) ) {
					NPCHelpers.Remove( npc );
				}
			}
		}


		////////////////

		public bool ValidateRaider( NPC npc ) {
			return npc.Center.Y < (this.TileY<<4);
		}
	}
}
