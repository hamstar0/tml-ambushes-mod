using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		public abstract float SpawnWeight { get; }



		////////////////

		protected abstract Ambush Clone( int tileX, int tileY );


		////////////////

		public abstract bool OnActivate( int clearTileX, int clearTileY );


		public abstract void OnDeactivate();


		public abstract bool Run();


		////////////////

		public virtual void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) { }


		public virtual void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) { }


		public virtual void PreAI( NPC npc ) { }
	}
}
