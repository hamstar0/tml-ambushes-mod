using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class MinibossAmbush : MobAmbush {
		private NPC Miniboss;


		////////////////

		public override float SpawnWeight => 0.25f;



		////////////////

		private MinibossAmbush() : base( 0, 0, false ) {
		}

		protected MinibossAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush Clone( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			return new MinibossAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override int GetSpawnsDuration() {
			return this.GetBrambleDuration();
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "An imposing presence approaches...", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
			if( this.Miniboss == null ) {
				spawnRate = 30;
			}
		}

		public override void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			if( this.Miniboss == null ) {
				return;
			}

			pool.Clear();
			pool[ this.Miniboss.type ] = 1f;
		}

		public override void NPCPreAIForMobs( NPC npc ) {
			if( this.Miniboss == null ) {
				this.Miniboss = npc;

				float sizeScale = 3f;
				float lifeScale = 6f;
				float damageScale = 3f;

				npc.scale *= sizeScale;
				npc.lifeMax = (int)( (float)npc.lifeMax * lifeScale );
				npc.life = (int)( (float)npc.life * lifeScale );
				npc.damage = (int)( (float)npc.damage * damageScale );
			}
		}
	}
}
