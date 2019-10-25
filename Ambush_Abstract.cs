using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		public abstract float WorldGenWeight { get; }

		public abstract bool PlaysMusic { get; }



		////////////////

		protected abstract Ambush CloneRandomized( int tileX, int tileY );


		////////////////

		protected abstract bool OnActivate( int clearTileX, int clearTileY );


		protected abstract void OnDeactivate();


		protected abstract bool RunUntil();


		////////////////

		public virtual float GetNPCSpawnWeight() {
			float undergroundTileY = this.TileY - WorldHelpers.DirtLayerTopTileY;
			float rangeY = WorldHelpers.UnderworldLayerBottomTileY - WorldHelpers.DirtLayerTopTileY;
			float depthPercent = undergroundTileY / rangeY;
			
			float weight = AmbushesMod.Config.DefaultNPCSpawnWeight
				+ (depthPercent * AmbushesMod.Config.DefaultNPCSpawnWeightPerDepthPercent);
			if( Main.hardMode ) {
				weight *= AmbushesMod.Config.DefaultNPCSpawnWeightPerHardMode;
			}
			if( NPC.downedPlantBoss ) {
				weight *= AmbushesMod.Config.DefaultNPCSpawnWeightPostPlantera;
			}
			if( NPC.downedMoonlord ) {
				weight *= AmbushesMod.Config.DefaultNPCSpawnWeightPostMoonLord;
			}

			return weight;
		}


		////////////////

		public virtual void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) { }


		public virtual void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) { }

		////

		protected virtual void OnClaimNPC( NPC npc ) { }

		protected virtual void UpdateNPCForAmbush( NPC npc ) { }
	}
}
